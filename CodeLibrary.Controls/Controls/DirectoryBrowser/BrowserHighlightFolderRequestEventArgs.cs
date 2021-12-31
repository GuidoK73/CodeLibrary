using System;

namespace CodeLibrary.Controls.DirectoryBrowser
{
    public class BrowserHighlightFolderRequestEventArgs : EventArgs
    {
        public BrowserHighlightFolderRequestEventArgs()
        {
        }

        public bool Highlight { get; set; }

        public string Path { get; set; }
    }
}