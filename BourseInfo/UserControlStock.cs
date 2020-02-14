using System;
using System.Drawing;
using System.Windows.Forms;

namespace BourseInfo
{
    public sealed partial class UserControlStock : UserControl
    {
        public UserControlStock(string id, Theme theme)
        {
            this.InitializeComponent();
            this.ForeColor = theme.Neutral;
            this.Id = id;
        }

        public Label LName { get; set; }

        public Label LValue { get; set; }

        public Label LPct { get; set; }

        public string Id { get; }

        /// <summary>
        /// Bold/light the font control. Return True if result is Bold (Highlighted)
        /// </summary>
        public bool ToggleHighlight()
        {
            var fontStyle = this.LName.Font.Bold ? FontStyle.Regular : FontStyle.Bold;

            this.LName.Font = new Font(this.LName.Font.FontFamily, this.LName.Font.Size, fontStyle);
            this.LValue.Font = new Font(this.LValue.Font.FontFamily, this.LValue.Font.Size, fontStyle);
            this.LPct.Font = new Font(this.LPct.Font.FontFamily, this.LPct.Font.Size, fontStyle);

            return this.LName.Font.Bold;
        }
    }
}
