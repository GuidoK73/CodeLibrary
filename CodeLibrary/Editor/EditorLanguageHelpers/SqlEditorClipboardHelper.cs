using CodeLibrary.Core.DevToys;
using CodeLibrary.Helpers;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CodeLibrary.Editor.EditorLanguageHelpers
{
    public class SqlEditorClipboardHelper : EditorClipboardHelperBase
    {

        public SqlEditorClipboardHelper(FormCodeLibrary mainform, TextBoxHelper textboxHelper, ThemeHelper themeHelper) : base(mainform, textboxHelper, themeHelper)
        {
        }

        protected override void Paste_CtrlShift_Text()
        {
            bool b = Do();
            if (b)
                return;

            base.Paste_CtrlShift_Text();
        }

        private bool Do()
        {
            string _text = Clipboard.GetText();
            _text = Core.Utils.TrimText(_text, "\r\n");

            bool _first = true;
            string[] _header = new string[0];

            StringBuilder _sb = new StringBuilder();


            bool _isCsv = Core.Utils.GetCsvSeparator(_text, out char _separator);
            if (!_isCsv)
                return false;

            if (Core.Utils.isReorderString(SelectedText))
            {
                string _reorderString = SelectedText;
                _text = Core.Utils.CsvChange(_text, _separator, _separator, _reorderString);
            }

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
                            _sb.Append("insert into TABLEX ");
                            string[] _items = _reader.ReadCsvLine().ToArray();
                            _sb.Append("(");
                            for (int ii = 0; ii < _header.Length; ii++)
                            {
                                _sb.Append($"{Core.Utils.ToSqlAscii(_header[ii])},");
                            }
                            _sb.Length--;

                            _sb.Append(") values (");

                            for (int ii = 0; ii < _header.Length; ii++)
                            {
                                _sb.Append($"'{Core.Utils.ToSqlAscii(_items[ii])}',");
                            }
                            _sb.Length--;
                            _sb.Append(");\r\n");
                        }
                    }
                }
            }

            SelectedText = _sb.ToString();

            return true;
        }

        protected override void Paste_CtrlShift_TextImage()
        {
            bool b = Do();
            if (b)
                return;


            base.Paste_CtrlShift_TextImage();
        }

        protected override void Paste_CtrlAltShift_TextImage()
        {
            bool b = Do();
            if (b)
                return;


            base.Paste_CtrlAltShift_TextImage();
        }


    }
}