namespace CodeLibrary.Controls.DirectoryBrowser
{
    public enum FileSort
    {
        NoSorting = 0,
        FileName_Asc = 1,
        FileName_Desc = 2,
        FileDateCreation_Asc = 3,
        FileDateCreation_Desc = 4,
        FileDateLastEdit_Asc = 5,
        FileDateLastEdit_Desc = 6
    }

    public enum TreeMode
    {
        AutomaticDirectory = 0,
        ManualDirectory = 1
    }

    internal enum NodeType
    {
        /// <summary>
        /// Node is a folder
        /// </summary>
        Folder = 0,

        /// <summary>
        /// Node is a file
        /// </summary>
        File = 1,

        /// <summary>
        /// Node is a folder but not yet expanded.
        /// </summary>
        NeedsExpanding = 2,

        /// <summary>
        /// Should not be used.
        /// </summary>
        Unknown = 99
    }
}