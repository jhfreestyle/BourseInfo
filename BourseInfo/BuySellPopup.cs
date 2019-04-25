using System;
using System.Windows.Forms;

namespace BourseInfo
{
    using PortfolioManagement;

    public partial class BuySellPopup : Form
    {
        private readonly MainForm mainForm;

        private readonly bool buy;

        private readonly Stock stock;

        public BuySellPopup(MainForm mainForm, bool buy, Stock stock)
        {
            InitializeComponent();

            this.mainForm = mainForm;
            this.buy = buy;
            this.stock = stock;

            this.inputPrice.Value = stock.Value;
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            int q = decimal.ToInt32(this.inputQuantity.Value);
            decimal p = this.inputPrice.Value;

            if (!this.buy)
            {
                // Sell
                q *= -1;
            }
            this.mainForm.MyPortfolio.AddTransaction(this.stock, q, p);
        }
    }
}
