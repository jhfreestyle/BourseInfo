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
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows.Forms;

    using Newtonsoft.Json.Linq;

    using PortfolioManagement;

    public partial class MainForm : Form
    {
        private readonly DataTable dataTable;
        private readonly NotificationWindow notificationWindow;
        private List<Stock> stockList;
        private const int NumberOfRetries = 1;
        private const int DelayOnRetry = 10000; // in milliseconds
        private const int RequestTimeout = 20000;

        public readonly List<string> JsonUrls = new List<string>
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

            List<string> idList = new List<string>();
            foreach (DataRow r in this.StocksInNotifTable.Rows)
            {
                idList.Add(r["Id"].ToString());
            }

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

        public string GetHttpResponse(string url)
        {
            string result = null;
            for (int i = 1; i <= NumberOfRetries; ++i)
            {
                WebRequest request = WebRequest.Create(url);
                request.Timeout = RequestTimeout;
                result = string.Empty;
                try
                {
                    WebResponse response = request.GetResponse();
                    Stream data = response.GetResponseStream();
                    using (StreamReader sr = new StreamReader(data))
                    {
                        result = sr.ReadToEnd();
                    }

                    break; // success, do not retry!
                }
                catch (WebException ex)
                {
                    Log.Write(ex);

                    // if (i == NumberOfRetries)
                    // {
                    // throw;
                    // }
                    if (NumberOfRetries > 1)
                        Thread.Sleep(DelayOnRetry);
                }
            }

            return result;
        }

        private async void ButtonRefreshClick(object sender, EventArgs e)
        {
            this.stockList = new List<Stock>();
            await this.LoadAllData();
            this.notificationWindow.RefreshContent(this.stockList);
            this.UpdateValo();
        }

        private void RefreshSelectedStockList(object sender, EventArgs e)
        {
            if (this.stockList != null)
            {
                try
                {
                    this.RefreshSelectedStockList();
                    this.notificationWindow.RefreshContent(this.stockList);
                }
                catch (Exception ex)
                {
                    Log.Write(ex);
                }
            }
        }

        private void UpdateValo()
        {
            Portfolio p = new Portfolio { Ledger = new List<Transaction>() };

            Stock s1 = this.stockList.FirstOrDefault(s => s.Name == "Claranova");
            Stock s2 = this.stockList.FirstOrDefault(s => s.Name == "Lagardère");
            p.Ledger.Add(new Transaction(s1, 100, 2));
            p.Ledger.Add(new Transaction(s2, 200, 1));

            this.label_valo.Text = p.GetPortfolioValue().ToString("C");
        }

        private async void RefreshOneStock(string id)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            string res = await WebController.GetAsync("https://api.lecho.be/services/stocks?quotes=urn:issue:" + id);

            dynamic json = JObject.Parse(res);
            var updatedStock = json.results[0];

            Stock currentStock = this.stockList.FirstOrDefault(s => s.Id == id);

            if (currentStock != null)
            {
                currentStock.Value = updatedStock.lastPrice;
                currentStock.Pct = updatedStock.dayChangePercentage;
            }

            this.textBoxLastUpdate.Text = updatedStock.updatedOn + "\r\n";

            stopWatch.Stop();
            Debug.Print($"Update executed in {stopWatch.Elapsed.TotalMilliseconds:0} milliseconds.");

        }

        private async void RefreshSelectedStockList()
        {
            DateTime lastUpdateDate = DateTime.MinValue;

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            string url = "/services/stocks?quotes=urn:issue:" + string.Join(",urn:issue:", this.notificationWindow.StockList);
            string res = await WebController.GetAsync("https://api.lecho.be" + url);
            dynamic json = JObject.Parse(res);

            foreach (var updatedStock in json.results)
            {
                Stock currentStock = this.stockList.FirstOrDefault(s => s.Id == this.GetStockId(updatedStock.issueUrn.ToString()));

                if (currentStock != null)
                {
                    currentStock.Value = updatedStock.lastPrice;
                    currentStock.Pct = updatedStock.dayChangePercentage;
                }

                if (updatedStock.updatedOn > lastUpdateDate)
                {
                    lastUpdateDate = updatedStock.updatedOn;
                }
            }

            this.textBoxLastUpdate.Text = lastUpdateDate + "\r\n";
            stopWatch.Stop();
            Debug.Print($"Update executed in {stopWatch.Elapsed.TotalMilliseconds:0} milliseconds.");
        }

        private string GetStockId(string id)
        {
            return id.Replace("urn:issue:", string.Empty);
        }

        public Stock GetStockById(string id)
        {
            return this.stockList.FirstOrDefault(s => s.Id.Equals(id));
        }


        private async Task LoadAllData()
        {
            try
            {
                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();

                foreach (var url in this.JsonUrls)
                {
                    try
                    {
                        await this.LoadJson(url);
                    }
                    catch (Exception ex)
                    {
                        Log.Write(ex, url);
                    }
                }

                stopWatch.Stop();
                Debug.Print($"Loading executed in {stopWatch.Elapsed.TotalMilliseconds:0} milliseconds.");

                this.stockList = this.stockList.OrderBy(o => o.Name).ToList();

                this.dataTable.Clear();
                foreach (var s in this.stockList)
                {
                    this.dataTable.Rows.Add(s.Id, s.Isin, s.Name, s.Ticker, s.Value, s.Pct );
                }

                this.RefreshNbCompany();
            }
            catch (Exception ex)
            {
                Log.Write(ex);
            }
        }

        private async Task LoadJson(string url)
        {
            string res = await WebController.GetAsync(url);

            if (!string.IsNullOrEmpty(res))
            {
                dynamic json = JObject.Parse(res);

                this.textBoxLastUpdate.Text = json.values?.lastTime + "\r\n";

                var items = json.embedded?.issues;

                if (items != null)
                {
                    foreach (var item in items)
                    {
                        string id = this.GetStockId(item.urn.ToString());

                        bool found = false;
                        foreach (var s in this.stockList)
                        {
                            if (s.Id == id)
                            {
                                found = true; // dont add twice the same
                            }
                        }

                        if (!found)
                        {
                            // add item
                            Stock stock = new Stock
                                              {
                                                  Id = id,
                                                  Isin = item.isinCode,
                                                  Name = item.fullName._default,
                                                  Ticker = item.ticker,
                                                  Value = item.values.lastPrice == null ? -1 : item.values.lastPrice,
                                                  Pct = item.values.dayChangePercentage == null
                                                            ? -1
                                                            : item.values.dayChangePercentage,
                                                  Info = item.ToString()
                                              };

                            this.stockList.Add(stock);

                        }
                    }
                }
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
            string filePath = "data.xml";
            this.dataSet.WriteXml(filePath);
        }

        private void LoadFile()
        {
            this.dataSet.Tables.ToString();

            string filePath = "data.xml";
            if (File.Exists(filePath))
            {
                this.dataSet.ReadXml(filePath);
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

            foreach (Stock s in this.stockList)
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
            this.companyNb.Text = this.stockBindingSource.Count + this.stockBindingSource.Count > 1
                                      ? " companies"
                                      : " company";
        }
    }
}
