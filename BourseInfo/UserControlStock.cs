using System;
using System.Drawing;
using System.Windows.Forms;

namespace BourseInfo
{
    public sealed partial class UserControlStock : UserControl
    {
        public UserControlStock(string id)
        {
            this.InitializeComponent();
            this.ForeColor = Color.LightGray;
            this.Id = id;
        }

        public Label LName { get; set; }

        public Label LValue { get; set; }

        public Label LPct { get; set; }

        public string Id { get; }
    }
}
