using CodeLibrary.Core;
using CodeLibrary.Helpers;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace CodeLibrary.Editor.EditorLanguageHelpers
{
    public class TemplateEditorClipboardHelper : EditorClipboardHelperBase
    {
        private CSharpUtils _CSharpUtils = new CSharpUtils();

        public TemplateEditorClipboardHelper(FormCodeLibrary mainform, TextBoxHelper textboxHelper, ThemeHelper themeHelper) : base(mainform, textboxHelper, themeHelper)
        {
        }

        private bool Execute()
        {
            string _text = Clipboard.GetText();
            _text = Utils.TrimText(_text, "\r\n");

            char _separator = ' ';
            string _data = string.Empty;

            char[] _chars = _text.ToCharArray();

            bool _isCsv = CsvUtils.GetCsvSeparator(_text, out _separator);
            if (_isCsv)
            {
                string _reorderString = null;
                if (Core.Utils.isReorderString(SelectedText))
                {
                    _reorderString = SelectedText;
                }

                _data = CsvUtils.CsvChange(_text, _separator, ';', _reorderString);
                this.SelectedText = _data;
                return true;
            }
            List<string[]> _classProps = _CSharpUtils.GetProperties(_text, out bool isClass);
            if (isClass)
            {
                StringBuilder _sb = new StringBuilder();
                foreach (string[] item in _classProps)
                {
                    _sb.AppendLine($"{item[0]};{item[1]}");
                }
                this.SelectedText = _sb.ToString();
                return true;
            }
            return false;
        }

        protected override void Paste_CtrlShift_Text()
        {
            bool _succes = Execute();
            if (_succes)
                return;

            base.Paste_CtrlShift_Text();
        }

        protected override void Paste_CtrlShift_TextImage()
        {
            bool _succes = Execute();
            if (_succes)
                return;

            base.Paste_CtrlShift_TextImage();
        }
    }
}