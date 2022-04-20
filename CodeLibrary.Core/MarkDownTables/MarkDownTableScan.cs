using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeLibrary.Core.MarkDownTables
{
    public class MarkDownTableScan
    {
        private string[] _Lines;

        private List<MdTable> _Tables = new List<MdTable>();

        public string Eval(string text)
        {
            _Tables = new List<MdTable>();
            MdTable _current = null;
            TableState _state = TableState.NoTable;
            int _rowNum = 0;

            _Lines = Utils.SplitLines(text);

            for (int linenumber = 0; linenumber < _Lines.Length; linenumber++)
            {
                string _line = _Lines[linenumber];
                string _trimmed = _line.Trim();
                if (_trimmed.StartsWith("|") && _trimmed.EndsWith("|"))
                {
                    if (_state == TableState.NoTable)
                    {
                        _state = TableState.TableStarted;
                    }
                    if (_state == TableState.TableScanning)
                    {
                        // OK
                    }
                }
                else
                {
                    if (_state == TableState.TableScanning)
                    {
                        _state = TableState.TableEnded;
                    }
                }

                if (_state == TableState.TableStarted)
                {
                    _rowNum = 0;
                    _state = TableState.TableScanning;
                    _current = new MdTable();
                    _current.StartLine = linenumber;
                    _Tables.Add(_current);
                }
                if (_state == TableState.TableEnded)
                {
                    _rowNum = 0;
                    _current.EndLine = linenumber - 1;
                    _state = TableState.NoTable;
                }

                if (_state == TableState.TableScanning)
                {
                    string _trimmed2 = _trimmed.Trim(new char[] { '|' });
                    string[] _stringRow = _trimmed2.Split(new char[] { '|' });

                    MdRow _row = new MdRow(_current, _rowNum, _stringRow);
                    _current.Rows.Add(_row);
                    _rowNum++;
                }



            }


            return Merge();
        }

        private string Merge()
        {
            StringBuilder _sb = new StringBuilder();
            for (int linenumber = 0; linenumber < _Lines.Length; linenumber++)
            {
                string _line = _Lines[linenumber];
                if (!LineIsTable(linenumber))
                {
                    _sb.AppendLine(_line);
                }
                else
                {
                    var _table = GetTable(linenumber);
                    _sb.Append(_table.ToString());
                    linenumber = _table.EndLine;
                }

            }
            return _sb.ToString();
        }

        private bool LineIsTable(int LineNumber)
        {
            foreach (MdTable table in _Tables)
            {
                if (LineNumber >= table.StartLine && LineNumber <= table.EndLine)
                {
                    return true;
                }
            }
            return false;
        }

        private MdTable GetTable(int LineNumber)
        {
            foreach (MdTable table in _Tables)
            {
                if (LineNumber >= table.StartLine && LineNumber <= table.EndLine)
                {
                    return table;
                }
            }
            return null;
        }
    }
}
