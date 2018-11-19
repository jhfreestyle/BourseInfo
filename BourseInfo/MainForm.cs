using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PortfolioManagement;


namespace BourseInfo
{
    public partial class MainForm : Form
    {
        private NotificationWindow _notificationWindow;
        private List<Stock> _stockList;
        private const int NumberOfRetries = 1;
        private const int DelayOnRetry = 10000; // in milliseconds
        private const int RequestTimeout = 20000;
        private const string LogFilePath = @"log.txt";

        public readonly List<String> JsonUrls = new List<String>
        {
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
            InitializeComponent();
            LoadFile();

            myNotifyIcon.Visible = false;
            pictureBox1.Visible = false;

            List<String> idList = new List<String>();
            foreach (DataRow r in StocksInNotifTable.Rows)
            {
               idList.Add(r["Id"].ToString());
            }
            _notificationWindow = new NotificationWindow(idList, this);

            InitializeComboBoxTime();
        }

        private void InitializeComboBoxTime()
        {
                        comboBoxTime.DisplayMember = "Text";
                        comboBoxTime.ValueMember = "Value";

                        comboBoxTime.DataSource = new[]
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
                        comboBoxTime.SelectedIndex = 0;

        }

        public string getHttpResponse(string url)
        {
            string result = null;
            for (int i = 1; i <= NumberOfRetries; ++i)
            {
                WebRequest request = WebRequest.Create(url);
                request.Timeout = RequestTimeout;
                result = String.Empty;
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
                    LogException(ex);

                    //if (i == NumberOfRetries)
                    //{
                    //    throw;
                    //}

                    if (NumberOfRetries > 1)
                        Thread.Sleep(DelayOnRetry);
                }
            }
            return result;
        }

        private void buttonRefresh_Click(object sender, EventArgs e)
        {
            _stockList = new List<Stock>();
            loadAllData();
            _notificationWindow.RefreshContent(_stockList);
            updateValo();
        }

        private void refreshSelectedStockList(object sender, EventArgs e)
        {
            if (_stockList != null)
            {
                try
                {
                    refreshSelectedStockList();
                    _notificationWindow.RefreshContent(_stockList);
                }
                catch (Exception ex)
                {
                    LogException(ex);
                }
            }
        }

        private void updateValo()
        {
            Portfolio p = new Portfolio();
            p.Ledger = new List<Transaction>();
            Stock s1 = _stockList.First(s => s.Name == "Claranova");
            Stock s2 = _stockList.First(s => s.Name == "Lagardère");
            p.Ledger.Add(new Transaction(s1, 100, 2));
            p.Ledger.Add(new Transaction(s2, 200, 1));

            label_valo.Text = p.GetPortfolioValue() + " €";
        }

        private void refreshOneStock(string id)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            string res = getHttpResponse("https://api.lecho.be/services/stocks?quotes=urn:issue:" + id);

            dynamic json = JObject.Parse(res);
            var updatedStock = json.results[0];

            Stock currentStock = _stockList.FirstOrDefault(s => s.Id == id);

            if (currentStock != null)
            {
                currentStock.Value = updatedStock.lastPrice;
                currentStock.Pct = updatedStock.dayChangePercentage;
            }

            textBox1.Text = updatedStock.updatedOn + "\r\n";

