using CodeLibrary.Core.DevToys;
using CodeLibrary.Helpers;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CodeLibrary.Editor.EditorLanguageHelpers
{
    public class CSharpEditorClipboardHelper : EditorClipboardHelperBase
    {
        public CSharpEditorClipboardHelper(FormCodeLibrary mainform, TextBoxHelper textboxHelper, ThemeHelper themeHelper) : base(mainform, textboxHelper, themeHelper)
        {
        }


        protected override void Paste_CtrlShift_Text()
        {
            StringBuilder _sb = new StringBuilder();
            string _text = Clipboard.GetText();

            bool _isCsv = Core.Utils.GetCsvSeparator(_text, out char _separator);
            if (_isCsv)
            {
                if (Core.Utils.isReorderString(SelectedText))
                {
                    string _reorderString = SelectedText;
                    _text = Core.Utils.CsvChange(_text, _separator, _separator, _reorderString);
                }


                string[] _header = Core.Utils.CsvHeader(_text, _separator);

                _sb.AppendLine("public class Item");
                _sb.AppendLine("{");
                foreach (string item in _header)
                {                    
                    _sb.AppendLine($"\tpublic string {Core.Utils.CamelCaseUpper(item)} {{ get; set; }}");
                }
                _sb.AppendLine("}");
                _sb.AppendLine();
                _sb.AppendLine("List<Item> _items = new List<Item>();");
                _sb.AppendLine();

                byte[] byteArray = Encoding.Default.GetBytes(_text);
                bool _first = true;
                int _columnCount = 0;

                using (MemoryStream _stream = new MemoryStream(byteArray))
                {
                    using (CsvStreamReader _reader = new CsvStreamReader(_stream))
                    {
                        _reader.Separator = _separator;
                        _sb.Append("\r\n");
                        while (!_reader.EndOfCsvStream)
                        {
                            string[] _items = _reader.ReadCsvLine().ToArray();
                            if (_items.Length > 1)
                            {
                                if (!_first)
                                {
                                    if (_items.Length != _columnCount)
                                    {
                                        continue;
                                    }
                                    _sb.Append("_items.Add(new Item() { ");
                                    for (int ii = 0; ii < _items.Length; ii++)
                                    {
                                        _sb.Append($"{Core.Utils.CamelCaseUpper(_header[ii])} = \"{_items[ii].Replace("\"", "\\\"")}\", ");
                                    }
                                    _sb.Length = _sb.Length -2;
                                    _sb.Append(" });\r\n");
    
                                }
                                else
                                {
                                    _columnCount = _items.Length;
                                    _first = false;
                                }
                            }

                        }
                    }
                }



                SelectedText = _sb.ToString();
                return;
            }
            base.Paste_Text();
        }

        protected override void Paste_CtrlAltShift_Text() => Paste_CtrlShift_Text();
    }
}