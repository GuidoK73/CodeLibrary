﻿using CodeLibrary.Core;
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
        protected override void Paste_CtrlAltShift_Text()
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
                NetType[] _columnTypes = new NetType[_header.Length];
                bool[] _columnNullable = new bool[_header.Length];


                byte[] byteArray = Encoding.Default.GetBytes(_text);
                bool _first = true;
                int _columnCount = 0;

                using (MemoryStream _stream = new MemoryStream(byteArray))
                {
                    using (CsvStreamReader _reader = new CsvStreamReader(_stream))
                    {
                        while (!_reader.EndOfCsvStream)
                        {
                            string[] _items = _reader.ReadCsvLine().ToArray();
                            if (!_first)
                            {
                                
                                for (int ii = 0; ii < _items.Length; ii++)
                                {
                                    if (_items.Length == _header.Length)
                                    {
                                        NetType _netType = TypeUtils.BestNetType(_columnTypes[ii], _items[ii]);
                                        if (_netType != NetType.Null)
                                        {
                                            _columnTypes[ii] = _netType;
                                        }
                                        else
                                        {
                                            if (_columnTypes[ii] != NetType.String && _columnTypes[ii] != NetType.Unknown)
                                            {
                                                _columnNullable[ii] = true;
                                            }
                                        }
                                    }
                                }
                            }
                            _first = false;
                        }
                    }
                }


                _sb.AppendLine("public class Item");
                _sb.AppendLine("{");
                for (int ii = 0; ii < _header.Length; ii++)
                {
                    string _item = _header[ii];
                    string _typename = TypeUtils.GetType(_columnTypes[ii]).Name;

                    if (_columnNullable[ii])
                    {
                        _sb.AppendLine($"\tpublic {_typename}? {Utils.CamelCaseUpper(_item)} {{ get; set; }}");
                    }
                    else
                    {
                        _sb.AppendLine($"\tpublic {_typename} {Utils.CamelCaseUpper(_item)} {{ get; set; }}");
                    }
                }
                _sb.AppendLine("}");
                _sb.AppendLine();
                _sb.AppendLine("List<Item> _items = new List<Item>();");
                _sb.AppendLine();

                byteArray = Encoding.Default.GetBytes(_text);
                _first = true;
                _columnCount = 0;



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

                                        object _value = TypeUtils.GetTypedValue(_items[ii], _columnTypes[ii]);
                                        string _code = TypeUtils.CSharpTypeConstructorCode(_value, _columnTypes[ii]);

                                        _sb.Append($"{Core.Utils.CamelCaseUpper(_header[ii])} = {_code}, ");
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
                return;
            }
            base.Paste_Text();
        }



    }
}