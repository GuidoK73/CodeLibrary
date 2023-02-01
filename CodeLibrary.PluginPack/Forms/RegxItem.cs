using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeLibrary.PluginPack.Forms
{
    public class RegexItem
    {
        public string Name { get; set; }

        public string Category { get; set; }

        public string Find { get; set; }
        public string Replace { get; set; }

        public override string ToString()
        {
            return $"{Name}";
        }
    }
}
