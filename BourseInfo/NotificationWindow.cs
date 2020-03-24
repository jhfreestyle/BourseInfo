using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using PortfolioManagement;

namespace BourseInfo
{
    using System.Linq;

    public partial class NotificationWindow : Form
    {
        private readonly MainForm mainForm;

        public NotificationWindow() : this(new List<string>(), null)
        {
        }

        public NotificationWindow(List<string> stockIdList, MainForm mainForm)
        {
            this.StockList = stockIdList;

            this.InitializeComponent();

            this.BindControlClicks(this);
            this.StartPosition = FormStartPosition.Manual;

            var wa = Screen.PrimaryScreen.WorkingArea;
            this.Location = new Point((int) Math.Floor((wa.Right - this.Width) * 0.99),
                (int) Math.Floor((wa.Bottom - this.Height) * 0.99));

            this.mainForm = mainForm;
        }

        public List<string> StockList { get; }

        public void RefreshContent(Dictionary<string, Stock> stockData)
        {
            // update background
            this.BackColor = mainForm.Theme.NotificationBackground;

            this.tableLayoutPanel.Controls.Clear();
            foreach (var id in this.StockList)
            {
                UserControlStock control = new UserControlStock(id, mainForm.Theme);

                var s = stockData?.Values.FirstOrDefault(e => e.Id == id);

                if (s != null)
                {
                    control.LName.Text = s.Name;
                    control.LValue.Text =
                        Math.Round(s.Value, 3, MidpointRounding.AwayFromZero).ToString("G29") +
                        "€"; // G29 to remove last zero if it is zero.
                    control.LPct.Text = Math.Round(s.Pct, 2) + "%";

                    if (s.Pct > 0)
                    {
                        control.LPct.ForeColor = mainForm.Theme.Positive;
                        control.LPct.Text = "+" + control.LPct.Text;
                    }
                    else if (s.Pct < 0)
                    {
                        control.LPct.ForeColor = mainForm.Theme.Negative;
                    }

                    this.tableLayoutPanel.Controls.Add(control);
                }
            }
        }

        public void AddStock(string stockId)
        {
            this.StockList.Add(stockId);
        }

        public void RemoveStock(string stockId)
        {
            this.StockList.Remove(stockId);
        }

        private void RefreshLocation()
        {
            var wa = Screen.PrimaryScreen.WorkingArea;
            this.Location = new Point((int) Math.Floor((wa.Right - this.Width) * 0.99),
                (int) Math.Floor((wa.Bottom - this.Height) * 0.99));
        }

        private void NotificationWindowPaint(object sender, PaintEventArgs e)
        {
            // e.Graphics.DrawRectangle(new Pen(Color.DarkSlateGray,2), this.DisplayRectangle.X, this.DisplayRectangle.Y, this.DisplayRectangle.Width - 1, this.DisplayRectangle.Height - 1);
            
            var sb = new SolidBrush(Color.FromArgb(100, 100, 100, 100));
            e.Graphics.FillRectangle(sb, this.DisplayRectangle);
        }

        private void NotificationWindowVisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                // just made visible by click event
                this.NotificationFadeEffect(fadeOut: false);
                this.RefreshLocation();
            }
            else
            {
                // notificationFadeEffect(fadeOut: true);
            }
        }

        private void NotificationFadeEffect(bool fadeOut)
        {

            int duration = 100; // in milliseconds
            int steps = 10;
            Timer timer = new Timer {Interval = duration / steps};

            int currentStep = 0;
            timer.Tick += (arg1, arg2) =>
            {
                this.Opacity = !fadeOut ? (double) currentStep / steps : 1 - ((double) currentStep / steps);

                currentStep++;

                if (currentStep > steps)
                {
                    timer.Stop();
                    timer.Dispose();
                }
            };

            timer.Start();

        }

        [Category("Action")]
        [Description("Fires when any control on the form is clicked.")]
        private void BindControlClicks(Control con)
        {
            con.MouseClick += this.TriggerClicked;

            // bind to controls already added
            foreach (Control i in con.Controls)
            {
                this.BindControlClicks(i);
            }

            // bind to controls added in the future
            con.ControlAdded += delegate(object sender, ControlEventArgs e) { this.BindControlClicks(e.Control); };
        }

        private void TriggerClicked(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Hide();
            }
            else if (e.Button == MouseButtons.Right)
            {
                this.contextMenuStripNotif.Show(Cursor.Position);
                this.contextMenuStripNotif.Tag = sender;
            }
        }

        private void RemoveToolStripMenuItemClick(object sender, EventArgs e)
        {
            UserControlStock c = (UserControlStock) ((Control) this.contextMenuStripNotif.Tag).Parent;
            this.mainForm.RemoveStockFromNotif(c.Id);
            this.RefreshLocation();
        }

        private void OpenBrowserToolStripMenuItemClick(object sender, EventArgs e)
        {
            UserControlStock c = (UserControlStock) ((Control) this.contextMenuStripNotif.Tag).Parent;
            Stock s = this.mainForm.GetStockById(c.Id);
            System.Diagnostics.Process.Start("https://www.boursorama.com/cours/1rP" + s.Ticker);
        }

        private void highlightToolStripMenuItemClick(object sender, EventArgs e)
        {
            UserControlStock c = (UserControlStock) ((Control) this.contextMenuStripNotif.Tag).Parent;
            Stock s = this.mainForm.GetStockById(c.Id);
            s.Highlight = c.ToggleHighlight();

            this.mainForm.SaveFile();
        }
    }
}
