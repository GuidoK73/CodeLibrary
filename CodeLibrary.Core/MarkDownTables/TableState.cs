using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeLibrary.Core.MarkDownTables
{
    public enum TableState
    {
        NoTable = 0,
        TableStarted = 1,
        TableScanning = 2,
        TableEnded = 3
    }
}
