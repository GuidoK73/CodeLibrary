using CodeLibrary.Core;
using CodeLibrary.Helpers;
using System.Windows.Forms;

namespace CodeLibrary.Editor.EditorLanguageHelpers
{
    public class JSEditorClipboardHelper : EditorClipboardHelperBase
    {
        public JSEditorClipboardHelper(FormCodeLibrary mainform, TextBoxHelper textboxHelper, ThemeHelper themeHelper) : base(mainform, textboxHelper, themeHelper)
        {
        }

        private bool Execute()
        {
            string _text = Clipboard.GetText();

            switch (GetClipboardTextType())
            {
                case TextType.Xml:
                    string _xml = Utils.FormatXml(_text, out bool _succes);
                    string _json = JsonUtils.ConvertXmlToJson(_xml);
                    SelectedText = JsonUtils.FormatJson(_json);
                    return true;

                case TextType.Json:
                    SelectedText = JsonUtils.FormatJson(_text);
                    return true;

                case TextType.Csv:
                    CsvUtils.GetCsvSeparator(_text, out char _separator);
                    if (Utils.isReorderString(SelectedText))
                    {
                        string _reorderString = SelectedText;
                        _text = CsvUtils.CsvChange(_text, _separator, _separator, _reorderString);
                    }
                    _text = CsvUtils.CsvToJSon(_text, _separator);
                    SelectedText = _text;
                    return true;

                case TextType.Text:
                    base.Paste_Text();
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

        protected override void Paste_CtrlAltShift_Text()
        {
            bool _succes = Execute();
            if (_succes)
                return;

            base.Paste_CtrlAltShift_Text();
        }
    }
}