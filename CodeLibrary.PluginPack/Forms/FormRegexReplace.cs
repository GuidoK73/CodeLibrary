using CodeLibrary.Core;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace CodeLibrary.PluginPack.Forms
{
    public partial class FormRegexReplace : Form
    {
        private List<RegexItem> Items = new List<RegexItem>();

        private RegexItem _CurrentItem = null;

        public FormRegexReplace()
        {
            InitializeComponent();
            lstRegexItems.ShowRefresh = false;
        }

        public string Library { get; set; }

        public string Find { get; set; }

        public string Replace { get; set; }

        private void LoadCollection()
        {
            if (!string.IsNullOrEmpty(Library))
            {
                string _data = Utils.FromBase64(Library);
                Items = Utils.FromJsonToList<RegexItem>(_data);
            }
            lstRegexItems.SetCollection<RegexItem>(Items);
        }

        private void SaveCollection()
        {
            Items = lstRegexItems.GetCollection<RegexItem>();
            string _data = Utils.ToJson<RegexItem>(Items);
            Library = Utils.ToBase64(_data);
        }

        private void FormRegexReplace_Load(object sender, EventArgs e)
        {
            LoadCollection();
            lstRegexItems.Refresh();
            DataToScreen();
        }

        private void FormRegexReplace_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveCollection();
        }

        private void buttonReplace_Click(object sender, EventArgs e)
        {
            ScreenToData();
            this.DialogResult = DialogResult.OK;
            Close();
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            ScreenToData();
            this.DialogResult = DialogResult.Cancel;
            Close();
        }

        private void buttonTest_Click(object sender, EventArgs e)
        {
            try
            {
                Regex _regex = new Regex(tbRegex.Text);
                tbTestResult.Text = _regex.Replace(tbTest.Text, tbReplace.Text);
            }
            catch
            {
            }
        }

        private void lstRegexItems_ItemSelected(object sender, Controls.CollectionListBox.CollectionListBoxEventArgs e)
        {
            _CurrentItem = e.Item as RegexItem;
            DataToScreen();
        }

        private void DataToScreen()
        {
            if (_CurrentItem == null)
            {
                return;
            }
            tbName.Text = _CurrentItem.Name;
            tbCategory.Text = _CurrentItem.Category;
            tbRegex.Text = _CurrentItem.Find;
            tbReplace.Text = _CurrentItem.Replace;
        }

        private void ScreenToData()
        {
            if (_CurrentItem == null)
            {
                return;
            }
            _CurrentItem.Name = tbName.Text;
            _CurrentItem.Category = tbCategory.Text;
            _CurrentItem.Find = tbRegex.Text;
            _CurrentItem.Replace = tbReplace.Text;
        }

        private void ClearScreen()
        {
            tbName.Text = "";
            tbCategory.Text = "";
            tbRegex.Text = "";
            tbReplace.Text = "";
        }

        private void lstRegexItems_OnAdd(object sender, Controls.CollectionListBox.CollectionListBoxEventArgs e)
        {
            ScreenToData();
            ClearScreen();
        }

        private void lstRegexItems_BeforeItemSelected(object sender, Controls.CollectionListBox.CollectionListBoxEventArgs e)
        {
      
        }

        private void tbName_KeyUp(object sender, KeyEventArgs e) => ScreenToData();

        private void tbCategory_KeyUp(object sender, KeyEventArgs e) => ScreenToData();

        private void tbRegex_KeyUp(object sender, KeyEventArgs e) => ScreenToData();

        private void tbReplace_KeyUp(object sender, KeyEventArgs e) => ScreenToData();

        private void tbName_Leave(object sender, EventArgs e) => ScreenToData();

        private void tbCategory_Leave(object sender, EventArgs e) => ScreenToData();

        private void tbRegex_Leave(object sender, EventArgs e) => ScreenToData();

        private void tbReplace_Leave(object sender, EventArgs e) => ScreenToData();
    }
}