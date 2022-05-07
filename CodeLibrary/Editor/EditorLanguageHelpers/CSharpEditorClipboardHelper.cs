using CodeLibrary.Core;
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

        private bool Execute()
        {
            StringBuilder _sb = new StringBuilder();
            string _text = Clipboard.GetText();

            switch (GetClipboardTextType())
            {
                case TextType.Csv:

                    CsvUtils.GetCsvSeparator(_text, out char _separator);
                    if (Utils.isReorderString(SelectedText))
                    {
                        string _reorderString = SelectedText;
                        _text = CsvUtils.CsvChange(_text, _separator, _separator, _reorderString);
                    }

                    var _schema = CsvUtils.CsvSchema(_text).ToArray();


                    _sb.AppendLine("public class Item");
                    _sb.AppendLine("{");

                    foreach (CsvColumnInfo columnInfo in _schema)
                    {
                        string _typename = TypeUtils.GetType(columnInfo.DotNetType).Name;

                        if (columnInfo.Nullable)
                        {
                            _sb.AppendLine($"\tpublic {_typename}? {Utils.CamelCaseUpper(columnInfo.Name)} {{ get; set; }}");
                        }
                        else
                        {
                            _sb.AppendLine($"\tpublic {_typename} {Utils.CamelCaseUpper(columnInfo.Name)} {{ get; set; }}");
                        }
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
                                            CsvColumnInfo _columnInfo = _schema[ii];

                                            object _value = TypeUtils.GetTypedValue(_items[ii], _columnInfo.DotNetType);
                                            string _code = TypeUtils.CSharpTypeConstructorCode(_value, _columnInfo.DotNetType);

                                            _sb.Append($"{Core.Utils.CamelCaseUpper(_columnInfo.Name)} = {_code}, ");
                                        }
                                        _sb.Length = _sb.Length - 2;
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
                    return true;

                case TextType.Json:

                    SelectedText = JsonUtils.JsonToCSharp(_text);

                    return true;

                case TextType.Xml:
                    string _xml = Utils.FormatXml(_text, out bool _succes);
                    string _json = JsonUtils.ConvertXmlToJson(_xml);
                    SelectedText = JsonUtils.JsonToCSharp(_json);

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

        protected override void Paste_CtrlShift_TextImage()
        {
            bool _succes = Execute();
            if (_succes)
                return;

            base.Paste_CtrlShift_Text();
        }
    }
}