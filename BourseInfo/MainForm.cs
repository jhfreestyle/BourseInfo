namespace BourseInfo
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics;
    using System.Drawing;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows.Forms;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    using PortfolioManagement;

    using Utils;

    public partial class MainForm : Form
    {
        private const string DataFilePath = "data.xml";

        private readonly DataTable dataTable;
        private readonly NotificationWindow notificationWindow;
        private Dictionary<string, Stock> stockList;

        public Portfolio MyPortfolio;

        public readonly List<string> JsonUrls = 
            new List<string>
            {
                // Indices
                "https://lesechos-bourse-fo-cdn.wlb.aw.atos.net/streaming/cours/getHeaderBourse",
                // euronext, alternext, cac40, eurolist
                "https://api.lecho.be/services/stockmarketgroup/urn:stockmarketgroup:euronext.france.marchelibre/issues.json?pageSize=300",
                "https://api.lecho.be/services/stockmarketgroup/urn:stockmarketgroup:euronext.france.shares.alternext.mlst/issues.json?pageSize=300",
                "https://api.lecho.be/services/stockmarketgroup/urn:stockmarketgroup:euronext.paris.shares.cac40/issues.json?sort=issue.fullName,asc&pageSize=100",
                "https://api.lecho.be/services/stockmarketgroup/urn:stockmarketgroup:euronext.france.shares.french.compa/issues.json?sort=issue.fullName,asc&pageSize=300",
                "https://api.lecho.be/services/stockmarketgroup/urn:stockmarketgroup:euronext.france.shares.french.compb/issues.json?sort=issue.fullName,asc&pageSize=300",
                "https://api.lecho.be/services/stockmarketgroup/urn:stockmarketgroup:euronext.france.shares.french.compc/issues.json?sort=issue.fullName,asc&pageSize=300",
            };

        public MainForm()
        {
            this.InitializeComponent();

            CultureInfo info = new CultureInfo("fr-FR");
            Thread.CurrentThread.CurrentCulture = info;

            this.dataTable = new DataTable();
            this.dataTable.Columns.Add("Id");
            this.dataTable.Columns.Add("Isin");
            this.dataTable.Columns.Add("Name");
            this.dataTable.Columns.Add("Ticker");
            this.dataTable.Columns.Add("Value", typeof(decimal));
            this.dataTable.Columns.Add("Pct", typeof(decimal));

            this.stockBindingSource.DataSource = this.dataTable;

            this.LoadFile();

            this.myNotifyIcon.Visible = false;
            this.pictureBox1.Visible = false;

            List<string> idList = this.StocksInNotifTable.Rows.Cast<DataRow>().Select(x => x["Id"].ToString()).ToList();

            this.notificationWindow = new NotificationWindow(idList, this);

            this.InitializeComboBoxTime();
        }

        private void InitializeComboBoxTime()
        {
            this.comboBoxTime.DisplayMember = "Text";
            this.comboBoxTime.ValueMember = "Value";

            this.comboBoxTime.DataSource = new[]
{
                new { Text = "Refresh time", Value = "0" }, // 0 = no refresh
                new { Text = "30s", Value = "30" },
                new { Text = "45s", Value = "45" },
                new { Text = "1m", Value = "60" },
                new { Text = "2m", Value = "120" },
                new { Text = "3m", Value = "180" },
                new { Text = "4m", Value = "240" },
                new { Text = "5m", Value = "300" },
                new { Text = "10m", Value = "600" },
            };
            this.comboBoxTime.SelectedIndex = 0;
        }


        private async void ButtonRefreshClick(object sender, EventArgs e)
        {
            this.stockList = new Dictionary<string, Stock>();

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            await this.LoadAllData();

            stopWatch.Stop();
            Debug.Print($"Loading executed in {stopWatch.Elapsed.TotalMilliseconds:0} milliseconds.");

            this.LoadDataTable();

            this.RefreshNbCompany();

            this.LoadPortfolio();

            this.RefreshUI();
        }

        private async void RefreshSelectedStockList(object sender, EventArgs e)
        {
            if (this.stockList != null)
            {
                try
                {
                    await this.RefreshSelectedStockList();
                    this.RefreshUI();
                }
                catch (Exception ex)
                {
                    Log.Write(ex);
                }
            }
        }

        private void RefreshUI()
        {
            this.RefreshLastTime();
            this.notificationWindow.RefreshContent(this.stockList);
            this.RefreshValo();
        }

        private void RefreshValo()
        {
            var gainLoss = this.MyPortfolio.GetPortfolioGainLoss();
            var gainLossPct = this.MyPortfolio.GetPortfolioGainLossPct();
            var sign = gainLoss < 0 ? "-" : "+";

            this.label_valo.Text = this.MyPortfolio.GetPortfolioValue().ToString("C");
            this.label_gain.Text = "(" + sign + gainLoss.ToString("C") + " " + sign + gainLossPct.ToString("P1") + ")";
        }

        private async Task RefreshSelectedStockList()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            string url = "/services/stocks?quotes=urn:issue:" + string.Join(",urn:issue:", this.notificationWindow.StockList);
            string res = await WebController.GetStringAsync("https://api.lecho.be" + url);
            dynamic json = JObject.Parse(res);

            foreach (var updatedStock in json.results)
            {
                Stock currentStock = this.GetStockById(Stock.GetStockId(updatedStock.issueUrn.ToString()));

                if (currentStock != null)
                {
                    currentStock.Value = updatedStock.lastPrice;
                    currentStock.Pct = updatedStock.dayChangePercentage;
                    currentStock.LastUpdate = updatedStock.updatedOn;
                }
            }

            stopWatch.Stop();
            Debug.Print($"Update executed in {stopWatch.Elapsed.TotalMilliseconds:0} milliseconds.");
        }

        public Stock GetStockById(string id)
        {
            return this.stockList.Values.FirstOrDefault(s => s.Id.Equals(id));
        }

        private async Task LoadAllData()
        {
            var n = this.JsonUrls.Count;
            var tasks = new Task<List<Stock>>[n];
            var results = new List<Stock>[n];

            // Create and start the tasks
            for (int i = 0; i < n; i++)
            {
                tasks[i] = WebController.GetStocksAsync(this.JsonUrls[i]);
            }

            // Wait for each task to finish and populate stockList
            for (int i = 0; i < n; i++)
            {
                results[i] = await tasks[i];
                foreach (var s in results[i])
                {
                    this.stockList[s.Id] = s;
                }

                this.UpdateLoadingPercentage(i + 1, n);
            }
        }

        private void LoadDataTable()
        {
            this.dataTable.Clear();

            foreach (var s in this.stockList.Values.OrderBy(o => o.Name))
            {
                this.dataTable.Rows.Add(s.Id, s.Isin, s.Name, s.Ticker, s.Value, s.Pct);
            }
        }

        private void DataGridViewCellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (this.dataGridView.Columns["pctDataGridViewTextBoxColumn"] != null)
            {
                if (e.ColumnIndex == this.dataGridView.Columns["pctDataGridViewTextBoxColumn"].Index 
                    && this.dataGridView.Rows[e.RowIndex].Cells["pctDataGridViewTextBoxColumn"].Value != null)
                {
                    e.CellStyle.ForeColor =
                        (decimal)this.dataGridView.Rows[e.RowIndex].Cells["pctDataGridViewTextBoxColumn"].Value > 0
                            ? Color.LawnGreen
                            : Color.Red;
                }
            }
        }

        private void SaveFile()
        {
            this.dataSet.WriteXml(DataFilePath);
        }

        private void SavePortfolio()
        {
            var json = JsonConvert.SerializeObject(this.MyPortfolio, Formatting.Indented);
            File.WriteAllText("portfolio.json", json);
        }

        private void LoadFile()
        {
            if (File.Exists(DataFilePath))
            {
                this.dataSet.ReadXml(DataFilePath);
            }
        }

        private void LoadPortfolio()
        {
            if (File.Exists("portfolio.json"))
            {
                var json = File.ReadAllText("portfolio.json");
                this.MyPortfolio = JsonConvert.DeserializeObject<Portfolio>(json);

                foreach (var t in this.MyPortfolio.Ledger)
                {
                    t.Stock = this.stockList.Values.FirstOrDefault(s => s.Isin.EqualsIgnoreCase(t.Isin));
                }
            }
            else
            {
                this.MyPortfolio = new Portfolio();
            }
        }

        private void MainFormResize(object sender, EventArgs e)
        {
            if (FormWindowState.Minimized == this.WindowState)
            {
                this.myNotifyIcon.Visible = true;

                // myNotifyIcon.ShowBalloonTip(500, "Application", "Application minimized to tray.", ToolTipIcon.Info);
                this.Hide();
            }
            else if (FormWindowState.Normal == this.WindowState)
            {
                this.myNotifyIcon.Visible = false;
            }
        }

        private void MyNotifyIconMouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.notificationWindow.TopMost = false;
            this.notificationWindow.Hide();

            this.Show();
            this.WindowState = FormWindowState.Normal;
        }

        private void PictureBox1Click(object sender, EventArgs e)
        {
            this.pictureBox1.Visible = false;
        }

        private void DataGridViewCellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                this.pictureBox1.UseWaitCursor = true;

                string stockId = this.dataGridView.Rows[e.RowIndex].Cells["idDataGridViewTextBoxColumn"].Value.ToString();
                string imgUrl = $"http://charting.vwdservices.com/tchart/tchart.aspx?user=Tijdnet&issue={stockId}&layout=gradient-v1&startdate=today&enddate=today&res=intraday&width=265&height=145&format=image/gif&culture=fr-BE";
                this.pictureBox1.Load(imgUrl);
                this.pictureBox1.Location = this.PointToClient(Cursor.Position);
                this.pictureBox1.Visible = true;

                this.pictureBox1.UseWaitCursor = false;
                Debug.WriteLine("ImgUrl:" + imgUrl);
            }
        }

        private void DataGridViewCellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex >= 0 && e.RowIndex >= 0)
            {
                if (e.Button == MouseButtons.Right)
                {
                    this.dataGridView.ClearSelection();
                    this.dataGridView.Rows[e.RowIndex].Selected = true;
                    this.dataGridContextMenu.Show(Cursor.Position);

                    // we save the "id" column of the selected row to retrieve the id when clicking on the context menu).
                    this.dataGridView.CurrentCell = this.dataGridView.Rows[e.RowIndex].Cells["isinDataGridViewTextBoxColumn"];
                }
            }
        }

        private void ToolStripMenuItemAddToNotifClick(object sender, EventArgs e)
        {
            string clickedStockIsin = this.dataGridView.CurrentCell.Value.ToString();

            foreach (Stock s in this.stockList.Values)
            {
                if (s.Isin == clickedStockIsin)
                {
                    this.notificationWindow.AddStock(s.Id);
                    this.notificationWindow.RefreshContent(this.stockList);

                    DataRow row = this.StocksInNotifTable.NewRow();
                    row["Id"] = s.Id;
                    this.StocksInNotifTable.Rows.Add(row);

                }
            }

            this.SaveFile();
        }

        private void ToolStripMenuItemRemoveFromNotifClick(object sender, EventArgs e)
        {
            string clickedStockId = this.dataGridView.CurrentCell.Value.ToString();
            this.RemoveStockFromNotif(clickedStockId);
        }

        public void RemoveStockFromNotif(string idToRemove)
        {
            this.notificationWindow.RemoveStock(idToRemove);
            this.notificationWindow.RefreshContent(this.stockList);

            this.DeleteRowInStocksInNotifTable(idToRemove);
        }

        private void DeleteRowInStocksInNotifTable(string rowId)
        {
            DataRow[] rowsToDelete = this.StocksInNotifTable.Select("Id = '" + rowId + "'");
            if (rowsToDelete.Length > 0)
            {
                foreach (DataRow r in rowsToDelete)
                {
                    this.StocksInNotifTable.Rows.Remove(r);
                }
            }

            this.SaveFile();

        }

        private void MyNotifyIconMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                this.notificationWindow.TopMost = true;
                this.notificationWindow.Show();
            }
        }

        private void ComboBoxTimeSelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.comboBoxTime.SelectedIndex == 0)
            {
                this.mainTimer.Enabled = false;
            }
            else
            {
                this.mainTimer.Interval = int.Parse(this.comboBoxTime.SelectedValue.ToString()) * 1000; // in milliseconds
                this.mainTimer.Enabled = true;
            }
        }

        private void TextBoxSearchTextChanged(object sender, EventArgs e)
        {
            if (!this.textBoxSearch.Text.Equals("Type to search..."))
            {
                this.textBoxSearch.ResetForeColor();

                // stockBindingSource.DataSource = _stockList.Where(s => s.Name.Contains(textBoxSearch.Text)).ToList();
                this.stockBindingSource.Filter = "Name LIKE '%" + this.textBoxSearch.Text + "%' OR Ticker LIKE '%"
                                                 + this.textBoxSearch.Text + "%' OR Isin LIKE '%"
                                                 + this.textBoxSearch.Text + "%'";
                this.RefreshNbCompany();
            }
        }

        private void TextBoxSearchEnter(object sender, EventArgs e)
        {
            if (this.textBoxSearch.Text.Equals("Type to search..."))
            {
                this.textBoxSearch.Text = string.Empty;
            }
        }

        private void TextBoxSearchLeave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.textBoxSearch.Text))
            {
                this.textBoxSearch.Text = @"Type to search...";
                this.textBoxSearch.ForeColor = Color.Gray;
            }
        }

        private void RefreshNbCompany()
        {
            this.companyNb.Text = this.stockBindingSource.Count
                                  + (this.stockBindingSource.Count > 1 ? " companies" : " company");
        }

        private void RefreshLastTime()
        {
            this.textBoxLastUpdate.Text = this.stockList.Values.Max(s => s.LastUpdate) + "\r\n";
        }

        private void UpdateLoadingPercentage(int i, int n)
        {
            double p = (100.0 * i / n);
            this.buttonLoad.Text = p == 100 ? "Load Data" : $"Loading...{p:#}%";
        }

        private void buyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.BuySellAction(true);
        }

        private void sellToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.BuySellAction(false);
        }

        private void BuySellAction(bool buy)
        {
            var isin = this.dataGridView.CurrentCell.Value.ToString();
            var stock = this.stockList.Values.FirstOrDefault(s => s.Isin.EqualsIgnoreCase(isin));

            if (stock != null)
            {
                // if buy == false => sell
                using (BuySellPopup bs = new BuySellPopup(this, buy, stock))
                {
                    if (bs.ShowDialog(this) == DialogResult.OK)
                    {
                        // Add transaction in datatable

                        this.RefreshValo();
                        this.SavePortfolio();
                    }
                }
            }
        }
    }
}
