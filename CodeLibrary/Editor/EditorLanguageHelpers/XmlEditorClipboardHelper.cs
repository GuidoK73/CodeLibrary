using CodeLibrary.Core;
using CodeLibrary.Core.DevToys;
using CodeLibrary.Helpers;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CodeLibrary.Editor.EditorLanguageHelpers
{
    public class XmlEditorClipboardHelper : EditorClipboardHelperBase
    {
        public XmlEditorClipboardHelper(FormCodeLibrary mainform, TextBoxHelper textboxHelper, ThemeHelper themeHelper) : base(mainform, textboxHelper, themeHelper)
        {
        }

        protected override void Paste_CtrlShift_Text()
        {
            bool _succes = Execute();
            if (_succes)
                return;

            base.Paste_CtrlShift_Text();
        }

        private bool Execute()
        {
            string _text = Clipboard.GetText();

            switch (GetClipboardTextType())
            {
                case TextType.Xml:
                    string _xml = Utils.FormatXml(_text, out bool _succes);
                    if (_succes)
                    {
                        SelectedText = _xml;
                        return true;
                    }
                    return false; ;

                case TextType.Json:
                    SelectedText = JsonUtils.ConvertJsonToXml(_text);
                    return true;

                case TextType.Csv:
                    _text = Utils.TrimText(_text, "\r\n");
                    string _data = string.Empty;
                    bool _first = true;
                    StringBuilder _sb = new StringBuilder();

                    bool _isCsv = CsvUtils.GetCsvSeparator(_text, out char _separator);
                    if (!_isCsv)
                        return false;

                    if (Utils.isReorderString(SelectedText))
                    {
                        string _reorderString = SelectedText;
                        _text = CsvUtils.CsvChange(_text, _separator, _separator, _reorderString);
                    }

                    string[] _header = CsvUtils.CsvHeader(_text, _separator);

                    byte[] byteArray = Encoding.Default.GetBytes(_text);
                    using (MemoryStream _stream = new MemoryStream(byteArray))
                    {
                        using (CsvStreamReader _reader = new CsvStreamReader(_stream))
                        {
                            _reader.Separator = _separator;
                            _sb.Append("<Root>\r\n");
                            while (!_reader.EndOfStream)
                            {
                                if (_first == false)
                                {
                                    _sb.Append("\t<Item>\r\n");
                                    string[] _items = _reader.ReadCsvLine().ToArray();
                                    for (int ii = 0; ii < _header.Length; ii++)
                                    {
                                        _sb.Append($"\t\t<{_header[ii]}>{_items[ii]}</{_header[ii]}>\r\n");
                                    }
                                    _sb.Append("\t</Item>\r\n");
                                }
                                if (_first == true)
                                {
                                    _first = false;
                                }
                            }
                            _sb.Append("</Root>\r\n");

                        }
                    }

                    SelectedText = _sb.ToString();
                    return true;
            }

            return false;
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