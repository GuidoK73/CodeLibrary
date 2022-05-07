using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace CodeLibrary.Controls.DirectoryBrowser
{
    [DefaultEvent("BrowserFolderClick")]
    public partial class DirectoryBrowser : UserControl
    {
        private const int FILE_ATTRIBUTE_NORMAL = 0x80;
        private const int MAX_PATH = 256;
        private const string NEEDEXPANDING = "#EMPTY#";

        private const uint SHGFI_ICON = 0x100;

        private const uint SHGFI_LARGEICON = 0x0;
        private const uint SHGFI_SMALLICON = 0x1;
        private const UInt32 TV_FIRST = 4352;
        private const UInt32 TVGN_ROOT = 0;
        private const UInt32 TVIF_HANDLE = 16;
        private const UInt32 TVIF_STATE = 8;
        private const UInt32 TVIS_STATEIMAGEMASK = 61440;
        private const UInt32 TVM_GETNEXTITEM = TV_FIRST + 10;
        private const UInt32 TVM_SETIMAGELIST = TV_FIRST + 9;
        private const UInt32 TVM_SETITEM = TV_FIRST + 13;
        private const UInt32 TVSIL_NORMAL = 0;
        private const UInt32 TVSIL_STATE = 2;
        private bool _CheckBoxes = false;
        private string _CurrentSelection = string.Empty;
        private List<KeyValuePair<string, PathItem>> _CustomPaths = new List<KeyValuePair<string, PathItem>>();
        private ILookup<string, PathItem> _CustomPathsLookup;
        private bool _IncludeFiles = false;
        private bool _InitLookup = true;
        private string _RootDirectory = string.Empty;
        private List<FileSystemWatcher> _Watchers = new List<FileSystemWatcher>();

        public DirectoryBrowser()
        {
            InitializeComponent();
        }

        [Category("DirectoryBrowser")]
        [Description("Raised when a file is clicked")]
        public event BrowserFileClickEventHandler FileClick;

        [Category("DirectoryBrowser")]
        [Description("Raised when a file is right-clicked, when assigned the system menu will not be shown.")]
        public event BrowserFileRightClickEventHandler FileRightClick;

        [Category("DirectoryBrowser")]
        [Description(" Raise prior to each folder / file is added to the treeview.")]
        public event BrowserFilterPathEventHandler FilterPathRequest;

        [Category("DirectoryBrowser")]
        [Description("Raised when a folder is clicked")]
        public event BrowserFolderClickEventHandler FolderClick;

        [Category("DirectoryBrowser")]
        [Description("Raised when a folder is right clicked, when assigned the system menu will not be shown.")]
        public event BrowserFolderRightClickEventHandler FolderRightClick;

        [Category("DirectoryBrowser")]
        [Description("Raised prior to each folder is added to the treeview, you can determine whether to highlight an item")]
        public event BrowserHighlightFolderRequestEventHandler HighlightFolderRequest;

        [Category("DirectoryBrowser")]
        [Description("Raised prior to each folder is added to the treeview, you can determine to display a custom icon.")]
        public event BrowserIconRequestEventHandler IconRequest;

        [Category("DirectoryBrowser")]
        [Description("Reading directory")]
        public event BrowserReadingEventHandler Reading;

        [Category("DirectoryBrowser")]
        [Description("Prior to rendering a filename in the treeviewer, this event enables you to rename the display name.")]
        public event BrowserReDisplayDirectoryNameEventHandler ReDisplayDirectoryName;

        [Category("DirectoryBrowser")]
        [Description("Prior to rendering a filename in the treeviewer, this event enables you to rename the display name.")]
        public event BrowserReDisplayFileNameEventHandler ReDisplayFileName;

        private enum IconSize
        {
            Small = 0,
            Large = 1
        }

        [Flags]
        private enum SHGetFileInfoConstants : int
        {
            SHGFI_ICON = 0x100, // get icon
            SHGFI_DISPLAYNAME = 0x200, // get display name
            SHGFI_TYPENAME = 0x400, // get type name
            SHGFI_ATTRIBUTES = 0x800, // get attributes
            SHGFI_ICONLOCATION = 0x1000, // get icon location
            SHGFI_EXETYPE = 0x2000, // return exe type
            SHGFI_SYSICONINDEX = 0x4000, // get system icon index
            SHGFI_LINKOVERLAY = 0x8000, // put a link overlay on icon
            SHGFI_SELECTED = 0x10000, // show icon in selected state
            SHGFI_ATTR_SPECIFIED = 0x20000, // get only specified attributes
            SHGFI_LARGEICON = 0x0, // get large icon
            SHGFI_SMALLICON = 0x1, // get small icon
            SHGFI_OPENICON = 0x2, // get open icon
            SHGFI_SHELLICONSIZE = 0x4, // get shell size icon
            SHGFI_PIDL = 0x8, // pszPath is a pidl
            SHGFI_USEFILEATTRIBUTES = 0x10, // use passed dwFileAttribute
            SHGFI_ADDOVERLAYS = 0x000000020, // apply the appropriate overlays
            SHGFI_OVERLAYINDEX = 0x000000040 // Get the index of the overlay
        }

        [Category("DirectoryBrowser")]
        [Description("Treeview backcolor")]
        public override Color BackColor
        {
            get
            {
                return Browser.BackColor;
            }
            set
            {
                Browser.BackColor = value;
            }
        }

        public override Image BackgroundImage { get => Browser.BackgroundImage; set => Browser.BackgroundImage = value; }

        [Category("DirectoryBrowser")]
        [Description("Indicate whether to use checkboxes in treeviewer to select multiple files or folders.")]
        public bool CheckBoxes
        {
            get
            {
                return _CheckBoxes;
            }
            set
            {
                _CheckBoxes = value;
                Browser.CheckBoxes = _CheckBoxes;
                FillRootDirectory(_IncludeFiles);
            }
        }

        [Category("DirectoryBrowser")]
        [Description("Get or Set the current selected path")]
        public string CurrentSelection
        {
            get
            {
                return _CurrentSelection;
            }
            set
            {
                _CurrentSelection = value;
                ExpandToCurrentSelection();
            }
        }

        [Category("DirectoryBrowser")]
        [Description("When IncludeFiles set to true determine which files to show.")]
        public string FileFilter { get; set; } = "*.*";


        [Category("DirectoryBrowser")]
        [Description("Determines how files are sorted.")]
        public FileSort FileSorting { get; set; } = FileSort.NoSorting;

        [Category("DirectoryBrowser")]
        [Description("Indicate whether to include files in the treeviewer")]
        public bool IncludeFiles
        {
            get
            {
                return _IncludeFiles;
            }
            set
            {
                _IncludeFiles = value;
                FillRootDirectory(_IncludeFiles);
            }
        }

        [Category("DirectoryBrowser")]
        [Description("Only build the tree for the specified directory, leave empty for full file system.")]
        public string RootDirectory
        {
            get
            {
                return _RootDirectory;
            }
            set
            {
                _RootDirectory = value;
                FullRefresh();
            }
        }

        [Category("DirectoryBrowser")]
        [Description("Show the Desktop folder")]
        public bool ShowDesktop { get; set; } = true;

        [Category("DirectoryBrowser")]
        [Description("Show the MyFavorites Folder")]
        public bool ShowFavorites { get; set; } = true;

        [Category("DirectoryBrowser")]
        [Description("Show the MyDocuments Folder")]
        public bool ShowMyDocuments { get; set; } = true;

        [Category("DirectoryBrowser")]
        [Description("Show the MyPictures folder")]
        public bool ShowMyPictures { get; set; } = true;

        [Category("DirectoryBrowser")]
        [Description("Show the MyVideos folder")]
        public bool ShowMyVideos { get; set; } = true;

        [Category("DirectoryBrowser")]
        [Description("Show the StartMenu folder")]
        public bool ShowStartMenu { get; set; } = true;

        [Category("DirectoryBrowser")]
        [Description("AutomaticDirectory or ManualDirectory (add all files manually)")]
        public TreeMode TreeviewMode { get; set; } = TreeMode.AutomaticDirectory;

        public void AddCustomPath(string path, string linkedPath)
        {
            _InitLookup = true;
            string[] items = DirectoryBrowserUtils.SplitPath(path);

            DirectoryBrowserUtils.FileOrDirectory pathType = DirectoryBrowserUtils.IsFileOrDirectory(path);
            DirectoryBrowserUtils.FileOrDirectory linkedPathType = DirectoryBrowserUtils.IsFileOrDirectory(linkedPath);

            if (pathType != DirectoryBrowserUtils.FileOrDirectory.DoesNotExist)
            {
                return;
            }

            PathItem _path = new PathItem() { Path = path, LinkedPath = linkedPath };
            switch (linkedPathType)
            {
                case DirectoryBrowserUtils.FileOrDirectory.Directory:
                    _path.Type = PathItemType.LinkedDirectory;
                    break;

                case DirectoryBrowserUtils.FileOrDirectory.File:
                    _path.Type = PathItemType.LinkedFile;
                    break;

                case DirectoryBrowserUtils.FileOrDirectory.DoesNotExist:
                    _path.Type = PathItemType.VirtualDirectory;
                    break;
            }

            _CustomPaths.Add(new KeyValuePair<string, PathItem>(items[items.Length - 2], _path));
        }

        /// <summary>
        /// Clears all nodes added with AddPath.
        /// </summary>
        public void ClearManual()
        {
            if (TreeviewMode == TreeMode.ManualDirectory)
            {
                Browser.Nodes.Clear();
            }
        }

        public IEnumerable<PathItem> EnumerateCustomPaths(string parent)
        {
            if (_InitLookup)
            {
                _CustomPathsLookup = _CustomPaths.ToLookup(p => p.Key, p => p.Value);
            }
            _InitLookup = false;
            if (_CustomPathsLookup.Contains(parent))
            {
                foreach (PathItem item in _CustomPathsLookup[parent])
                {
                    Reading?.Invoke(this, new BrowserReadingEventArgs() { Path = item.Path });
                    yield return item;
                }
            }
        }

        /// <summary>
        /// Refresh the Treeviewer
        /// </summary>
        public void FullRefresh()
        {
            base.Refresh();
            Browser.Nodes.Clear();
            FillRootDirectory(_IncludeFiles);
        }

        /// <summary>
        /// Refresh the Treeviewer
        /// </summary>
        public override void Refresh()
        {
            base.Refresh();
            if (Browser.SelectedNode != null)
            {
                Browser.SelectedNode.Nodes.Clear();
                FillDirectory(Browser.SelectedNode.Nodes, Browser.SelectedNode.Name, _IncludeFiles);
            }
            else
            {
                Browser.Nodes.Clear();
                FillRootDirectory(_IncludeFiles);
            }
        }

        /// <summary>
        /// Returns a list of selected items
        /// </summary>
        public List<string> SelectedItems()
        {
            List<string> CurrentSelected = new List<string>();
            if (_CheckBoxes)
            {
                foreach (TreeNode tn in Browser.Nodes)
                {
                    if (tn.Checked)
                    {
                        CurrentSelected.Add(tn.Name);
                    }
                    FillSelectedItems(tn, ref CurrentSelected);
                }
            }
            else
            {
                if (Browser.SelectedNode != null)
                {
                    CurrentSelected.Add(Browser.SelectedNode.Name);
                }
            }
            return CurrentSelected;
        }

        [DllImport("user32.dll")]
        private static extern UInt32 SendMessage(IntPtr hWnd, UInt32 Msg,
        UInt32 wParam, UInt64 lParam);

        [DllImport("shell32.dll")]
        private static extern IntPtr SHGetFileInfo(string pszPath,
        uint dwFileAttributes,
        ref SHFILEINFO psfi,
        uint cbSizeFileInfo,
        uint uFlags);

        [DllImport("shell32")]
        private static extern IntPtr SHGetFileInfo(
        string pszPath,
        int dwFileAttributes,
        ref SHFILEINFO psfi,
        uint cbFileInfo,
        uint uFlags);

        private void addDriveNode(DriveInfo drv, bool selected)
        {
            if (DirectoryBrowserUtils.HasDirectoryAccess(drv.RootDirectory.FullName))
            {
                string driveName = string.Empty;
                string volumeName = string.Empty;
                if (drv.IsReady)
                {
                    volumeName = drv.VolumeLabel;
                    if (string.IsNullOrEmpty(volumeName))
                    {
                        volumeName = drv.DriveType.ToString();
                    }
                }
                else
                {
                    volumeName = drv.DriveType.ToString();
                }
                driveName = string.Format("{0} ({1})", volumeName, drv.RootDirectory.Name.Replace(@"\", ""));

                TreeNode node = AddNode(Browser.Nodes, NodeType.Folder, selected, PathItem.RealDirectory(drv.RootDirectory.FullName), driveName);
                string path = raiseEventBrowserIconRequest(drv.RootDirectory.FullName);
                node.Checked = selected;
                node.ImageIndex = GetImageIndex(path, IconSize.Small);
                node.SelectedImageIndex = node.ImageIndex;
                createWatcher(drv.RootDirectory.FullName);
            }
        }

        private TreeNode AddNode(TreeNodeCollection collection, NodeType nodetype, bool selected, PathItem path, string name)
        {
            var _node = collection.Add(path.WorkingPath, name);
            NodeTag.SetNodeType(_node, nodetype);
            _node.ImageIndex = GetImageIndex(path.WorkingPath, IconSize.Small);
            _node.SelectedImageIndex = _node.ImageIndex;
            _node.Checked = selected;

            switch (nodetype)
            {
                case NodeType.Folder:
                    _node.Nodes.Add(NeedsExpandingLode());
                    break;

                case NodeType.File:
                    break;

                default:
                    break;
            }

            return _node;
        }

        private static TreeNode NeedsExpandingLode()
        {
            var _node = new TreeNode();
            _node.Name = NEEDEXPANDING;
            return _node;
        }

        private void Browser_AfterSelect(object sender, TreeViewEventArgs e)
        {
        }

        private void Browser_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            if (TreeviewMode == TreeMode.ManualDirectory)
                return;

            if (nodeNotYetExpanded(e.Node))
            {
                e.Node.Nodes.Clear();
                FillDirectory(e.Node.Nodes, e.Node.Name, _IncludeFiles);
            }
        }

        private void Browser_DragDrop(object sender, DragEventArgs e)
        {
        }

        private void Browser_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            BrowserClickEventArgs bcea = new BrowserClickEventArgs();
            if (e.Button == MouseButtons.Right)
            {
                if (NodeTag.GetNodeType(e.Node) == NodeType.File)
                {
                    bcea.Path = e.Node.Name;
                    _CurrentSelection = bcea.Path;
                    BrowserFileRightClickEventHandler BrowserFileRightClickHandler = FileRightClick;
                    if (BrowserFileRightClickHandler != null)
                    {
                        BrowserFileRightClickHandler(this, bcea);
                        return;
                    }
                    else
                    {
                        // #FIX
                        //showContextMenu(bcea.Path);
                    }
                }
                if (NodeTag.GetNodeType(e.Node) == NodeType.Folder)
                {
                    bcea.Path = e.Node.Name;
                    _CurrentSelection = bcea.Path;
                    BrowserFolderRightClickEventHandler BrowserFolderRightClickHandler = FolderRightClick;
                    if (BrowserFolderRightClickHandler != null)
                    {
                        BrowserFolderRightClickHandler(this, bcea);
                        return;
                    }
                    else
                    {
                        // #FIX
                        //showContextMenu(bcea.Path);
                    }
                }

                Browser.SelectedNode = e.Node;
                return;
            }

            if (NodeTag.GetNodeType(e.Node) == NodeType.File)
            {
                bcea.Path = e.Node.Name;
                _CurrentSelection = bcea.Path;
                FileClick?.Invoke(this, bcea);
            }
            if (NodeTag.GetNodeType(e.Node) == NodeType.Folder)
            {
                bcea.Path = e.Node.Name;
                _CurrentSelection = bcea.Path;
                FolderClick?.Invoke(this, bcea);
            }
        }

        /// <summary>
        /// Create file system watcher for the Roots
        /// </summary>
        private void createWatcher(string path)
        {
            foreach (FileSystemWatcher wat in _Watchers)
            {
                if (wat.Path.Equals(path))
                {
                    return;
                }
            }
            if (Directory.Exists(path))
            {
                FileSystemWatcher watch = new FileSystemWatcher(path);
                watch.EnableRaisingEvents = true;
                watch.IncludeSubdirectories = true;
                watch.Deleted += watch_Deleted;
                watch.Created += watch_Created;
                watch.Renamed += watch_Renamed;
                _Watchers.Add(watch);
            }
        }

        private void DirectoryBrowser_Load(object sender, EventArgs e)
        {
            Browser.CheckBoxes = _CheckBoxes;
            SetImageList();
            if (TreeviewMode == TreeMode.ManualDirectory)
            {
                Browser.Nodes.Clear();
            }
        }

        private void ExpandToCurrentSelection()
        {
            Browser.BeginUpdate();

            string[] items = DirectoryBrowserUtils.SplitPath(_CurrentSelection);
            if (items.Length == 0)
            {
                return;
            }
            TreeNode _treeNode = selectRootNode(string.Format("{0}\\", items[0]));
            for (int ii = 1; ii < items.Length; ii++)
            {
                _treeNode = selectChildNode(items[ii], _treeNode);
            }
            Browser.SelectedNode = _treeNode;
            Browser.HideSelection = false;

            Browser.EndUpdate();
        }

        private void FillDirectory(TreeNodeCollection collection, string pPath, bool pIncludeFiles)
        {
            if (TreeviewMode == TreeMode.ManualDirectory)
            {
                return;
            }
            Browser.BeginUpdate();
            try
            {
                IEnumerable<PathItem> directories = GetDirectories(pPath);

                foreach (PathItem directory in directories)
                {
                    DirectoryInfo _dirInfo = null;
                    TreeNode node = null;
                    string path = null;
                    switch (directory.Type)
                    {
                        case PathItemType.VirtualDirectory:
                            _dirInfo = new DirectoryInfo(directory.WorkingPath);
                            node = AddNode(collection, NodeType.Folder, false, directory, _dirInfo.Name);
                            path = raiseEventBrowserIconRequest(directory.WorkingPath);
                            node.ImageIndex = GetImageIndex(path, IconSize.Small);
                            node.SelectedImageIndex = node.ImageIndex;
                            break;

                        case PathItemType.RealDirectory:
                            _dirInfo = new DirectoryInfo(directory.WorkingPath);
                            node = AddNode(collection, NodeType.Folder, false, PathItem.RealDirectory(directory.WorkingPath), _dirInfo.Name);
                            path = raiseEventBrowserIconRequest(directory.WorkingPath);
                            node.ImageIndex = GetImageIndex(path, IconSize.Small);
                            node.SelectedImageIndex = node.ImageIndex;
                            break;

                        case PathItemType.LinkedDirectory:
                            _dirInfo = new DirectoryInfo(directory.WorkingPath);
                            node = AddNode(collection, NodeType.Folder, false, directory, _dirInfo.Name);
                            path = raiseEventBrowserIconRequest(directory.WorkingPath);
                            node.ImageIndex = GetImageIndex(path, IconSize.Small);
                            node.SelectedImageIndex = node.ImageIndex;
                            break;
                    }
                }
                if (pIncludeFiles)
                {
                    FillDirectoryWithFiles(collection, pPath);
                }
            }
            catch
            {
            }
            Browser.EndUpdate();
        }

        private void FillDirectoryWithFiles(TreeNodeCollection Nodes, string pPath)
        {
            if (TreeviewMode == TreeMode.ManualDirectory)
            {
                return;
            }
            IEnumerable<PathItem> filelist = GetFiles(pPath);
            IEnumerable<PathItem> sortedList = filelist;

            switch (FileSorting)
            {
                case FileSort.NoSorting:
                    sortedList = filelist;
                    break;

                case FileSort.FileDateCreation_Asc:
                    sortedList = (from n in filelist orderby n.GetFileInfo().CreationTime ascending select n);
                    break;

                case FileSort.FileDateCreation_Desc:
                    sortedList = (from n in filelist orderby n.GetFileInfo().CreationTime descending select n);
                    break;

                case FileSort.FileDateLastEdit_Asc:
                    sortedList = (from n in filelist orderby n.GetFileInfo().LastWriteTime ascending select n);
                    break;

                case FileSort.FileDateLastEdit_Desc:
                    sortedList = (from n in filelist orderby n.GetFileInfo().LastWriteTime descending select n);
                    break;

                case FileSort.FileName_Asc:
                    sortedList = (from n in filelist orderby n.GetFileInfo().Name ascending select n);
                    break;

                case FileSort.FileName_Desc:
                    sortedList = (from n in filelist orderby n.GetFileInfo().Name descending select n);
                    break;
            }

            foreach (PathItem file in sortedList)
            {
                string tooltipName = string.Empty;
                string displayName = raiseBrowserReDisplayFileName(file.GetFileInfo().FullName, file.GetFileInfo().Name);
                if (!displayName.Equals(file.GetFileInfo().Name))
                {
                    tooltipName = file.GetFileInfo().Name;
                }

                TreeNode node = Nodes.Add(file.GetFileInfo().FullName, displayName);

                NodeTag.SetNodeType(node, NodeType.File);
                node.ToolTipText = tooltipName;
                node.ForeColor = Color.Yellow;
                string path = raiseEventBrowserIconRequest(file.GetFileInfo().FullName);
                node.ImageIndex = GetImageIndex(path, IconSize.Small);

                node.SelectedImageIndex = node.ImageIndex;
            }
        }

        private void FillRootDirectory(bool pIncludeFiles)
        {
            if (TreeviewMode == TreeMode.ManualDirectory)
            {
                return;
            }

            Browser.Nodes.Clear();

            if (!string.IsNullOrEmpty(_RootDirectory))
            {
                FillDirectory(Browser.Nodes, _RootDirectory, pIncludeFiles);
                return;
            }
            if (TreeviewMode == TreeMode.ManualDirectory)
            {
                return;
            }

            showSpecialFolder(Environment.SpecialFolder.MyPictures, ShowMyPictures);
            showSpecialFolder(Environment.SpecialFolder.MyVideos, ShowMyVideos);
            showSpecialFolder(Environment.SpecialFolder.StartMenu, ShowStartMenu);

            showSpecialFolder(Environment.SpecialFolder.Desktop, ShowDesktop);
            showSpecialFolder(Environment.SpecialFolder.MyDocuments, ShowMyDocuments);
            showSpecialFolder(Environment.SpecialFolder.Favorites, ShowFavorites);

            foreach (DriveInfo drv in DriveInfo.GetDrives())
            {
                if (raiseFilterPath(drv.RootDirectory.FullName))
                {
                    addDriveNode(drv, false);
                }
            }
        }

        private void FillSelectedItems(TreeNode Node, ref List<string> CurrentSelected)
        {
            foreach (TreeNode tn in Node.Nodes)
            {
                if (tn.Checked)
                {
                    CurrentSelected.Add(tn.Name);
                }
                FillSelectedItems(tn, ref CurrentSelected);
            }
        }

        private IEnumerable<PathItem> GetDirectories(string path)
        {
            if (Directory.Exists(path))
            {
                foreach (string directory in Directory.EnumerateDirectories(path))
                {
                    Reading?.Invoke(this, new BrowserReadingEventArgs() { Path = directory });
                    yield return new PathItem() { Path = directory };
                }
            }

            foreach (PathItem directory in EnumerateCustomPaths(path).Where(p => p.Type == PathItemType.VirtualDirectory || p.Type == PathItemType.RealDirectory || p.Type == PathItemType.LinkedDirectory))
            {
                Reading?.Invoke(this, new BrowserReadingEventArgs() { Path = directory.Path });
                yield return directory;
            }
        }

        private IEnumerable<PathItem> GetFiles(string path)
        {
            if (Directory.Exists(path))
            {
                IEnumerable<string> files = Directory.EnumerateFiles(path);
                foreach (string file in files)
                {
                    string sName = Path.GetFileName(file);
                    if (raiseFilterPath(file))
                    {
                        if (DirectoryBrowserUtils.MatchPattern(sName, FileFilter))
                        {
                            Reading?.Invoke(this, new BrowserReadingEventArgs() { Path = file });
                            yield return PathItem.RealFile(file);
                        }
                    }
                }
            }

            foreach (PathItem file in EnumerateCustomPaths(path).Where(p => p.Type == PathItemType.LinkedFile || p.Type == PathItemType.RealFile))
            {
                Reading?.Invoke(this, new BrowserReadingEventArgs() { Path = file.Path });
                yield return file;
            }
        }

        private int GetImageIndex(string file, IconSize size)
        {
            SHFILEINFO shfi = new SHFILEINFO();
            SHGetFileInfoConstants flags =
            SHGetFileInfoConstants.SHGFI_SYSICONINDEX;

            /* Check the size specified for return. */
            if (size == IconSize.Small)
            {
                flags |= SHGetFileInfoConstants.SHGFI_SMALLICON;
            }
            else
            {
                flags |= SHGetFileInfoConstants.SHGFI_LARGEICON;
            }

            SHGetFileInfo(file,
            FILE_ATTRIBUTE_NORMAL,
            ref shfi,
            (uint)System.Runtime.InteropServices.Marshal.SizeOf(shfi),
            (uint)flags);

            return shfi.iIcon;
        }

        private IntPtr GetSystemImageListHandle(IconSize size)
        {
            SHGetFileInfoConstants dwFlags = SHGetFileInfoConstants.SHGFI_SYSICONINDEX;
            if (size == IconSize.Small)
            {
                dwFlags |= SHGetFileInfoConstants.SHGFI_SMALLICON;
            }
            else
            {
                dwFlags |= SHGetFileInfoConstants.SHGFI_LARGEICON;
            }

            // Get image list
            SHFILEINFO shfi = new SHFILEINFO();
            uint shfiSize = (uint)Marshal.SizeOf(shfi.GetType());

            // Call SHGetFileInfo to get the image list handle
            // using an arbitrary file:
            return SHGetFileInfo(
            "C:\\",
            FILE_ATTRIBUTE_NORMAL,
            ref shfi,
            shfiSize,
            (uint)dwFlags);
        }

        private bool nodeNotYetExpanded(TreeNode node)
        {
            if (node.Nodes.Count == 1)
            {
                if (node.Nodes[0].Name == NEEDEXPANDING)
                {
                    if (NodeTag.GetNodeType(node) == NodeType.Folder)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private string raiseBrowserReDisplayDirectoryName(string path, string currentDisplayName)
        {
            if (ReDisplayDirectoryName == null)
                return currentDisplayName;

            BrowserReDisplayNameEventArgs ea = new BrowserReDisplayNameEventArgs();
            ea.DisplayName = currentDisplayName;
            ea.Path = path;
            ReDisplayDirectoryName(this, ea);
            return ea.DisplayName;
        }

        private string raiseBrowserReDisplayFileName(string path, string currentDisplayName)
        {
            if (ReDisplayFileName == null)
                return currentDisplayName;

            BrowserReDisplayNameEventArgs ea = new BrowserReDisplayNameEventArgs();
            ea.DisplayName = currentDisplayName;
            ea.Path = path;
            ReDisplayFileName(this, ea);
            return ea.DisplayName;
        }

        private string raiseEventBrowserIconRequest(string path)
        {
            if (IconRequest == null)
                return path;

            BrowserIconRequestEventArgs ea = new BrowserIconRequestEventArgs();
            ea.Path = path;
            IconRequest(this, ea);
            return ea.Path;
        }

        private bool raiseFilterPath(string path)
        {
            if (FilterPathRequest == null)
                return true;

            BrowserFilterPathEventArgs ea = new BrowserFilterPathEventArgs();
            ea.Path = path;
            FilterPathRequest(this, ea);
            return ea.Show;
        }

        private Color raiseFolderHighLight(string path)
        {
            if (HighlightFolderRequest == null)
                return Color.Orange;

            BrowserHighlightFolderRequestEventArgs ea = new BrowserHighlightFolderRequestEventArgs();
            ea.Path = path;
            HighlightFolderRequest(this, ea);
            if (ea.Highlight)
            {
                return Color.Orange;
            }
            return Color.Orange;
        }

        private TreeNode selectChildNode(string path, TreeNode parentNode)
        {
            if (parentNode == null)
            {
                return null;
            }
            foreach (TreeNode tn in parentNode.Nodes)
            {
                if (tn.Name.Equals(path, StringComparison.OrdinalIgnoreCase))
                {
                    if (tn.Nodes.Count == 1)
                    {
                        if (tn.Nodes[0].Name == NEEDEXPANDING)
                        {
                            tn.Nodes.Clear();
                            FillDirectory(tn.Nodes, tn.Name, _IncludeFiles);
                        }
                    }
                    tn.Expand();
                    return tn;
                }
            }
            return null;
        }

        private TreeNode selectRootNode(string path)
        {
            foreach (TreeNode tn in Browser.Nodes)
            {
                if (tn.Name.Equals(path, StringComparison.OrdinalIgnoreCase))
                {
                    if (tn.Nodes.Count == 1)
                    {
                        if (tn.Nodes[0].Name == NEEDEXPANDING)
                        {
                            tn.Nodes.Clear();
                            FillDirectory(tn.Nodes, tn.Name, _IncludeFiles);
                        }
                    }
                    tn.Expand();
                    return tn;
                }
            }
            return null;
        }

        private void SetImageList()
        {
            UInt64 imagelistHandle = (UInt64)GetSystemImageListHandle(IconSize.Small);
            try
            {
                SendMessage(
                Browser.Handle,
                TVM_SETIMAGELIST,
                TVSIL_NORMAL,
                imagelistHandle);
            }
            catch (Exception ex)
            {
            }
        }

        private void showContextMenu(string path)
        {
            ShellContextMenu scm = new ShellContextMenu();
            FileInfo[] files = new FileInfo[1];
            files[0] = new FileInfo(path);
            scm.ShowContextMenu(Handle, files, Cursor.Position);
        }

        private void showSpecialFolder(Environment.SpecialFolder specialFolder, bool show)
        {
            if (show)
            {
                string _folder = Environment.GetFolderPath(specialFolder);
                AddNode(Browser.Nodes, NodeType.Folder, false, PathItem.RealDirectory(_folder), Path.GetFileName(_folder));
                createWatcher(_folder);
            }
        }

        private void watch_Created(object sender, FileSystemEventArgs e)
        {
            if (InvokeRequired)
            {
                // Invoke this method on UI Thread
                var _self = new Action<object, FileSystemEventArgs>(watch_Created);
                BeginInvoke(_self, new[] { sender, e });
                return;
            }

            lock (this)
            {
                TreeNode[] parentNodes = new TreeNode[0];
                switch (DirectoryBrowserUtils.IsFileOrDirectory(e.FullPath))
                {
                    case DirectoryBrowserUtils.FileOrDirectory.Directory:

                        // A directory has been added
                        var _dir = new DirectoryInfo(e.FullPath);

                        // Find the nodes for the parent. (can be multiple special folders for example).
                        parentNodes = Browser.Nodes.Find(_dir.Parent.FullName, true);
                        foreach (TreeNode parent in parentNodes)
                        {
                            // Nodes only contains subnodes after the first node expansion
                            if (nodeNotYetExpanded(parent) == false)
                            {
                                // The parent exists in the treeviewer, add the child.
                                AddNode(parent.Nodes, NodeType.Folder, false, PathItem.RealDirectory(e.FullPath), _dir.Name);
                            }
                        }
                        break;

                    case DirectoryBrowserUtils.FileOrDirectory.File:

                        // A File Has been added.
                        if (!_IncludeFiles)
                        {
                            // We do not show files in the Browser.
                            return;
                        }
                        var _file = new FileInfo(e.FullPath);

                        // Find the nodes for the parent. (can be multiple special folders for example).
                        parentNodes = Browser.Nodes.Find(_file.Directory.FullName, true);
                        foreach (TreeNode parent in parentNodes)
                        {
                            // Nodes only contains subnodes after the first node expansion
                            if (nodeNotYetExpanded(parent) == false)
                            {
                                // The parent exists in the treeviewer, add the child, but only if the file complies to the file filter.
                                if (raiseFilterPath(_file.FullName))
                                {
                                    if (DirectoryBrowserUtils.MatchPattern(_file.Name, FileFilter))
                                    {
                                        AddNode(parent.Nodes, NodeType.File, false, PathItem.RealFile(e.FullPath), _file.Name);
                                    }
                                }
                            }
                        }
                        break;
                }
            }
        }

        private void watch_Deleted(object sender, FileSystemEventArgs e)
        {
            if (InvokeRequired)
            {
                // Invoke this method on UI Thread
                var _self = new Action<object, FileSystemEventArgs>(watch_Deleted);
                BeginInvoke(_self, new[] { sender, e });
                return;
            }

            lock (this)
            {
                TreeNode[] _nodes = Browser.Nodes.Find(e.FullPath, true);
                for (int ii = 0; ii < _nodes.Length; ii++)
                {
                    Browser.Nodes.Remove(_nodes[ii]);
                }
            }
        }

        /// <summary>
        /// A file has been renamed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void watch_Renamed(object sender, RenamedEventArgs e)
        {
            if (e.ChangeType == WatcherChangeTypes.Renamed)
            {
                if (InvokeRequired)
                {
                    // Invoke this method on UI Thread
                    var _self = new Action<object, RenamedEventArgs>(watch_Renamed);
                    BeginInvoke(_self, new[] { sender, e });
                    return;
                }
                TreeNode[] _nodes = Browser.Nodes.Find(e.OldFullPath, true); // (Node which has been expanded).
                foreach (TreeNode node in _nodes)
                {
                    node.Name = e.FullPath;
                    switch (DirectoryBrowserUtils.IsFileOrDirectory(e.FullPath))
                    {
                        case DirectoryBrowserUtils.FileOrDirectory.File:
                            node.Text = Path.GetFileName(e.FullPath);
                            break;

                        case DirectoryBrowserUtils.FileOrDirectory.Directory:
                            node.Text = Path.GetDirectoryName(e.FullPath);
                            break;
                    }
                }
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct SHFILEINFO
        {
            public IntPtr hIcon;
            public int iIcon;
            public int dwAttributes;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MAX_PATH)]
            public string szDisplayName;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
            public string szTypeName;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 8, CharSet = CharSet.Auto)]
        private struct TVITEM
        {
            public uint mask;
            public IntPtr hItem;
            public uint state;
            public uint stateMask;
            public IntPtr pszText;
            public int cchTextMax;
            public int iImage;
            public int iSelectedImage;
            public int cChildren;
            public IntPtr lParam;
        }
    }
}