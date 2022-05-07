using CodeLibrary.Core;
using System;
using System.Windows.Forms;

namespace CodeLibrary
{
    public partial class FormSettings : Form
    {
        private EnumComboBoxModeHelper<ETheme> _ComboBoxHelperTheme;
        private EnumComboBoxModeHelper<CssStyle> _ComboBoxHelperMDCss;
        private EnumComboBoxModeHelper<CssStyle> _ComboBoxHelperMDCssPreview;

        public FormSettings()
        {
            InitializeComponent();
            _ComboBoxHelperTheme = new EnumComboBoxModeHelper<ETheme>(comboBoxTheme, ETheme.Dark);
            _ComboBoxHelperTheme.Fill();

            _ComboBoxHelperMDCss = new EnumComboBoxModeHelper<CssStyle>(comboBoxMDCSS, CssStyle.Splendor);
            _ComboBoxHelperMDCss.Fill();


            _ComboBoxHelperMDCssPreview = new EnumComboBoxModeHelper<CssStyle>(comboBoxMDCSSPreview, CssStyle.Splendor);
            _ComboBoxHelperMDCssPreview.Fill();
        }

        private void FormSettings_Load(object sender, EventArgs e)
        {
            _ComboBoxHelperTheme.SetSelectedIndex(Config.Theme);
            _ComboBoxHelperMDCss.SetSelectedIndex(Config.MarkdownCssStyle);
            _ComboBoxHelperMDCssPreview.SetSelectedIndex(Config.MarkdownCssPreviewStyle);
            rbSortA.Checked = Config.SortMode == ESortMode.Alphabetic;
            rbSortB.Checked = Config.SortMode == ESortMode.AlphabeticGrouped;
        }

        private void dialogButton_DialogButtonClick(object sender, Controls.DialogButton.DialogButtonClickEventArgs e)
        {
            DialogResult = e.Result;
            if (e.Result == DialogResult.OK)
            {
                Config.Theme = (ETheme)_ComboBoxHelperTheme.GetValue();
                Config.MarkdownCssStyle = (CssStyle)_ComboBoxHelperMDCss.GetValue();
                Config.MarkdownCssPreviewStyle = (CssStyle)_ComboBoxHelperMDCssPreview.GetValue();
                if (rbSortA.Checked)
                {
                    Config.SortMode = ESortMode.Alphabetic;
                }
                if (rbSortB.Checked)
                {
                    Config.SortMode = ESortMode.AlphabeticGrouped;
                }
            }
            Close();
        }
    }
}
