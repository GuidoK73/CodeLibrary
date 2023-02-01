namespace CodeLibrary.PluginPack.Forms
{
    partial class FormRegexReplace
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
            this.lstRegexItems = new CodeLibrary.Controls.CollectionListBox();
            this.lblName = new System.Windows.Forms.Label();
            this.tbName = new System.Windows.Forms.TextBox();
            this.tbRegex = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tbReplace = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.buttonReplace = new System.Windows.Forms.Button();
            this.buttonClose = new System.Windows.Forms.Button();
            this.tbTestResult = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tbTest = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.buttonTest = new System.Windows.Forms.Button();
            this.tbCategory = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lstRegexItems
            // 
            this.lstRegexItems.AskBeforeDelete = false;
            this.lstRegexItems.CategoryProperty = "Category";
            this.lstRegexItems.ColorProperty = null;
            this.lstRegexItems.ColumnWidth = 300;
            this.lstRegexItems.DefaultCategoryName = "No Category";
            this.lstRegexItems.ImageProperty = null;
            this.lstRegexItems.Location = new System.Drawing.Point(12, 13);
            this.lstRegexItems.MultiSelect = false;
            this.lstRegexItems.Name = "lstRegexItems";
            this.lstRegexItems.NameProperty = "Name";
            this.lstRegexItems.ShowAdd = true;
            this.lstRegexItems.ShowCopy = false;
            this.lstRegexItems.ShowDelete = true;
            this.lstRegexItems.ShowRefresh = false;
            this.lstRegexItems.ShowSearch = false;
            this.lstRegexItems.ShowToolstrip = true;
            this.lstRegexItems.Size = new System.Drawing.Size(273, 373);
            this.lstRegexItems.Sorting = System.Windows.Forms.SortOrder.Descending;
            this.lstRegexItems.TabIndex = 0;
            this.lstRegexItems.BeforeItemSelected += new System.EventHandler<CodeLibrary.Controls.CollectionListBox.CollectionListBoxEventArgs>(this.lstRegexItems_BeforeItemSelected);
            this.lstRegexItems.ItemSelected += new System.EventHandler<CodeLibrary.Controls.CollectionListBox.CollectionListBoxEventArgs>(this.lstRegexItems_ItemSelected);
            this.lstRegexItems.OnAdd += new System.EventHandler<CodeLibrary.Controls.CollectionListBox.CollectionListBoxEventArgs>(this.lstRegexItems_OnAdd);
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(291, 12);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(38, 13);
            this.lblName.TabIndex = 1;
            this.lblName.Text = "Name:";
            // 
            // tbName
            // 
            this.tbName.Location = new System.Drawing.Point(294, 28);
            this.tbName.Name = "tbName";
            this.tbName.Size = new System.Drawing.Size(201, 20);
            this.tbName.TabIndex = 2;
            this.tbName.KeyUp += new System.Windows.Forms.KeyEventHandler(this.tbName_KeyUp);
            this.tbName.Leave += new System.EventHandler(this.tbName_Leave);
            // 
            // tbRegex
            // 
            this.tbRegex.Location = new System.Drawing.Point(294, 117);
            this.tbRegex.Multiline = true;
            this.tbRegex.Name = "tbRegex";
            this.tbRegex.Size = new System.Drawing.Size(311, 93);
            this.tbRegex.TabIndex = 4;
            this.tbRegex.KeyUp += new System.Windows.Forms.KeyEventHandler(this.tbRegex_KeyUp);
            this.tbRegex.Leave += new System.EventHandler(this.tbRegex_Leave);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(291, 101);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Regex:";
            // 
            // tbReplace
            // 
            this.tbReplace.Location = new System.Drawing.Point(294, 233);
            this.tbReplace.Multiline = true;
            this.tbReplace.Name = "tbReplace";
            this.tbReplace.Size = new System.Drawing.Size(311, 93);
            this.tbReplace.TabIndex = 6;
            this.tbReplace.KeyUp += new System.Windows.Forms.KeyEventHandler(this.tbReplace_KeyUp);
            this.tbReplace.Leave += new System.EventHandler(this.tbReplace_Leave);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(291, 217);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(84, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Result Template";
            // 
            // buttonReplace
            // 
            this.buttonReplace.Location = new System.Drawing.Point(835, 370);
            this.buttonReplace.Name = "buttonReplace";
            this.buttonReplace.Size = new System.Drawing.Size(75, 23);
            this.buttonReplace.TabIndex = 7;
            this.buttonReplace.Text = "Replace";
            this.buttonReplace.UseVisualStyleBackColor = true;
            this.buttonReplace.Click += new System.EventHandler(this.buttonReplace_Click);
            // 
            // buttonClose
            // 
            this.buttonClose.Location = new System.Drawing.Point(754, 370);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(75, 23);
            this.buttonClose.TabIndex = 8;
            this.buttonClose.Text = "Close";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // tbTestResult
            // 
            this.tbTestResult.Location = new System.Drawing.Point(620, 144);
            this.tbTestResult.Multiline = true;
            this.tbTestResult.Name = "tbTestResult";
            this.tbTestResult.Size = new System.Drawing.Size(290, 93);
            this.tbTestResult.TabIndex = 12;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(617, 128);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(61, 13);
            this.label3.TabIndex = 11;
            this.label3.Text = "Test Result";
            // 
            // tbTest
            // 
            this.tbTest.Location = new System.Drawing.Point(620, 28);
            this.tbTest.Multiline = true;
            this.tbTest.Name = "tbTest";
            this.tbTest.Size = new System.Drawing.Size(290, 93);
            this.tbTest.TabIndex = 10;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(617, 12);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(51, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "TestData";
            // 
            // buttonTest
            // 
            this.buttonTest.Location = new System.Drawing.Point(835, 243);
            this.buttonTest.Name = "buttonTest";
            this.buttonTest.Size = new System.Drawing.Size(75, 23);
            this.buttonTest.TabIndex = 13;
            this.buttonTest.Text = "Test";
            this.buttonTest.UseVisualStyleBackColor = true;
            this.buttonTest.Click += new System.EventHandler(this.buttonTest_Click);
            // 
            // tbCategory
            // 
            this.tbCategory.Location = new System.Drawing.Point(294, 71);
            this.tbCategory.Name = "tbCategory";
            this.tbCategory.Size = new System.Drawing.Size(201, 20);
            this.tbCategory.TabIndex = 15;
            this.tbCategory.KeyUp += new System.Windows.Forms.KeyEventHandler(this.tbCategory_KeyUp);
            this.tbCategory.Leave += new System.EventHandler(this.tbCategory_Leave);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(291, 55);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(52, 13);
            this.label5.TabIndex = 14;
            this.label5.Text = "Category:";
            // 
            // FormRegexReplace
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(922, 405);
            this.Controls.Add(this.tbCategory);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.buttonTest);
            this.Controls.Add(this.tbTestResult);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tbTest);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.buttonReplace);
            this.Controls.Add(this.tbReplace);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tbRegex);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbName);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.lstRegexItems);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FormRegexReplace";
            this.Text = "Regex Extract";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormRegexReplace_FormClosing);
            this.Load += new System.EventHandler(this.FormRegexReplace_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Controls.CollectionListBox lstRegexItems;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.TextBox tbName;
        private System.Windows.Forms.TextBox tbRegex;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbReplace;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button buttonReplace;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.TextBox tbTestResult;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbTest;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button buttonTest;
        private System.Windows.Forms.TextBox tbCategory;
        private System.Windows.Forms.Label label5;
    }
}