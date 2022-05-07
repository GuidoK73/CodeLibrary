using CodeLibrary.Core.DevToys;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace CodeLibrary.Core
{
    public static class CsvUtils
    {
        public static string CsvChange(string text, char separator, char newSeparator, string reorder)
        {
            if (!string.IsNullOrEmpty(reorder))
            {
                string[] _reorderString = reorder.Split(new char[] { ',' });
                int[] _reorder = new int[_reorderString.Length];
                for (int ii = 0; ii < _reorder.Length; ii++)
                {
                    _reorder[ii] = Convert.ToInt32(_reorderString[ii]);
                }
                return CsvChange(text, separator, newSeparator, _reorder);
            }
            return CsvChange(text, separator, newSeparator);
        }

        public static string CsvChange(string text, char separator, char newSeparator, params int[] reorder)
        {
            byte[] byteArray = Encoding.Default.GetBytes(text);
            using (MemoryStream _inputStream = new MemoryStream(byteArray))
            {
                using (CsvStreamReader _reader = new CsvStreamReader(_inputStream))
                {
                    using (MemoryStream _outputStream = new MemoryStream())
                    {
                        using (StreamWriter _streamWriter = new StreamWriter(_outputStream))
                        {
                            _reader.Separator = separator;
                            using (CsvStreamWriter _writer = new CsvStreamWriter(_outputStream))
                            {
                                _writer.Separator = newSeparator;
                                while (!_reader.EndOfCsvStream)
                                {
                                    var _items = _reader.ReadCsvLine().ToArray();

                                    if (reorder != null && reorder.Length > 0 && reorder.Length <= _items.Length && reorder.Max(p => p) <= _items.Length)
                                    {
                                        var _newItems = new string[reorder.Count()];
                                        for (int ii = 0; ii < reorder.Length; ii++)
                                        {
                                            _newItems[ii] = _items[reorder[ii]];
                                        }
                                        _writer.WriteCsvLine(_newItems);
                                    }
                                    else
                                    {
                                        _writer.WriteCsvLine(_items);
                                    }
                                }
                                return Utils.StreamToString(_outputStream);
                            }
                        }
                    }
                }
            }
        }

        public static string[] CsvHeader(string text, char separator)
        {
            byte[] byteArray = Encoding.Default.GetBytes(text);
            using (MemoryStream _stream = new MemoryStream(byteArray))
            {
                using (CsvStreamReader _reader = new CsvStreamReader(_stream))
                {
                    _reader.Separator = separator;
                    return _reader.ReadCsvLine().ToArray();
                }
            }
        }

        public static IEnumerable<CsvColumnInfo> CsvSchema(string text)
        {
            bool _succes = CsvUtils.GetCsvSeparator(text, out char _separator);
            if (!_succes)
                throw new ArgumentException("text is not valid csv!");

            string[] _header = CsvHeader(text, _separator);
            NetType[] _columnTypes = new NetType[_header.Length];
            bool[] _columnNullable = new bool[_header.Length];

            byte[] byteArray = Encoding.Default.GetBytes(text);
            bool _first = true;

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

            for (int ii = 0; ii < _header.Length; ii++)
            {
                yield return new CsvColumnInfo()
                {
                    Name = _header[ii],
                    DotNetType = _columnTypes[ii],
                    Nullable = _columnNullable[ii],
                    Index = ii,
                    DatabaseType = TypeUtils.GetDbType(_columnTypes[ii]),
                    SqlDatabaseType = TypeUtils.GetSqlDbType(_columnTypes[ii]),
                    IsLast = (ii == _header.Length - 1)
                };
            }
        }

        public static string CsvToJSon(string text, char separator)
        {
            string[] _header = CsvHeader(text, separator);

            StringBuilder _sb = new StringBuilder();
            byte[] byteArray = Encoding.Default.GetBytes(text);
            int _columnCount = 0;
            bool _first = true;
            using (MemoryStream _stream = new MemoryStream(byteArray))
            {
                using (CsvStreamReader _reader = new CsvStreamReader(_stream))
                {
                    _reader.Separator = separator;
                    _sb.Append("[\r\n");
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
                                _sb.Append("\t{\r\n");
                                for (int ii = 0; ii < _items.Length; ii++)
                                {
                                    _sb.Append($"\t\t\"{_header[ii].Replace("\"", "\\\"")}\": \"{_items[ii].Replace("\"", "\\\"")}\",\r\n");
                                }
                                _sb.Length = _sb.Length - 3;
                                _sb.Append("\r\n\t},\r\n");
                            }
                            else
                            {
                                _columnCount = _items.Length;
                                _first = false;
                            }
                        }
                    }
                    _sb.Length = _sb.Length - 3;
                    _sb.Append("\r\n]\r\n");
                }
            }
            return _sb.ToString();
        }

        public static string CsvToJSonNoHeader(string text, char separator)
        {
            StringBuilder _sb = new StringBuilder();
            byte[] byteArray = Encoding.Default.GetBytes(text);
            int _columnCount = 0;
            bool _first = true;
            using (MemoryStream _stream = new MemoryStream(byteArray))
            {
                using (CsvStreamReader _reader = new CsvStreamReader(_stream))
                {
                    _reader.Separator = separator;
                    _sb.Append("[\r\n");
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
                                _sb.Append("\t[");
                                for (int ii = 0; ii < _items.Length; ii++)
                                {
                                    _sb.Append($"\"{_items[ii].Replace("\"", "\\\"")}\",");
                                }
                                _sb.Length = _sb.Length - 1;
                                _sb.Append("],\r\n");
                            }
                            else
                            {
                                _columnCount = _items.Length;
                                _first = false;
                            }
                        }
                    }
                    _sb.Length = _sb.Length - 3;
                    _sb.Append("\r\n]\r\n");
                }
            }
            return _sb.ToString();
        }

        public static string CsvToMdTable(string text, char separator)
        {
            StringBuilder _sb = new StringBuilder();
            byte[] byteArray = Encoding.Default.GetBytes(text);
            int _columnCount = 0;
            bool _first = true;
            using (MemoryStream _stream = new MemoryStream(byteArray))
            {
                using (CsvStreamReader _reader = new CsvStreamReader(_stream))
                {
                    _reader.Separator = separator;
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
                                _sb.Append("|");
                                _sb.Append(String.Join("|", _items));
                                _sb.Append("|\r\n");
                            }
                            else
                            {
                                _columnCount = _items.Length;
                                _first = false;
                                _sb.Append("|");
                                _sb.Append(String.Join("|", _items));
                                _sb.Append("|\r\n");
                                for (int ii = 0; ii < _items.Length; ii++)
                                {
                                    _items[ii] = ":-";
                                }
                                _sb.Append("|");
                                _sb.Append(String.Join("|", _items));
                                _sb.Append("|\r\n");
                            }
                        }
                    }
                }
            }
            return _sb.ToString();
        }

        public static bool GetCsvSeparator(string text, out char separator)
        {
            char[] _tests = new char[] { ',', ';', '\t', '|' };
            foreach (char c in _tests)
            {
                if (IsCsv(text, c, 20))
                {
                    separator = c;
                    return true;
                }
            }
            separator = ' ';
            return false;
        }

        private static bool IsCsv(string text, char separator, int sample)
        {
            byte[] byteArray = Encoding.Default.GetBytes(text);
            List<int> _columnCount = new List<int>();

            using (MemoryStream _stream = new MemoryStream(byteArray))
            {
                using (CsvStreamReader _reader = new CsvStreamReader(_stream))
                {
                    _reader.Separator = separator;
                    while (!_reader.EndOfCsvStream)
                    {
                        string[] _items = _reader.ReadCsvLine().ToArray();
                        int _length = _items.Length == 1 && string.IsNullOrWhiteSpace(_items[0]) ? 0 : _items.Length;
                        if (_length > 0)
                        {
                            _columnCount.Add(_length);
                        }
                    }
                    int prevcount = _columnCount.First();
                    foreach (int count in _columnCount)
                    {
                        if (count == 1)
                            return false;
                        if (count != prevcount)
                            return false;
                    }
                }
            }
            return true;
        }
    }

    public class CsvColumnInfo
    {
        public NetType DotNetType { get; set; }

        public SqlDbType SqlDatabaseType { get; set; }

        public DbType DatabaseType { get; set; }

        public int Index { get; set; }
        public bool IsLast { get; set; }
        public string Name { get; set; }
        public bool Nullable { get; set; }
    }
}