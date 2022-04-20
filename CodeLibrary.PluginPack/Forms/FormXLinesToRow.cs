using System;
using System.Windows.Forms;

namespace CodeLibrary.PluginPack.Forms
{
    public partial class FormXLinesToRow : Form
    {
        public FormXLinesToRow()
        {
            InitializeComponent();
            CancelButton = buttonCancel;
        }

        public string Separator { get; set; }
        public int LineCount { get; set; }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            Separator = tbSeparator.Text;
            try
            {
                LineCount = System.Convert.ToInt32(tbLineCount.Text);
            }
            catch 
            {
                LineCount = 2;
            }
            DialogResult = DialogResult.OK;
            Close();
        }

        private void FormTrimLines_Load(object sender, EventArgs e)
        {
            tbLineCount.Text = LineCount.ToString();
            tbSeparator.Text = Separator;
        }
    }
}