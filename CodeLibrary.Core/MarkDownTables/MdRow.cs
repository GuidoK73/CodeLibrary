using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeLibrary.Core.MarkDownTables
{
    public class MdRow
    {
        public int RowNumber { get; set; }

        public MdRow(MdTable table, int rowNum, string[] items)
        {
            RowNumber = rowNum;
            for (int colNum = 0; colNum < items.Length; colNum++)
            {
                MdCell _cell = new MdCell() { ColumnNumber = colNum, RowNumber = rowNum, Value = items[colNum] };
                table.CellByKey.Add(_cell.Key, _cell);
                Cells.Add(_cell);
            }
        }

        public List<MdCell> Cells { get; set; } = new List<MdCell>();
    }
}
