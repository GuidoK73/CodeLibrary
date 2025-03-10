using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeLibrary.Core
{
    public class MDTabify
    {
        public List<string[]> MarkDownTableToArray(string text)
        {
            StringBuilder sb = new StringBuilder();

            string[] _lines = SplitLines(text, true);

            List<string[]> _records = new List<string[]>();

            foreach (string line in _lines)
            {
                _records.Add(line.Split('|'));
            }
            return _records;
        }

        public string TabifyTable(string text, int tabsize)
        {
            StringBuilder sb = new StringBuilder();

            string[] _lines = SplitLines(text, true);

            List<string[]> _records = new List<string[]>();

            foreach (string line in _lines)
            {
                line.Replace("\t", new string(' ', tabsize));
                _records.Add(line.Split('|'));
            }
            int _column = 0;


            foreach (string[] record in _records)
            {
                for (int ii = 0; ii < record.Length; ii++)
                {
                    record[ii] = " " + record[ii].Trim() + " ";
                }
            }

            int _maxCols = _records.Select(p => p.Length).Max();
            int _recordCount = 0;
            while (true)
            {
                if (_column >= _maxCols)
                {
                    break;
                }
                var maxColumnLength = _records.Where(p => p.Count() > _column).Select(p => p[_column].Length).Max();
                _recordCount = 0;
                foreach (string[] record in _records)
                {
                    if (record.Length <= _column)
                    {
                        break;
                    }
                    string _field = record[_column];
                    int _fieldLength = _field.Length;
                    int _spaceToAdd = maxColumnLength - _fieldLength;
                    record[_column] += new string(' ', _spaceToAdd);
                    _recordCount++;
                }

                _column++;
            }
            foreach (string[] _items in _records)
            {
                string _line = string.Join("|", _items);
                sb.AppendLine(_line.Trim());
            }


            return sb.ToString();
        }

        private static string[] SplitLines(string text, bool skipEmpty)
        {
            var _result = new List<string>();
            var _partBuilder = new StringBuilder();
            var _textCharArray = text.ToCharArray();
            var _prevChar = (char)0;
            var _line = string.Empty;

            for (int ii = 0; ii < _textCharArray.Length; ii++)
            {
                char _currChar = _textCharArray[ii];

                if (_currChar == '\n' && _prevChar == '\r')
                {
                    _partBuilder.Length--;
                    _line = _partBuilder.ToString();
                    if (skipEmpty == false)
                    {
                        _result.Add(_line);
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(_line))
                        {
                            _result.Add(_line);
                        }
                    }
                    _partBuilder = new StringBuilder();
                    _prevChar = (char)0;
                    continue;
                }
                if (_currChar == '\n' && _prevChar != '\r')
                {
                    _line = _partBuilder.ToString();
                    if (skipEmpty == false)
                    {
                        _result.Add(_line);
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(_line))
                        {
                            _result.Add(_line);
                        }
                    }
                    _partBuilder = new StringBuilder();
                    _prevChar = (char)0;
                    continue;
                }
                if (_prevChar == '\n' || _prevChar == '\r')
                {
                    if (_currChar != '\r' && _currChar != '\n')
                        _partBuilder.Append(_currChar);

                    _line = _partBuilder.ToString();
                    if (skipEmpty == false)
                    {
                        _result.Add(_line);
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(_line))
                        {
                            _result.Add(_line);
                        }
                    }
                    _partBuilder = new StringBuilder();
                    _prevChar = _textCharArray[ii];
                    continue;
                }
                _partBuilder.Append(_currChar);
                _prevChar = _textCharArray[ii];
            }
            return _result.ToArray();
        }
    }
}
