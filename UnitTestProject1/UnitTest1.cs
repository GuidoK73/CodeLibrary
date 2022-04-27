using CodeLibrary.Core;
using CodeLibrary.Core.DevToys;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Linq;
using System.Text;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {

            string _test = @"Field1,Field2,Field3
,,bbbbbb
cccccc,2011-12-22 00:00:00.000,eeeeee
dddddd,,1000
";



            StringBuilder _sb = new StringBuilder();
            string _text = _test;

            bool _isCsv = Utils.GetCsvSeparator(_text, out char _separator);
            if (_isCsv)
            {
                string[] _header = Utils.CsvHeader(_text, _separator);
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

                                        _sb.Append($"{Utils.CamelCaseUpper(_header[ii])} = {_code}, ");
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

                string _result = _sb.ToString();
            }
        }
    }
}
