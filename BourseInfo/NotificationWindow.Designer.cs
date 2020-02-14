using System;
using System.Windows.Forms;

namespace BourseInfo
{
    using System.ComponentModel;

    partial class NotificationWindow
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.contextMenuStripNotif = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.removeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openBrowserToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.highlightToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStripNotif.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.AutoSize = true;
            this.tableLayoutPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel.ColumnCount = 1;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 1;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(250, 20);
            this.tableLayoutPanel.TabIndex = 2;
            // 
            // contextMenuStripNotif
            // 
            this.contextMenuStripNotif.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.removeToolStripMenuItem,
            this.openBrowserToolStripMenuItem,
            this.toolStripSeparator1,
            this.highlightToolStripMenuItem});
            this.contextMenuStripNotif.Name = "contextMenuStripNotif";
            this.contextMenuStripNotif.Size = new System.Drawing.Size(237, 76);
            // 
            // removeToolStripMenuItem
            // 
            this.removeToolStripMenuItem.Name = "removeToolStripMenuItem";
            this.removeToolStripMenuItem.Size = new System.Drawing.Size(236, 22);
            this.removeToolStripMenuItem.Text = "Remove";
            this.removeToolStripMenuItem.Click += new System.EventHandler(this.RemoveToolStripMenuItemClick);
            // 
            // openBrowserToolStripMenuItem
            // 
            this.openBrowserToolStripMenuItem.Name = "openBrowserToolStripMenuItem";
            this.openBrowserToolStripMenuItem.Size = new System.Drawing.Size(236, 22);
            this.openBrowserToolStripMenuItem.Text = "Open in browser (Boursorama)";
            this.openBrowserToolStripMenuItem.Click += new System.EventHandler(this.OpenBrowserToolStripMenuItemClick);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(233, 6);
            // 
            // highlightToolStripMenuItem
            // 
            this.highlightToolStripMenuItem.AccessibleDescription = "";
            this.highlightToolStripMenuItem.Name = "highlightToolStripMenuItem";
            this.highlightToolStripMenuItem.Size = new System.Drawing.Size(236, 22);
            this.highlightToolStripMenuItem.Text = "Highlight item";
            this.highlightToolStripMenuItem.Click += new System.EventHandler(this.highlightToolStripMenuItemClick);
            // 
            // NotificationWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(250, 20);
            this.Controls.Add(this.tableLayoutPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximumSize = new System.Drawing.Size(250, 1080);
            this.MinimumSize = new System.Drawing.Size(250, 0);
            this.Name = "NotificationWindow";
            this.ShowInTaskbar = false;
            this.Text = "NotificationWindow";
            this.VisibleChanged += new System.EventHandler(this.NotificationWindowVisibleChanged);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.NotificationWindowPaint);
            this.contextMenuStripNotif.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TableLayoutPanel tableLayoutPanel;
        private ContextMenuStrip contextMenuStripNotif;
        private ToolStripMenuItem removeToolStripMenuItem;
        private ToolStripMenuItem openBrowserToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem highlightToolStripMenuItem;
    }
}