using org.mariuszgromada.math.mxparser;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CodeLibrary.Core.MarkDownTables
{
    public class MdTable
    {
        public int StartLine { get; set; } = 0;
        public int EndLine { get; set; } = 0;

        private Regex _RegexNums = new Regex("[a-zA-Z]\\d+");

        private Regex _RegexRange = new Regex("[a-zA-Z]\\d+:[a-zA-Z]\\d+");

        private Regex _RegexNumber = new Regex("\\d+");

        private Regex _RegexLetter = new Regex("[a-zA-Z]");

        public List<MdRow> Rows { get; set; } = new List<MdRow>();

        public Dictionary<string, MdCell> CellByKey = new Dictionary<string, MdCell>();

        public MdTable()
        {            
        }

        public override string ToString()
        {
            StringBuilder _sb = new StringBuilder();
            foreach (MdRow row in Rows)
            {
                _sb.Append("|");
                foreach (MdCell cell in row.Cells)
                {
                    _sb.Append(EvalCell(cell));
                    _sb.Append("|");
                }
                _sb.Append("\r\n");
            }
            return _sb.ToString();
        }

        

        public string EvalCell(MdCell cell)
        {
            string _cellTextTrimmed = cell.Value.Trim();


            if (_cellTextTrimmed.StartsWith("="))
            {
                _cellTextTrimmed = _cellTextTrimmed.TrimStart(new char[] { '=' });

                var _rangeMatched = _RegexRange.Matches(_cellTextTrimmed);

                foreach (Match _match in _rangeMatched)
                {
                    string[] _items = _match.Value.ToLower().Split(new char[] { ':'});
                    if (CellByKey.ContainsKey(_items[0]) && CellByKey.ContainsKey(_items[1]))
                    {
                        string _letterStart = _RegexLetter.Matches(_items[0])[0].Value.ToLower();
                        string _letterEnd = _RegexLetter.Matches(_items[1])[0].Value.ToLower();

                        if (_letterStart.Equals(_letterEnd))
                        {
                            int _start = int.Parse(_RegexNumber.Matches(_items[0])[0].Value);
                            int _end = int.Parse(_RegexNumber.Matches(_items[1])[0].Value);
                            StringBuilder _sbRange = new StringBuilder();
                            for (int ii = _start; ii <= _end; ii++)
                            {
                                _sbRange.Append($"{_letterStart}{ii}+");
                            }
                            if (_sbRange.Length > 0)
                            {
                                _sbRange.Length--;
                                _cellTextTrimmed = _cellTextTrimmed.Replace(_match.Value, _sbRange.ToString());
                            }
                        }
                    }
                }

                var _matches = _RegexNums.Matches(_cellTextTrimmed);              
                foreach (Match _match in _matches)
                {
                    string _cellKey = _match.Value.ToLower();

                    if (CellByKey.ContainsKey(_cellKey))
                    {
                        string _value = EvalCell(CellByKey[_cellKey]);
                        if(string.IsNullOrWhiteSpace(_value))
                        {
                            _value = "0";
                        }
                        _cellTextTrimmed = _cellTextTrimmed.Replace(_match.Value, _value);
                    }
                }


                // EVALUATE FUNCTIONS

                mXparser _parser = new mXparser();

                cell.Value = EvaluateExpression(_cellTextTrimmed).ToString(CultureInfo.InvariantCulture);
            }
            return cell.Value;
        }


        //public static double EvaluateExpression(string s)
        //{
        //    s = "max(1,2,3,4,5,-5,343,3)";
        //    Expression _expression = new Expression(s);
        //    double _value = _expression.calculate();
        //    return _value;
        //}

        public static double EvaluateExpression(string s)
        {
            if (string.IsNullOrWhiteSpace(s.Trim()))
            {
                return 0;
            }
            double val = 0;
            try
            {
                val = (double)new System.Xml.XPath.XPathDocument
                (new StringReader("<r/>")).CreateNavigator().Evaluate
                (string.Format("number({0})", new
                System.Text.RegularExpressions.Regex(@"([\+\-\*])")
                .Replace(s, " ${1} ")
                .Replace("/", " div ")
                .Replace("%", " mod ")));
            }
            catch { }
            {
            }
            return val;
        }
    }

}
