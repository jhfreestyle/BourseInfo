namespace BourseInfo
{
    partial class UserControlStock
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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
            this.labelName = new System.Windows.Forms.Label();
            this.labelValue = new System.Windows.Forms.Label();
            this.labelPct = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // labelName
            // 
            this.labelName.AutoEllipsis = true;
            this.labelName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelName.Location = new System.Drawing.Point(3, 2);
            this.labelName.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(95, 19);
            this.labelName.TabIndex = 0;
            this.labelName.Text = "abcdefgh";
            // 
            // labelValue
            // 
            this.labelValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelValue.Location = new System.Drawing.Point(101, 0);
            this.labelValue.Name = "labelValue";
            this.labelValue.Size = new System.Drawing.Size(76, 19);
            this.labelValue.TabIndex = 1;
            this.labelValue.Text = "1000,000€";
            // 
            // labelPct
            // 
            this.labelPct.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPct.Location = new System.Drawing.Point(176, 0);
            this.labelPct.Name = "labelPct";
            this.labelPct.Size = new System.Drawing.Size(74, 19);
            this.labelPct.TabIndex = 2;
            this.labelPct.Text = "+000,00%";
            // 
            // UserControlStock
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.labelPct);
            this.Controls.Add(this.labelValue);
            this.Controls.Add(this.labelName);
            this.Name = "UserControlStock";
            this.Size = new System.Drawing.Size(250, 20);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label labelName;
        private System.Windows.Forms.Label labelValue;
        private System.Windows.Forms.Label labelPct;
    }
}
