namespace CodeLibrary.Controls.DirectoryBrowser
{
    partial class DirectoryBrowser
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
            this.components = new System.ComponentModel.Container();
            this.Browser = new System.Windows.Forms.TreeView();
            this.MenuFav = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.newFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuFav.SuspendLayout();
            this.SuspendLayout();
            // 
            // Browser
            // 
            this.Browser.AllowDrop = true;
            this.Browser.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(44)))), ((int)(((byte)(44)))));
            this.Browser.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.Browser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Browser.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Browser.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.Browser.Indent = 24;
            this.Browser.ItemHeight = 24;
            this.Browser.LineColor = System.Drawing.Color.White;
            this.Browser.Location = new System.Drawing.Point(0, 0);
            this.Browser.Margin = new System.Windows.Forms.Padding(4);
            this.Browser.Name = "Browser";
            this.Browser.Size = new System.Drawing.Size(319, 326);
            this.Browser.TabIndex = 1;
            this.Browser.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.Browser_BeforeExpand);
            this.Browser.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.Browser_AfterSelect);
            this.Browser.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.Browser_NodeMouseClick);
            this.Browser.DragDrop += new System.Windows.Forms.DragEventHandler(this.Browser_DragDrop);
            this.Browser.HideSelection = false;
            // 
            // MenuFav
            // 
            this.MenuFav.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.MenuFav.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newFolderToolStripMenuItem});
            this.MenuFav.Name = "MenuFav";
            this.MenuFav.Size = new System.Drawing.Size(155, 28);
            // 
            // newFolderToolStripMenuItem
            // 
            this.newFolderToolStripMenuItem.Name = "newFolderToolStripMenuItem";
            this.newFolderToolStripMenuItem.Size = new System.Drawing.Size(154, 24);
            this.newFolderToolStripMenuItem.Text = "New Folder";
            // 
            // DirectoryBrowser
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.Browser);
            this.DoubleBuffered = true;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "DirectoryBrowser";
            this.Size = new System.Drawing.Size(319, 326);
            this.Load += new System.EventHandler(this.DirectoryBrowser_Load);
            this.MenuFav.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView Browser;
        private System.Windows.Forms.ContextMenuStrip MenuFav;
        private System.Windows.Forms.ToolStripMenuItem newFolderToolStripMenuItem;
    }
}
