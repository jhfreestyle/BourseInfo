using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BourseInfo
{
    public partial class UserControlStock : UserControl
    {
        public String Id;

        public UserControlStock(String _id)
        {
            InitializeComponent();
            this.ForeColor = Color.LightGray;
            Id = _id;
        }

        public Label lName
        {
            get { return labelName; }
            set { labelName = value; }
        }

        public Label lValue
        {
            get { return labelValue; }
            set { labelValue = value; }
        }

        public Label lPct
        {
            get { return labelPct; }
            set { labelPct = value; }
        }

    }
}