            stopWatch.Stop();
            Debug.Print($"Update executed in {stopWatch.Elapsed.TotalMilliseconds:0} milliseconds.");

        }

        private void refreshSelectedStockList()
        {
            DateTime lastUpdateDate = DateTime.MinValue;

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            string url = "/services/stocks?quotes=urn:issue:" + String.Join(",urn:issue:", _notificationWindow.StockList);
            string res = getHttpResponse("https://api.lecho.be" + url);
            dynamic json = JObject.Parse(res);

            foreach (var updatedStock in json.results)
            {
                Stock currentStock = _stockList.FirstOrDefault(s => s.Id == GetStockId(updatedStock.issueUrn.ToString()));

                if (currentStock != null)
                {
                    currentStock.Value = updatedStock.lastPrice;
                    currentStock.Pct = updatedStock.dayChangePercentage;
                }

                if (updatedStock.updatedOn > lastUpdateDate) lastUpdateDate = updatedStock.updatedOn;
            }

            textBox1.Text = lastUpdateDate + "\r\n";
            stopWatch.Stop();
            Debug.Print($"Update executed in {stopWatch.Elapsed.TotalMilliseconds:0} milliseconds.");
        }

        private String GetStockId(string id)
        {
            return id.Replace("urn:issue:", "");
        }

        public Stock GetStockById(string id)
        {
            return _stockList.FirstOrDefault(s => s.Id.Equals(id));
        }


        private void loadAllData()
        {
            try
            {
                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();

                foreach (var url in JsonUrls)
                {
                    try
                    {
                        loadJson(url);
                    }
                    catch (Exception ex)
                    {
                        LogException(ex, url);
                    }
                }

                stopWatch.Stop();
                Debug.Print($"Loading executed in {stopWatch.Elapsed.TotalMilliseconds:0} milliseconds.");

                _stockList = _stockList.OrderBy(o => o.Name).ToList();
                stockBindingSource.DataSource = _stockList;

                companyNb.Text = _stockList.Count + " companies listed";
            }
            catch (Exception ex)
            {
                LogException(ex);
            }
        }

        private void loadJson(string url)
        {
            string res = getHttpResponse(url);
            if (!string.IsNullOrEmpty(res))
            {
                dynamic json = JObject.Parse(res);

                textBox1.Text = json.values.lastTime + "\r\n";

                foreach (var item in json.embedded.issues)
                {
                    string id = GetStockId(item.urn.ToString());

                    bool found = false;
                    foreach (var s in _stockList)
                    {
                        if (s.Id == id)
                            found = true; // dont add twice the same
                    }

                    if (!found) // add item
                    {
                        Stock stock = new Stock
                        {
                            Id = id,
                            Isin = item.isinCode,
                            Name = item.fullName._default,
                            Ticker = item.ticker,
                            Value = item.values.lastPrice == null ? -100 : item.values.lastPrice,
                            Pct = item.values.dayChangePercentage == null ? -100 : item.values.dayChangePercentage
                        };

                        _stockList.Add(stock);
                    }
                }
            }
        }

        private void dataGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == dataGridView.Columns["pctDataGridViewTextBoxColumn"].Index)
            {
                if ((Decimal) dataGridView.Rows[e.RowIndex].Cells["pctDataGridViewTextBoxColumn"].Value > 0)
                    e.CellStyle.ForeColor = Color.LawnGreen;
                else
                {
                    e.CellStyle.ForeColor = Color.Red;
                }
            }
        }

        private void SaveFile()
        {
            string filePath = "data.xml";
            dataSet.WriteXml(filePath);
        }      
        
        private void LoadFile()
        {
            dataSet.Tables.ToString();

            string filePath = "data.xml";
            if (File.Exists(filePath))
                dataSet.ReadXml(filePath);
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            if (FormWindowState.Minimized == this.WindowState)
            {
                myNotifyIcon.Visible = true;
                //myNotifyIcon.ShowBalloonTip(500, "Application", "Application minimized to tray.", ToolTipIcon.Info);
                this.Hide();
            }

            else if (FormWindowState.Normal == this.WindowState)
            {
                myNotifyIcon.Visible = false;
            }
        }

        private void myNotifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            _notificationWindow.TopMost = false;
            _notificationWindow.Hide();

            this.Show();
            this.WindowState = FormWindowState.Normal;


        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            pictureBox1.Visible = false;
        }

        private void dataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            pictureBox1.UseWaitCursor = true;

            string stockId = dataGridView.Rows[e.RowIndex].Cells["idDataGridViewTextBoxColumn"].Value.ToString();
            string imgUrl = string.Format("http://charting.vwdservices.com/tchart/tchart.aspx?user=Tijdnet&issue={0}&layout=gradient-v1&startdate=today&enddate=today&res=intraday&width=265&height=145&format=image/gif&culture=fr-BE", stockId);
            pictureBox1.Load(imgUrl);
            pictureBox1.Location = this.PointToClient(Cursor.Position);
            pictureBox1.Visible = true;

            pictureBox1.UseWaitCursor = false;
            Debug.WriteLine("ImgUrl:" + imgUrl);
        }

        private void dataGridView_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex >= 0 && e.RowIndex >= 0)
                if (e.Button == MouseButtons.Right)
                {
                    //var hit = TestCEvsDWHDataGridView.HitTest(e.X, e.Y);
                    dataGridView.ClearSelection();
                    dataGridView.Rows[e.RowIndex].Selected = true;
                    dataGridContextMenu.Show(Cursor.Position);
                    //TestCEvsDWHGenerateSQLQueryToolStripMenuItem.Text = "x=" + e.X + " y=" + e.Y + " row=" + e.RowIndex + " col=" + e.ColumnIndex;
                    //TestCEvsDWHDataGridView.Focus();
                    // we save the "id" column of the selected row to retrieve the id when clicking on the context menu).
                    dataGridView.CurrentCell = dataGridView.Rows[e.RowIndex].Cells["isinDataGridViewTextBoxColumn"];
                }
        }

        private void toolStripMenuItemAddToNotif_Click(object sender, EventArgs e)
        {
            string clickedStockIsin = dataGridView.CurrentCell.Value.ToString();

            foreach (Stock s in _stockList)
            {
                if (s.Isin == clickedStockIsin)
                {
                    _notificationWindow.AddStock(s.Id);
                    _notificationWindow.RefreshContent(_stockList);

                    DataRow row = StocksInNotifTable.NewRow();
                    row["Id"] = s.Id;
                    StocksInNotifTable.Rows.Add(row);

                }
            }
            SaveFile();
        }

        private void toolStripMenuItemRemoveFromNotif_Click(object sender, EventArgs e)
        {
            string clickedStockId = dataGridView.CurrentCell.Value.ToString();
            RemoveStockFromNotif(clickedStockId);
        }

        public void RemoveStockFromNotif(string idToRemove)
        {
                    _notificationWindow.RemoveStock(idToRemove);
                    _notificationWindow.RefreshContent(_stockList);

                    DeleteRowInStocksInNotifTable(idToRemove);
        }

        private void DeleteRowInStocksInNotifTable(string rowId)
        {
            DataRow[] rowsToDelete = StocksInNotifTable.Select("Id = '" + rowId + "'");
            if (rowsToDelete.Length > 0)
            {
                foreach (DataRow r in rowsToDelete)
                {
                    StocksInNotifTable.Rows.Remove(r);
                }
            }
            SaveFile();            

        }

        private void myNotifyIcon_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                _notificationWindow.TopMost = true;
                _notificationWindow.Show();
            }
        }

        private void comboBoxTime_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxTime.SelectedIndex == 0)
                mainTimer.Enabled = false;
            else
            {
                mainTimer.Interval = Int32.Parse(comboBoxTime.SelectedValue.ToString()) * 1000; // in milliseconds
                mainTimer.Enabled = true;
            }
        }

        public void LogException(Exception ex, string url = null)
        {
                if (!string.IsNullOrEmpty(url))
                    Log(DateTime.Now + " Error loading " + url + ": " + ex.Message + Environment.NewLine);
                else
                    Log(DateTime.Now + " Error: " + ex.Message + Environment.NewLine + "StackTrace: " + ex.StackTrace + Environment.NewLine + Environment.NewLine);
        }

        public void Log(string message)
        {
            using (StreamWriter writer = new StreamWriter(LogFilePath, true))
            {
                writer.WriteLine(message);
            }
        }

    }
}
