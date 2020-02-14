using System.Windows.Forms;

namespace BourseInfo
{
    using System.ComponentModel;

    sealed partial class UserControlStock
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.LName = new System.Windows.Forms.Label();
            this.LValue = new System.Windows.Forms.Label();
            this.LPct = new System.Windows.Forms.Label();
            this.SuspendLayout();

            // labelName
            this.LName.AutoEllipsis = true;
            this.LName.Font = new System.Drawing.Font(
                "Microsoft Sans Serif",
                9.75F,
                System.Drawing.FontStyle.Regular,
                System.Drawing.GraphicsUnit.Point,
                ((byte)(0)));
            this.LName.Location = new System.Drawing.Point(3, 2);
            this.LName.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.LName.Name = "labelName";
            this.LName.Size = new System.Drawing.Size(95, 19);
            this.LName.TabIndex = 0;
            this.LName.Text = "abcdefgh";

            // labelValue
            this.LValue.Font = new System.Drawing.Font(
                "Microsoft Sans Serif",
                11.25F,
                System.Drawing.FontStyle.Regular,
                System.Drawing.GraphicsUnit.Point,
                ((byte)(0)));
            this.LValue.Location = new System.Drawing.Point(101, 0);
            this.LValue.Name = "labelValue";
            this.LValue.Size = new System.Drawing.Size(76, 19);
            this.LValue.TabIndex = 1;
            this.LValue.Text = "1000,000€";

            // labelPct
            this.LPct.Font = new System.Drawing.Font(
                "Microsoft Sans Serif",
                11.25F,
                System.Drawing.FontStyle.Regular,
                System.Drawing.GraphicsUnit.Point,
                ((byte)(0)));
            this.LPct.Location = new System.Drawing.Point(176, 0);
            this.LPct.Name = "labelPct";
            this.LPct.Size = new System.Drawing.Size(74, 19);
            this.LPct.TabIndex = 2;
            this.LPct.Text = "+000,00%";

            // UserControlStock
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.LPct);
            this.Controls.Add(this.LValue);
            this.Controls.Add(this.LName);
            this.Name = "UserControlStock";
            this.Size = new System.Drawing.Size(250, 20);
            this.ResumeLayout(false);
        }

        #endregion
    }
}
