using CodeLibrary.Helpers;
using System.Windows.Forms;

namespace CodeLibrary.Editor.EditorLanguageHelpers
{
    public class JSEditorClipboardHelper : EditorClipboardHelperBase
    {
        public JSEditorClipboardHelper(FormCodeLibrary mainform, TextBoxHelper textboxHelper, ThemeHelper themeHelper) : base(mainform, textboxHelper, themeHelper)
        {
        }

        protected override void Paste_CtrlShift_Text()
        {
            string _text = Clipboard.GetText();

            bool _isCsv = Core.Utils.GetCsvSeparator(_text, out char _separator);
            if (_isCsv)
            {
                if (Core.Utils.isReorderString(SelectedText))
                {
                    string _reorderString = SelectedText;
                    _text = Core.Utils.CsvChange(_text, _separator, _separator, _reorderString);
                }

                _text = Core.Utils.CsvToJSon(_text, _separator);
                SelectedText = _text;
                return;
            }
            base.Paste_Text();
        }

        protected override void Paste_CtrlAltShift_Text()
        {
            string _text = Clipboard.GetText();
            bool _isCsv = Core.Utils.GetCsvSeparator(_text, out char _separator);
            if (_isCsv)
            {
                if (Core.Utils.isReorderString(SelectedText))
                {
                    string _reorderString = SelectedText;
                    _text = Core.Utils.CsvChange(_text, _separator, _separator, _reorderString);
                }

                _text = Core.Utils.CsvToJSonNoHeader(_text, _separator);
                SelectedText = _text;
                return;                
            }
            base.Paste_Text();
        }
    }
}