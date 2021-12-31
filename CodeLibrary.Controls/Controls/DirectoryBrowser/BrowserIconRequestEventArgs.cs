using System;

namespace CodeLibrary.Controls.DirectoryBrowser
{
    public class BrowserIconRequestEventArgs : EventArgs
    {
        private string _path;

        public BrowserIconRequestEventArgs()
        {
        }

        public string Path
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
    }
}