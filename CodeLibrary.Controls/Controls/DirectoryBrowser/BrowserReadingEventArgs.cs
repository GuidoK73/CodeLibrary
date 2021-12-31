using System;

namespace CodeLibrary.Controls.DirectoryBrowser
{
    public class BrowserReadingEventArgs : EventArgs
    {
        private String _path;

        public BrowserReadingEventArgs()
        {
        }

        public String Path
        {
            get
            {
                return _path;
            }
            set
            {
                _path = value;
            }
        }

        public bool IsDirectory
        {
            get
            {
                return (DirectoryBrowserUtils.IsFileOrDirectory(Path) == DirectoryBrowserUtils.FileOrDirectory.Directory);
            }
        }

        public bool IsFile
        {
            get
            {
                return (DirectoryBrowserUtils.IsFileOrDirectory(Path) == DirectoryBrowserUtils.FileOrDirectory.File);
            }
        }
    }
}
