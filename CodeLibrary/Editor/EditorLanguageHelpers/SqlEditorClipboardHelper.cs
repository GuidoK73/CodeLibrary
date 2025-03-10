using CodeLibrary.Core;
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
            bool b = Execute();
            if (b)
                return;

            base.Paste_CtrlShift_Text();
        }

        private bool Execute()
        {
            string _text = Clipboard.GetText();
            _text = Core.Utils.TrimText(_text, "\r\n");

            bool _first = true;
            string[] _header = new string[0];

            StringBuilder _sb = new StringBuilder();


            bool _isCsv = CsvUtils.GetCsvSeparator(_text, out char _separator);
            if (!_isCsv)
                return false;

            if (Utils.isReorderString(SelectedText))
            {
                string _reorderString = SelectedText;
                _text = CsvUtils.CsvChange(_text, _separator, _separator, _reorderString);
            }

            var _schema = CsvUtils.CsvSchema(_text).ToArray();


            _sb.Append("Create table dbo.MY_TABLE\r\n");
            _sb.Append("(\r\n");
            foreach (var item in _schema)
            {
                _sb.Append($"\t{TypeUtils.SqlTypeDefinition(item.DotNetType, item.Name, item.Nullable, false, 0)},");
                if (item.IsLast)
                {
                    _sb.Length--;
                }
                _sb.Append("\r\n");
            }
            _sb.Append(")\r\nGO\r\n\r\n");

            byte[] byteArray = Encoding.Default.GetBytes(_text);
            using (MemoryStream _stream = new MemoryStream(byteArray))
            {
                using (CsvStreamReader _reader = new CsvStreamReader(_stream))
                {
                    _reader.Separator = _separator;
                    while (!_reader.EndOfStream)
                    {
                        if (_first)
                        {
                            _header = _reader.ReadCsvLine().ToArray();
                            _first = false;
                        }
                        else
                        {
                            _sb.Append("insert into MY_TABLE ");
                            string[] _items = _reader.ReadCsvLine().ToArray();
                            _sb.Append("(");
                            for (int ii = 0; ii < _schema.Count(); ii++)
                            {
                                _sb.Append($"[{Utils.ToSqlAscii(_schema[ii].Name)}],");
                            }
                            _sb.Length--;

                            _sb.Append(") values (");

                            for (int ii = 0; ii < _schema.Count(); ii++)
                            {
                                object _value = TypeUtils.GetTypedValue(_items[ii], _schema[ii].DotNetType);
                                string _code = TypeUtils.SqlTypeConstructorCode(_value, _schema[ii].DotNetType);
                                _sb.Append($"{_code},");
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
            bool b = Execute();
            if (b)
                return;


            base.Paste_CtrlShift_TextImage();
        }


    }
}