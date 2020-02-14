namespace BourseInfo
{
    partial class MainForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.textBoxLastUpdate = new System.Windows.Forms.TextBox();
            this.buttonLoad = new System.Windows.Forms.Button();
            this.dataGridView = new System.Windows.Forms.DataGridView();
            this.idDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.isinDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tickerDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.valueDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pctDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.stockBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.dataSet = new System.Data.DataSet();
            this.stocksNameTable = new System.Data.DataTable();
            this.columnId = new System.Data.DataColumn();
            this.columnName = new System.Data.DataColumn();
            this.HighlightColumn = new System.Data.DataColumn();
            this.StocksInNotifTable = new System.Data.DataTable();
            this.IdColumn = new System.Data.DataColumn();
            this.myNotifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.companyNb = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.dataGridContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItemAddToNotif = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemRemoveFromNotif = new System.Windows.Forms.ToolStripMenuItem();
            this.buyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sellToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mainTimer = new System.Windows.Forms.Timer(this.components);
            this.comboBoxTime = new System.Windows.Forms.ComboBox();
            this.buttonUpdate = new System.Windows.Forms.Button();
            this.label_valo = new System.Windows.Forms.Label();
            this.textBoxSearch = new System.Windows.Forms.TextBox();
            this.label_gain = new System.Windows.Forms.Label();
            this.comboBoxTheme = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.stockBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.stocksNameTable)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.StocksInNotifTable)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.dataGridContextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBoxLastUpdate
            // 
            this.textBoxLastUpdate.Location = new System.Drawing.Point(258, 11);
            this.textBoxLastUpdate.Name = "textBoxLastUpdate";
            this.textBoxLastUpdate.ReadOnly = true;
            this.textBoxLastUpdate.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxLastUpdate.Size = new System.Drawing.Size(113, 20);
            this.textBoxLastUpdate.TabIndex = 1;
            // 
            // buttonLoad
            // 
            this.buttonLoad.Location = new System.Drawing.Point(11, 11);
            this.buttonLoad.Name = "buttonLoad";
            this.buttonLoad.Size = new System.Drawing.Size(66, 23);
            this.buttonLoad.TabIndex = 2;
            this.buttonLoad.Text = "Load Data";
            this.buttonLoad.UseVisualStyleBackColor = true;
            this.buttonLoad.Click += new System.EventHandler(this.ButtonRefreshClick);
            // 
            // dataGridView
            // 
            this.dataGridView.AllowUserToAddRows = false;
            this.dataGridView.AllowUserToDeleteRows = false;
            this.dataGridView.AllowUserToResizeRows = false;
            this.dataGridView.AutoGenerateColumns = false;
            this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.idDataGridViewTextBoxColumn,
            this.isinDataGridViewTextBoxColumn,
            this.tickerDataGridViewTextBoxColumn,
            this.nameDataGridViewTextBoxColumn,
            this.valueDataGridViewTextBoxColumn,
            this.pctDataGridViewTextBoxColumn});
            this.dataGridView.DataSource = this.stockBindingSource;
            this.dataGridView.Location = new System.Drawing.Point(12, 67);
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.ReadOnly = true;
            this.dataGridView.RowHeadersVisible = false;
            this.dataGridView.Size = new System.Drawing.Size(443, 359);
            this.dataGridView.TabIndex = 4;
            this.dataGridView.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DataGridViewCellDoubleClick);
            this.dataGridView.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.DataGridViewCellFormatting);
            this.dataGridView.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.DataGridViewCellMouseDown);
            // 
            // idDataGridViewTextBoxColumn
            // 
            this.idDataGridViewTextBoxColumn.DataPropertyName = "Id";
            this.idDataGridViewTextBoxColumn.HeaderText = "Id";
            this.idDataGridViewTextBoxColumn.Name = "idDataGridViewTextBoxColumn";
            this.idDataGridViewTextBoxColumn.ReadOnly = true;
            this.idDataGridViewTextBoxColumn.Visible = false;
            // 
            // isinDataGridViewTextBoxColumn
            // 
            this.isinDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.isinDataGridViewTextBoxColumn.DataPropertyName = "Isin";
            this.isinDataGridViewTextBoxColumn.HeaderText = "Isin";
            this.isinDataGridViewTextBoxColumn.Name = "isinDataGridViewTextBoxColumn";
            this.isinDataGridViewTextBoxColumn.ReadOnly = true;
            this.isinDataGridViewTextBoxColumn.Width = 48;
            // 
            // tickerDataGridViewTextBoxColumn
            // 
            this.tickerDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.tickerDataGridViewTextBoxColumn.DataPropertyName = "Ticker";
            this.tickerDataGridViewTextBoxColumn.HeaderText = "Ticker";
            this.tickerDataGridViewTextBoxColumn.Name = "tickerDataGridViewTextBoxColumn";
            this.tickerDataGridViewTextBoxColumn.ReadOnly = true;
            this.tickerDataGridViewTextBoxColumn.Width = 62;
            // 
            // nameDataGridViewTextBoxColumn
            // 
            this.nameDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.nameDataGridViewTextBoxColumn.DataPropertyName = "Name";
            this.nameDataGridViewTextBoxColumn.HeaderText = "Name";
            this.nameDataGridViewTextBoxColumn.Name = "nameDataGridViewTextBoxColumn";
            this.nameDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // valueDataGridViewTextBoxColumn
            // 
            this.valueDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.valueDataGridViewTextBoxColumn.DataPropertyName = "Value";
            this.valueDataGridViewTextBoxColumn.HeaderText = "Value";
            this.valueDataGridViewTextBoxColumn.Name = "valueDataGridViewTextBoxColumn";
            this.valueDataGridViewTextBoxColumn.ReadOnly = true;
            this.valueDataGridViewTextBoxColumn.Width = 59;
            // 
            // pctDataGridViewTextBoxColumn
            // 
            this.pctDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.pctDataGridViewTextBoxColumn.DataPropertyName = "Pct";
            this.pctDataGridViewTextBoxColumn.HeaderText = "Pct";
            this.pctDataGridViewTextBoxColumn.Name = "pctDataGridViewTextBoxColumn";
            this.pctDataGridViewTextBoxColumn.ReadOnly = true;
            this.pctDataGridViewTextBoxColumn.Width = 48;
            // 
            // stockBindingSource
            // 
            this.stockBindingSource.DataSource = typeof(PortfolioManagement.Stock);
            // 
            // dataSet
            // 
            this.dataSet.DataSetName = "NewDataSet";
            this.dataSet.Tables.AddRange(new System.Data.DataTable[] {
            this.stocksNameTable,
            this.StocksInNotifTable});
            // 
            // stocksNameTable
            // 
            this.stocksNameTable.Columns.AddRange(new System.Data.DataColumn[] {
            this.columnId,
            this.columnName,
            this.HighlightColumn});
            this.stocksNameTable.Constraints.AddRange(new System.Data.Constraint[] {
            new System.Data.UniqueConstraint("Constraint1", new string[] {
                        "Id"}, true)});
            this.stocksNameTable.PrimaryKey = new System.Data.DataColumn[] {
        this.columnId};
            this.stocksNameTable.TableName = "StocksNameTable";
            // 
            // columnId
            // 
            this.columnId.AllowDBNull = false;
            this.columnId.Caption = "Id";
            this.columnId.ColumnName = "Id";
            // 
            // columnName
            // 
            this.columnName.Caption = "Name";
            this.columnName.ColumnName = "Name";
            // 
            // HighlightColumn
            // 
            this.HighlightColumn.Caption = "Highlight";
            this.HighlightColumn.ColumnName = "Highlight";
            this.HighlightColumn.DataType = typeof(bool);
            // 
            // StocksInNotifTable
            // 
            this.StocksInNotifTable.Columns.AddRange(new System.Data.DataColumn[] {
            this.IdColumn});
            this.StocksInNotifTable.TableName = "StocksInNotifTable";
            // 
            // IdColumn
            // 
            this.IdColumn.Caption = "Id";
            this.IdColumn.ColumnName = "Id";
            // 
            // myNotifyIcon
            // 
            this.myNotifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("myNotifyIcon.Icon")));
            this.myNotifyIcon.Text = "InfoBourse";
            this.myNotifyIcon.Visible = true;
            this.myNotifyIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.MyNotifyIconMouseDoubleClick);
            this.myNotifyIcon.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MyNotifyIconMouseDown);
            // 
            // companyNb
            // 
            this.companyNb.Location = new System.Drawing.Point(374, 43);
            this.companyNb.Margin = new System.Windows.Forms.Padding(0);
            this.companyNb.Name = "companyNb";
            this.companyNb.Size = new System.Drawing.Size(81, 18);
            this.companyNb.TabIndex = 5;
            this.companyNb.Text = "0 company";
            this.companyNb.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(350, 186);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(214, 133);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 6;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.PictureBox1Click);
            // 
            // dataGridContextMenu
            // 
            this.dataGridContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemAddToNotif,
            this.toolStripMenuItemRemoveFromNotif,
            this.buyToolStripMenuItem,
            this.sellToolStripMenuItem});
            this.dataGridContextMenu.Name = "dataGridContextMenu";
            this.dataGridContextMenu.ShowImageMargin = false;
            this.dataGridContextMenu.Size = new System.Drawing.Size(190, 92);
            // 
            // toolStripMenuItemAddToNotif
            // 
            this.toolStripMenuItemAddToNotif.Name = "toolStripMenuItemAddToNotif";
            this.toolStripMenuItemAddToNotif.Size = new System.Drawing.Size(189, 22);
            this.toolStripMenuItemAddToNotif.Text = "Add To Notification";
            this.toolStripMenuItemAddToNotif.Click += new System.EventHandler(this.ToolStripMenuItemAddToNotifClick);
            // 
            // toolStripMenuItemRemoveFromNotif
            // 
            this.toolStripMenuItemRemoveFromNotif.Name = "toolStripMenuItemRemoveFromNotif";
            this.toolStripMenuItemRemoveFromNotif.Size = new System.Drawing.Size(189, 22);
            this.toolStripMenuItemRemoveFromNotif.Text = "Remove From Notification";
            this.toolStripMenuItemRemoveFromNotif.Click += new System.EventHandler(this.ToolStripMenuItemRemoveFromNotifClick);
            // 
            // buyToolStripMenuItem
            // 
            this.buyToolStripMenuItem.Name = "buyToolStripMenuItem";
            this.buyToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
            this.buyToolStripMenuItem.Text = "Buy";
            this.buyToolStripMenuItem.Click += new System.EventHandler(this.buyToolStripMenuItem_Click);
            // 
            // sellToolStripMenuItem
            // 
            this.sellToolStripMenuItem.Name = "sellToolStripMenuItem";
            this.sellToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
            this.sellToolStripMenuItem.Text = "Sell";
            this.sellToolStripMenuItem.Click += new System.EventHandler(this.sellToolStripMenuItem_Click);
            // 
            // mainTimer
            // 
            this.mainTimer.Tick += new System.EventHandler(this.RefreshSelectedStockList);
            // 
            // comboBoxTime
            // 
            this.comboBoxTime.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxTime.FormattingEnabled = true;
            this.comboBoxTime.Location = new System.Drawing.Point(165, 11);
            this.comboBoxTime.Name = "comboBoxTime";
            this.comboBoxTime.Size = new System.Drawing.Size(87, 21);
            this.comboBoxTime.TabIndex = 7;
            this.comboBoxTime.SelectedIndexChanged += new System.EventHandler(this.ComboBoxTimeSelectedIndexChanged);
            // 
            // buttonUpdate
            // 
            this.buttonUpdate.Location = new System.Drawing.Point(83, 11);
            this.buttonUpdate.Name = "buttonUpdate";
            this.buttonUpdate.Size = new System.Drawing.Size(76, 23);
            this.buttonUpdate.TabIndex = 8;
            this.buttonUpdate.Text = "Update Data";
            this.buttonUpdate.UseVisualStyleBackColor = true;
            this.buttonUpdate.Click += new System.EventHandler(this.RefreshSelectedStockList);
            // 
            // label_valo
            // 
            this.label_valo.Location = new System.Drawing.Point(162, 43);
            this.label_valo.Margin = new System.Windows.Forms.Padding(0);
            this.label_valo.Name = "label_valo";
            this.label_valo.Size = new System.Drawing.Size(90, 18);
            this.label_valo.TabIndex = 9;
            this.label_valo.Text = "0.00 €";
            this.label_valo.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // textBoxSearch
            // 
            this.textBoxSearch.ForeColor = System.Drawing.Color.Gray;
            this.textBoxSearch.Location = new System.Drawing.Point(13, 40);
            this.textBoxSearch.Name = "textBoxSearch";
            this.textBoxSearch.Size = new System.Drawing.Size(146, 20);
            this.textBoxSearch.TabIndex = 10;
            this.textBoxSearch.Text = "Type to search...";
            this.textBoxSearch.TextChanged += new System.EventHandler(this.TextBoxSearchTextChanged);
            this.textBoxSearch.Enter += new System.EventHandler(this.TextBoxSearchEnter);
            this.textBoxSearch.Leave += new System.EventHandler(this.TextBoxSearchLeave);
            // 
            // label_gain
            // 
            this.label_gain.Location = new System.Drawing.Point(258, 43);
            this.label_gain.Margin = new System.Windows.Forms.Padding(0);
            this.label_gain.Name = "label_gain";
            this.label_gain.Size = new System.Drawing.Size(116, 18);
            this.label_gain.TabIndex = 11;
            this.label_gain.Text = "0.00 €";
            // 
            // comboBoxTheme
            // 
            this.comboBoxTheme.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxTheme.FormattingEnabled = true;
            this.comboBoxTheme.Location = new System.Drawing.Point(377, 11);
            this.comboBoxTheme.Name = "comboBoxTheme";
            this.comboBoxTheme.Size = new System.Drawing.Size(78, 21);
            this.comboBoxTheme.TabIndex = 12;
            this.comboBoxTheme.SelectionChangeCommitted += new System.EventHandler(this.comboBoxThemeSelectedIndexChanged);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(466, 438);
            this.Controls.Add(this.comboBoxTheme);
            this.Controls.Add(this.label_gain);
            this.Controls.Add(this.textBoxSearch);
            this.Controls.Add(this.label_valo);
            this.Controls.Add(this.buttonUpdate);
            this.Controls.Add(this.comboBoxTime);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.companyNb);
            this.Controls.Add(this.dataGridView);
            this.Controls.Add(this.buttonLoad);
            this.Controls.Add(this.textBoxLastUpdate);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Text = "InfoBourse - By Jean";
            this.Resize += new System.EventHandler(this.MainFormResize);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.stockBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.stocksNameTable)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.StocksInNotifTable)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.dataGridContextMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxLastUpdate;
        private System.Windows.Forms.Button buttonLoad;
        private System.Windows.Forms.DataGridView dataGridView;
        private System.Data.DataSet dataSet;
        private System.Data.DataTable stocksNameTable;
        private System.Data.DataColumn columnId;
        private System.Data.DataColumn columnName;
        private System.Windows.Forms.NotifyIcon myNotifyIcon;
        private System.Windows.Forms.BindingSource stockBindingSource;
        private System.Windows.Forms.Label companyNb;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.ContextMenuStrip dataGridContextMenu;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemAddToNotif;
        private System.Windows.Forms.Timer mainTimer;
        private System.Windows.Forms.ComboBox comboBoxTime;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemRemoveFromNotif;
        private System.Data.DataTable StocksInNotifTable;
        private System.Data.DataColumn IdColumn;
        private System.Windows.Forms.Button buttonUpdate;
        private System.Windows.Forms.Label label_valo;
        private System.Windows.Forms.DataGridViewTextBoxColumn idDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn isinDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn tickerDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn valueDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn pctDataGridViewTextBoxColumn;
        private System.Windows.Forms.TextBox textBoxSearch;
        private System.Windows.Forms.ToolStripMenuItem buyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sellToolStripMenuItem;
        private System.Windows.Forms.Label label_gain;
        private System.Data.DataColumn HighlightColumn;
        private System.Windows.Forms.ComboBox comboBoxTheme;
    }
}

