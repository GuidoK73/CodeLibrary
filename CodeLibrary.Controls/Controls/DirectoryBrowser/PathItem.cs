using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeLibrary.Controls.DirectoryBrowser
{
    public enum PathItemType
    {
        RealFile = 0,
        RealDirectory = 1,
        LinkedFile = 2,
        LinkedDirectory = 3,
        VirtualDirectory = 4

    }


    public class PathItem
    {
        public PathItemType Type { get; set; } = PathItemType.RealDirectory;

        public string Path { get; set; }

        public string LinkedPath { get; set; }

        public string WorkingPath
        {
            get
            {
                if (Type == PathItemType.RealDirectory || Type == PathItemType.RealFile || Type == PathItemType.VirtualDirectory)
                    return Path;

                return LinkedPath;
            }
        }

        public FileInfo GetFileInfo()
        {
            return new FileInfo(WorkingPath);
        }

        public FileInfo GetDirectoryInfo()
        {
            return new FileInfo(WorkingPath);
        }

        public override string ToString()
        {
            return WorkingPath;
        }

        public static PathItem RealDirectory(string path)
        {
            return new PathItem() { Path = path, Type = PathItemType.RealDirectory };
        }

        public static PathItem RealFile(string path)
        {
            return new PathItem() { Path = path, Type = PathItemType.RealFile };
        }
    }
}
