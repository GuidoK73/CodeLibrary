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

        protected override void PasteAdvancedText()
        {
            bool b = Do();
            if (b)
                return;

            base.PasteAdvancedText();
        }

        private bool Do()
        {
            string _text = Clipboard.GetText();
            _text = Core.Utils.TrimText(_text, "\r\n");

            string _data = string.Empty;
            bool _first = true;
            string[] _header = new string[0];

            StringBuilder _sb = new StringBuilder();

            bool _isCsv = Core.Utils.GetCsvSeparator(_text, out char _separator);
            if (!_isCsv)
                return false;

            byte[] byteArray = Encoding.Default.GetBytes(_text);
            using (MemoryStream _stream = new MemoryStream(byteArray))
            {
                using (CsvStreamReader _reader = new CsvStreamReader(_stream))
                {
                    _reader.Separator = _separator;
                    while (!_reader.EndOfCsvStream)
                    {
                        if (_first)
                        {
                            _header = _reader.ReadCsvLine().ToArray();
                            _first = false;
                        }
                        else
                        {
                            _sb.Append("<item>\r\n");
                            string[] _items = _reader.ReadCsvLine().ToArray();
                            for (int ii = 0; ii < _header.Length; ii++)
                            {
                                _sb.Append($"\t<{_header[ii]}>{_items[ii]}</{_header[ii]}>\r\n");
                            }
                            _sb.Append("</item>\r\n");
                        }
                    }
                }
            }

            SelectedText = _sb.ToString();

            return true;
        }

        protected override void PasteAdvancedTextImage()
        {
            bool b = Do();
            if (b)
                return;

            base.PasteAdvancedTextImage();
        }
    }
}