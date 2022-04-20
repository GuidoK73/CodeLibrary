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
            bool b = Do(false);
            if (b)
                return;

            base.Paste_CtrlShift_Text();
        }

        protected override void Paste_CtrlAltShift_Text()
        {
            bool b = Do(true);
            if (b)
                return;

            base.Paste_CtrlShift_Text();
        }

        private bool Do(bool noHeader)
        {
            string _text = Clipboard.GetText();
            _text = Core.Utils.TrimText(_text, "\r\n");

            string _data = string.Empty;
            bool _first = true;
            

            StringBuilder _sb = new StringBuilder();

            bool _isCsv = Core.Utils.GetCsvSeparator(_text, out char _separator);
            if (!_isCsv)
                return false;


            if (Core.Utils.isReorderString(SelectedText))
            {
                string _reorderString = SelectedText;
                _text = Core.Utils.CsvChange(_text, _separator, _separator, _reorderString);
            }

            string[] _header = Core.Utils.CsvHeader(_text, _separator);
            if (noHeader)
            {
                for (int ii = 0; ii < _header.Length; ii++)
                {
                    _header[ii] = $"Field_{ii}";
                }
            }

            byte[] byteArray = Encoding.Default.GetBytes(_text);
            using (MemoryStream _stream = new MemoryStream(byteArray))
            {
                using (CsvStreamReader _reader = new CsvStreamReader(_stream))
                {
                    _reader.Separator = _separator;
                    while (!_reader.EndOfCsvStream)
                    {
                        if (_first == false || noHeader)
                        {
                            _sb.Append("<item>\r\n");
                            string[] _items = _reader.ReadCsvLine().ToArray();
                            for (int ii = 0; ii < _header.Length; ii++)
                            {
                                _sb.Append($"\t<{_header[ii]}>{_items[ii]}</{_header[ii]}>\r\n");
                            }
                            _sb.Append("</item>\r\n");
                        }
                        if (_first == true)
                        {
                            _first = false;
                        }
                    }
                }
            }

            SelectedText = _sb.ToString();

            return true;
        }

        protected override void Paste_CtrlShift_TextImage()
        {
            bool b = Do(false);
            if (b)
                return;

            base.Paste_CtrlShift_TextImage();
        }

        protected override void Paste_CtrlAltShift_TextImage()
        {
            bool b = Do(true);
            if (b)
                return;

            base.Paste_CtrlShift_TextImage();
        }
    }
}