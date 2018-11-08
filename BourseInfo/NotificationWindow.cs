using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PortfolioManagement;

namespace BourseInfo
{
    public partial class NotificationWindow : Form
    {
        private List<String> _stockIdList;
        private MainForm _mainForm;

        public List<String> StockList => _stockIdList;

        public void RefreshContent(List<Stock> stockData)
        {
            tableLayoutPanel.Controls.Clear();
            foreach (var id in _stockIdList)
            {
                UserControlStock control = new UserControlStock(id);

                var s = stockData.Find(e => e.Id == id);

                if (s != null)
                {
                    control.lName.Text = s.Name;
                    control.lValue.Text = Math.Round(s.Value, 3, MidpointRounding.AwayFromZero).ToString("G29") + "€"; // G29 to remove last zero if it is zero.
                    control.lPct.Text = Math.Round(s.Pct, 2) + "%";

                    if (s.Pct > 0)
                    {
                        control.lPct.ForeColor = Color.LimeGreen;
                        control.lPct.Text = "+" + control.lPct.Text;
                    }
                    else if (s.Pct < 0)
                    {
                        control.lPct.ForeColor = Color.Tomato;
                    }

                    tableLayoutPanel.Controls.Add(control);
                }
            }
        }

        public void AddStock(String stockId)
        {
            _stockIdList.Add(stockId);
        }

        public void RemoveStock(String stockId)
        {
            _stockIdList.Remove(stockId);
        }

        public NotificationWindow():this(new List<string>(), null)
        {
        }

        public NotificationWindow(List<String> stockIdList, MainForm mainForm)
        {
            _stockIdList = stockIdList;

            InitializeComponent();

            BindControlClicks(this);
            StartPosition = FormStartPosition.Manual;

            var wa = Screen.PrimaryScreen.WorkingArea;
            this.Location = new Point((int)Math.Floor((wa.Right - this.Width) * 0.99), (int)Math.Floor((wa.Bottom - this.Height) * 0.99));

            _mainForm = mainForm;

        }

        private void RefreshLocation()
        {
            var wa = Screen.PrimaryScreen.WorkingArea;
            this.Location = new Point((int)Math.Floor((wa.Right - this.Width) * 0.99), (int)Math.Floor((wa.Bottom - this.Height) * 0.99));
        }

        private void NotificationWindow_Paint(object sender, PaintEventArgs e)
        {
            //e.Graphics.DrawRectangle(new Pen(Color.DarkSlateGray,2), this.DisplayRectangle.X, this.DisplayRectangle.Y, this.DisplayRectangle.Width - 1, this.DisplayRectangle.Height - 1);
        }

        private void NotificationWindow_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible) // just made visible by click event
            {
                notificationFadeEffect(fadeOut: false);
                RefreshLocation();
            }
            else
            {
                //notificationFadeEffect(fadeOut: true);
            }
        }

        private void notificationFadeEffect(bool fadeOut)
        {
            int duration = 100; //in milliseconds
            int steps = 10;
            Timer timer = new Timer { Interval = duration / steps };

            int currentStep = 0;
            timer.Tick += (arg1, arg2) =>
            {
                if (!fadeOut)
                    Opacity = (double)currentStep / steps;
                else
                    Opacity = 1 - ((double)currentStep / steps);
                currentStep++;

                if (currentStep >= steps)
                {
                    timer.Stop();
                    timer.Dispose();
                }
            };

            timer.Start();

        }

        //public delegate void GlobalClickEventHander(object sender, EventArgs e);
        [Category("Action")]
        [Description("Fires when any control on the form is clicked.")]
        //public event GlobalClickEventHander GlobalClick;

        private void BindControlClicks(Control con)
        {
            con.MouseClick += delegate (object sender, MouseEventArgs e)
            {
                TriggerClicked(sender, e);
            };

            // bind to controls already added
            foreach (Control i in con.Controls)
            {
                BindControlClicks(i);
            }

            // bind to controls added in the future
            con.ControlAdded += delegate (object sender, ControlEventArgs e)
            {
                BindControlClicks(e.Control);
            };
        }

        private void TriggerClicked(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            //if (GlobalClick != null)
            {
                this.Hide();
            }
            else if (e.Button == MouseButtons.Right)
            {
                contextMenuStripNotif.Show(Cursor.Position);
                contextMenuStripNotif.Tag = sender;
            }
        }

        private void removeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                UserControlStock c = (UserControlStock) ((Control) contextMenuStripNotif.Tag).Parent;
                _mainForm.RemoveStockFromNotif(c.Id);
                RefreshLocation();
            }
            catch (Exception)
            {
                
            }
        }

        private void openBrowserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                UserControlStock c = (UserControlStock)((Control)contextMenuStripNotif.Tag).Parent;
                Stock s = _mainForm.GetStockById(c.Id);
                System.Diagnostics.Process.Start("https://www.boursorama.com/cours/1rP" + s.Ticker);
            }
            catch (Exception)
            {

            }
        }
    }
}
