using System;

namespace CodeLibrary.Controls.DirectoryBrowser
{
    public class BrowserFilterPathEventArgs : EventArgs
    {
        public BrowserFilterPathEventArgs()
        {
            Show = true;
        }

        public string Path { get; set; }

        public bool Show { get; set; }

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