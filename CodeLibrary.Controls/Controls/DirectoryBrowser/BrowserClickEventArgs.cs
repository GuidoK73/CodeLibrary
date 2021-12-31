using System;

namespace CodeLibrary.Controls.DirectoryBrowser
{
    public class BrowserClickEventArgs : EventArgs
    {
        public BrowserClickEventArgs()
        {
        }

        public String Path { get; set; }
    }
}