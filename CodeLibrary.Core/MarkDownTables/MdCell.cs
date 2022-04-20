using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeLibrary.Core.MarkDownTables
{
    public class MdCell
    {
        public int ColumnNumber { get; set; }

        public int RowNumber { get; set; }

        public string Value { get; set; }

        public override string ToString() => Value;

        public string Key => $"{(char)(ColumnNumber + 97)}{RowNumber}";
    }
}
