using System;

namespace CodeLibrary.Controls.DirectoryBrowser
{
    public class BrowserReDisplayNameEventArgs : EventArgs
    {
        public BrowserReDisplayNameEventArgs()
        {
        }

        public string DisplayName { get; set; }

        public string Path { get; set; }
    }
}