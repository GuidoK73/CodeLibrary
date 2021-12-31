namespace CodeLibrary.Controls.DirectoryBrowser
{

    public delegate void BrowserFileClickEventHandler(object sender, BrowserClickEventArgs ce);
    public delegate void BrowserReadingEventHandler(object sender, BrowserReadingEventArgs ce);
    public delegate void BrowserFileRightClickEventHandler(object sender, BrowserClickEventArgs ce);
    public delegate void BrowserFilterPathEventHandler(object sender, BrowserFilterPathEventArgs ea);
    public delegate void BrowserFolderClickEventHandler(object sender, BrowserClickEventArgs ce);
    public delegate void BrowserFolderRightClickEventHandler(object sender, BrowserClickEventArgs ce);
    public delegate void BrowserHighlightFolderRequestEventHandler(object sender, BrowserHighlightFolderRequestEventArgs ce);
    public delegate void BrowserIconRequestEventHandler(object sender, BrowserIconRequestEventArgs ce);
    public delegate void BrowserReDisplayDirectoryNameEventHandler(object sender, BrowserReDisplayNameEventArgs ce);
    public delegate void BrowserReDisplayFileNameEventHandler(object sender, BrowserReDisplayNameEventArgs ce);
}