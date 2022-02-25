using CodeLibrary.Helpers;
using System.Windows.Forms;

namespace CodeLibrary.Editor.EditorLanguageHelpers
{
    public class TemplateEditorClipboardHelper : EditorClipboardHelperBase
    {
        public TemplateEditorClipboardHelper(FormCodeLibrary mainform, TextBoxHelper textboxHelper, ThemeHelper themeHelper) : base(mainform, textboxHelper, themeHelper)
        {
        }

        protected override void PasteAdvancedText()
        {
            string _text = Clipboard.GetText();
            _text = Core.Utils.TrimText(_text, "\r\n");

            bool _isCsv = Core.Utils.GetCsvSeparator(_text, out char _separator);
            if (_isCsv)
            {
                string _data = Core.Utils.CsvChange(_text, _separator, ';');
                SelectedText = _data;
                return;
            }

            base.PasteText();
        }

        protected override void PasteAdvancedTextImage()
        {
            string _text = Clipboard.GetText();
            _text = Core.Utils.TrimText(_text, "\r\n");


            bool _isCsv = Core.Utils.GetCsvSeparator(_text, out char _separator);
            if (_isCsv)
            {
                string _data = Core.Utils.CsvChange(_text, _separator, ';');
                SelectedText = _data;
                return;
            }

            base.PasteAdvancedTextImage();
        }
    }
}