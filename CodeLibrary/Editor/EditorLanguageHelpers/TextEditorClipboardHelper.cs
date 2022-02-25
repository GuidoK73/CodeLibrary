using CodeLibrary.Helpers;
using System.Windows.Forms;

namespace CodeLibrary.Editor.EditorLanguageHelpers
{
    public class TextEditorClipboardHelper : EditorClipboardHelperBase
    {
        public TextEditorClipboardHelper(FormCodeLibrary mainform, TextBoxHelper textboxHelper, ThemeHelper themeHelper) : base(mainform, textboxHelper, themeHelper)
        {
        }

        protected override void PasteAdvancedText()
        {
            string _text = Clipboard.GetText();
            _text = Core.Utils.TrimText(_text, "\r\n");

            char _separator = ' ';
            string _data = string.Empty;

            bool _isCsv = Core.Utils.GetCsvSeparator(_text, out _separator);
            if (_isCsv)
            {
                _data = Core.Utils.CsvChange(_text, _separator, ',');
                this.SelectedText = _data;
                return;
            }

            base.PasteAdvancedText();
        }

        protected override void PasteAdvancedTextImage()
        {
            string _text = Clipboard.GetText();
            _text = Core.Utils.TrimText(_text, "\r\n");

            char _separator = ' ';
            string _data = string.Empty;

            bool _isCsv = Core.Utils.GetCsvSeparator(_text, out _separator);
            if (_isCsv)
            {
                _data = Core.Utils.CsvChange(_text, _separator, ',');
                this.SelectedText = _data;
                return;
            }

            base.PasteAdvancedTextImage();
        }
    }
}