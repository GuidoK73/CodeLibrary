using CodeLibrary.Core;
using CodeLibrary.Helpers;
using System.Windows.Forms;

namespace CodeLibrary.Editor.EditorLanguageHelpers
{
    public class TextEditorClipboardHelper : EditorClipboardHelperBase
    {
        public TextEditorClipboardHelper(FormCodeLibrary mainform, TextBoxHelper textboxHelper, ThemeHelper themeHelper) : base(mainform, textboxHelper, themeHelper)
        {
        }

        private bool Execute()
        {
            string _text = Clipboard.GetText();
            _text = Core.Utils.TrimText(_text, "\r\n");

            char _separator = ' ';
            string _data = string.Empty;

            bool _isCsv = CsvUtils.GetCsvSeparator(_text, out _separator);
            if (_isCsv)
            {
                string _reorderString = null;
                if (Core.Utils.isReorderString(SelectedText))
                {
                    _reorderString = SelectedText;
                }

                _data = CsvUtils.CsvChange(_text, _separator, ',', _reorderString);
                this.SelectedText = _data;
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