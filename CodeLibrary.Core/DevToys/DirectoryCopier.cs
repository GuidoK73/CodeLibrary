using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace DevToys
{
    public class DirectoryCopier
    {
        #region Kernel

        [Flags]
        private enum CopyFileFlags : uint
        {
            COPY_FILE_FAIL_IF_EXISTS = 0x00000001,
            COPY_FILE_RESTARTABLE = 0x00000002,
            COPY_FILE_OPEN_SOURCE_FOR_WRITE = 0x00000004,
            COPY_FILE_ALLOW_DECRYPTED_DESTINATION = 0x00000008
        }

        private enum CopyProgressCallbackReason : uint
        {
            CALLBACK_CHUNK_FINISHED = 0x00000000,
            CALLBACK_STREAM_SWITCH = 0x00000001
        }

        private enum CopyProgressResult : uint
        {
            PROGRESS_CONTINUE = 0,
            PROGRESS_CANCEL = 1,
            PROGRESS_STOP = 2,
            PROGRESS_QUIET = 3
        }

        [Flags]
        private enum MoveFileFlags : uint
        {
            MOVE_FILE_REPLACE_EXISTSING = 0x00000001,
            MOVE_FILE_COPY_ALLOWED = 0x00000002,
            MOVE_FILE_DELAY_UNTIL_REBOOT = 0x00000004,
            MOVE_FILE_WRITE_THROUGH = 0x00000008,
            MOVE_FILE_CREATE_HARDLINK = 0x00000010,
            MOVE_FILE_FAIL_IF_NOT_TRACKABLE = 0x00000020
        }

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool CopyFileEx(string lpExistingFileName, string lpNewFileName, CopyProgressRoutine lpProgressRoutine, IntPtr lpData, ref Int32 pbCancel, CopyFileFlags dwCopyFlags);

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern bool MoveFileWithProgress(string lpExistingFileName, string lpNewFileName, CopyProgressRoutine lpProgressRoutine, IntPtr lpData, MoveFileFlags dwCopyFlags);

        #endregion Kernel

        #region Nested classes

        public sealed class DirectoryCopyEventArgs
        {
            public DirectoryCopyEventArgs() { }

            internal DirectoryCopyEventArgs(long size, long progress, string source, string target)
            {
                Size = size;
                Progress = progress;
                SourceFile = source;
                TargetFile = target;
            }

            public long Percentage => Math.Min(Utils.PercentageScale(Size, Progress), 100);

            public long Progress { get; internal set; }

            public long Size { get; internal set; }

            public string SourceFile { get; internal set; }

            public string TargetFile { get; internal set; }
        }

        public sealed class CopyDirectoryOptions
        {
            public CopyDirectoryOptions() { }

            public enum OverwriteMode { Always = 0, OnlyNewer = 1, Never = 2 }

            /// <summary>Cleanup the target directory.</summary>
            public bool CleanupTarget { get; set; } = true;

            /// <summary>Only copy source directory's structure.</summary>
            public bool CopyDirectoryStructureOnly { get; set; }

            /// <summary>Do not create the source directory in target.</summary>
            public bool CopyOnlySourceContents { get; set; } = true;

            /// <summary>Array of relative directories to create.</summary>
            public string[] CreateDirectories { get; set; }

            /// <summary>Do not copy empty directories.</summary>
            public bool ExcludeEmptyDirectories { get; set; } = false;

            /// <summary>Excludes file names with pattern</summary>
            public string ExcludeFilter { get; set; }

            /// <summary>Excludes directory names with pattern</summary>
            public string ExcludePathFilter { get; set; }

            /// <summary>Do not copy read only files.</summary>
            public bool ExcludeReadOnly { get; set; } = false;

            /// <summary>Do not copy empty directories, even when those directories contains directories which are empty (do not contain files).</summary>
            public bool ExludeEmptyDirectoriesRecursive { get; set; } = false;

            /// <summary>All files in source directory (in all subdirectories) will be copied to the target root directory.</summary>
            public bool FlatCopy { get; set; } = false;

            /// <summary>Include files with pattern.</summary>
            public string IncludeFilter { get; set; } = "*";

            /// <summary>Include files with regex, is tested before include filter.</summary>
            public string IncludeRegex { get; set; } = null;

            /// <summary>Include directories with pattern.</summary>
            public string IncludePathFilter { get; set; } = "*";

            /// <summary>Include sub directories in copy action.</summary>
            public bool IncludeSubDirectories { get; set; } = true;

            /// <summary>Exclude all files above a file size maximum</summary>
            public long MaximumFileSize { get; set; } = 0;

            /// <summary>Exclude all files below a file size minimum</summary>
            public long MinimumFileSize { get; set; } = 0;

            /// <summary>Check for modified Since Date</summary>
            public DateTime? ModifiedSinceDate { get; set; } = null;

            /// <summary>Check for modified Till Date</summary>
            public DateTime? ModifiedTillDate { get; set; } = null;

            public string Name { get; set; } = "New Copy";

            public OverwriteMode Overwrite { get; set; } = OverwriteMode.Always;

            /// <summary>Source Directory</summary>
            public string Source { get; set; }

            public DirectoryInfo SourceInfo => new DirectoryInfo(Source.Replace("$Name", Name));

            /// <summary>Target Directory</summary>
            public string Target { get; set; }

            public DirectoryInfo TargetInfo
            {
                get
                {
                    string target = Target.Replace("$Name", this.Name);

                    DirectoryInfo result = new DirectoryInfo(target);

                    if (!CopyOnlySourceContents)
                        result = new DirectoryInfo(Path.Combine(target.TrimEnd(new char[] { '\\' }), SourceInfo.Name.TrimStart(new char[] { '\\' })));

                    return result;
                }
            }

            public long CalcDirectorySize()
            {
                long total = 0;
                return CalcDirectorySize(SourceInfo, ref total);
            }

            public CopyDirectoryOptions Clone()
            {
                CopyDirectoryOptions newitem = new CopyDirectoryOptions
                {
                    Name = Name,
                    IncludeFilter = IncludeFilter,
                    IncludeRegex = IncludeRegex,
                    IncludePathFilter = IncludePathFilter,
                    ExcludeFilter = ExcludeFilter,
                    ExcludePathFilter = ExcludePathFilter,
                    ExcludeReadOnly = ExcludeReadOnly,
                    CleanupTarget = CleanupTarget,
                    IncludeSubDirectories = IncludeSubDirectories,
                    CopyOnlySourceContents = CopyOnlySourceContents,
                    ExcludeEmptyDirectories = ExcludeEmptyDirectories,
                    MinimumFileSize = MinimumFileSize,
                    MaximumFileSize = MaximumFileSize,
                    ModifiedSinceDate = ModifiedSinceDate,
                    ModifiedTillDate = ModifiedTillDate,
                    Source = Source,
                    Target = Target
                };

                if (CreateDirectories != null)
                {
                    newitem.CreateDirectories = new string[CreateDirectories.Length];
                    for (int ii = 0; ii < CreateDirectories.Length; ii++)
                        newitem.CreateDirectories[ii] = CreateDirectories[ii];
                }
                return newitem;
            }

            public FileInfo TargetFile(FileInfo sourceFile)
            {
                string relativefile = sourceFile.FullName.Replace(SourceInfo.FullName, string.Empty);
                string targetfile = Path.Combine(TargetInfo.FullName.TrimEnd(new char[] { '\\' }), relativefile.TrimStart(new char[] { '\\' }));
                if (FlatCopy)
                    targetfile = Path.Combine(TargetInfo.FullName.TrimEnd(new char[] { '\\' }), sourceFile.Name);

                return new FileInfo(targetfile);
            }

            public override string ToString() => Name;

            internal bool Exclude(FileInfo sourcefile)
            {
                string test = ExcludeFilter.Replace("\r", string.Empty);
                test = test.Replace("\n", string.Empty);
                bool exludepattern = MatchPattern(sourcefile.Name, test);
                bool excludereadonly = (ExcludeReadOnly && sourcefile.IsReadOnly);
                bool excludemax = (sourcefile.Length > MaximumFileSize && MaximumFileSize > 0);
                bool excludemin = (sourcefile.Length < MinimumFileSize && MinimumFileSize > 0);
                bool exclude = exludepattern || excludereadonly || excludemax || excludemin;
                return exclude;
            }

            internal bool Exclude(DirectoryInfo directory)
            {
                bool excludeEmptyDir = false;
                if (ExcludeEmptyDirectories)
                {
                    if (ExludeEmptyDirectoriesRecursive)
                        excludeEmptyDir = DirectoryContainsFilesRecursive(directory) == false;
                    else
                        excludeEmptyDir = DirectoryIsEmpty(directory);
                }
                string test = ExcludePathFilter.Replace("\r", string.Empty).Replace("\n", string.Empty);
                return MatchPattern(directory.Name, test) || excludeEmptyDir;
            }

            internal bool Include(FileInfo sourcefile)
            {
                if (!string.IsNullOrEmpty(IncludeRegex))
                {
                    Regex _regExInclude = new Regex(IncludeRegex);
                    bool _isMatch = _regExInclude.IsMatch(sourcefile.Name);
                    if (!_isMatch)
                        return false;
                }

                string test = IncludeFilter.Replace("\r", string.Empty);
                test = test.Replace("\n", string.Empty);
                bool include = MatchPattern(sourcefile.Name, test);

                FileInfo targetfile = TargetFile(sourcefile);
                bool overwrite = true;

                if (targetfile.Exists && Overwrite == OverwriteMode.Always)
                    overwrite = true;

                if (targetfile.Exists && Overwrite == OverwriteMode.Never)
                    overwrite = false;

                if (targetfile.Exists && Overwrite == OverwriteMode.OnlyNewer)
                    overwrite = targetfile.LastWriteTimeUtc < sourcefile.LastWriteTimeUtc;

                if (ModifiedSinceDate.HasValue && sourcefile.LastWriteTime < ModifiedSinceDate.Value)
                    include = false;

                if (ModifiedTillDate.HasValue && sourcefile.LastWriteTime > ModifiedTillDate.Value)
                    include = false;

                return include && overwrite;
            }

            internal bool Include(DirectoryInfo directory) => MatchPattern(directory.Name, IncludePathFilter.Replace("\r", string.Empty).Replace("\n", string.Empty));

            private long CalcDirectorySize(DirectoryInfo source, ref long size)
            {
                foreach (DirectoryInfo directory in source.GetDirectories())
                {
                    if (!Exclude(directory))
                        CalcDirectorySize(directory, ref size);
                }

                FileInfo[] files = source.GetFiles();
                foreach (FileInfo file in files)
                {
                    if (!Exclude(file) && Include(file))
                        size += file.Length;
                }
                return size;
            }

            private bool DirectoryContainsFilesRecursive(DirectoryInfo sourceDir)
            {
                bool result = false;

                if (!sourceDir.Exists)
                    return false;

                FileInfo[] _files;
                try
                {
                    _files = sourceDir.GetFiles();
                }
                catch
                {
                    return false;
                }

                if (IncludeSubDirectories)
                {
                    foreach (DirectoryInfo directory in sourceDir.GetDirectories())
                    {
                        if (!Exclude(directory))
                            result = DirectoryContainsFilesRecursive(directory);

                        if (result)
                            return true;
                    }
                }

                foreach (FileInfo file in _files)
                {
                    if (!Exclude(file) && Include(file))
                        return true;
                }
                return result;
            }

            private bool DirectoryIsEmpty(DirectoryInfo directory) => directory.GetFiles().Length == 0 && directory.GetDirectories().Length == 0;

            private bool MatchPattern(string s, string pattern)
            {
                if (pattern == null || s == null)
                    return false;

                string[] patterns = pattern.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string subpattern in patterns)
                {
                    string pat = string.Format("^{0}$", Regex.Escape(subpattern).Replace("\\*", ".*").Replace("\\?", "."));
                    Regex regex = new Regex(pat, RegexOptions.IgnoreCase);
                    if (regex.IsMatch(s))
                        return true;
                }
                return false;
            }
        }

        private static class Utils
        {
            public static long PercentageScale(double total, double part)
            {
                if (total == 0)
                    return 0;

                double _perc = part / total * 100;
                return (long)_perc;
            }

            public static bool DirectoryContainsSubDirectories(string path)
            {
                if (Directory.Exists(path))
                    return DirectoryContainsSubDirectories(new DirectoryInfo(path));

                return false;
            }
            public static bool DirectoryContainsSubDirectories(DirectoryInfo directory) => (directory.GetDirectories("*.*").Length > 0);

            public static bool DirectoryContainsFiles(DirectoryInfo directory, string filter)
            {
                if (!directory.Exists)
                    return false;

                FileInfo[] files;
                try
                {
                    files = directory.GetFiles();
                }
                catch
                {
                    return false;
                }
                if (string.IsNullOrEmpty(filter))
                    return (files.Length > 0);

                foreach (FileInfo file in files)
                {
                    if (MatchPattern(file.Name, filter))
                        return true;
                }
                return false;
            }

            public static bool MatchPattern(string s, string pattern)
            {
                if (pattern == null || s == null)
                    return false;

                string[] patterns = pattern.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string subpattern in patterns)
                {
                    string pat = string.Format("^{0}$", Regex.Escape(subpattern).Replace("\\*", ".*").Replace("\\?", "."));
                    Regex regex = new Regex(pat, RegexOptions.IgnoreCase);
                    if (regex.IsMatch(s))
                        return true;
                }
                return false;
            }

            public static bool DirectoryContainsFiles(string directory, string filter)
            {
                if (string.IsNullOrEmpty(directory))
                    return false;

                return DirectoryContainsFiles(new DirectoryInfo(directory), filter);
            }

            public static void DeleteDirectory(DirectoryInfo directory)
            {
                if (!directory.Exists)
                    return;

                if (DirectoryContainsSubDirectories(directory))
                {
                    foreach (DirectoryInfo dir in directory.GetDirectories())
                        DeleteDirectory(dir);
                }
                if (DirectoryContainsFiles(directory, string.Empty))
                {
                    foreach (FileInfo fi in directory.GetFiles())
                    {
                        fi.IsReadOnly = false;
                        fi.Delete();
                    }
                }
                directory.Delete();
            }
        }

        #endregion

        private long allFilesSize = 0;
        private long allTransferred = 0;
        private string currentSource = string.Empty;
        private string currentTarget = string.Empty;
        private int pbCancel;
        private long prevtransferred = 0;
        private int srcFilePointer = 0;

        public DirectoryCopier() { }

        private delegate CopyProgressResult CopyProgressRoutine(
                long TotalFileSize,
                long TotalBytesTransferred,
                long StreamSize,
                long StreamBytesTransferred,
                uint dwStreamNumber,
                CopyProgressCallbackReason dwCallbackReason,
                IntPtr hSourceFile,
                IntPtr hDestinationFile,
                IntPtr lpData);


        public event EventHandler<DirectoryCopyEventArgs> CopyProgress = delegate { };

        public event EventHandler<DirectoryCopyEventArgs> Finished = delegate { };

        public event EventHandler<DirectoryCopyEventArgs> StartCopy = delegate { };

        public void XCopy(CopyDirectoryOptions options)
        {
            allTransferred = 0;
            allFilesSize = options.CalcDirectorySize();
            pbCancel = 0;
            prevtransferred = 0;
            srcFilePointer = 0;
            currentSource = string.Empty;
            currentTarget = string.Empty;

            StartCopy?.Invoke(this, new DirectoryCopyEventArgs(allFilesSize, allTransferred, currentSource, currentTarget));

            if (options.CleanupTarget)
                Utils.DeleteDirectory(options.TargetInfo);

            Copy(options, options.SourceInfo);

            if (options.CreateDirectories != null)
            {
                foreach (string newdir in options.CreateDirectories)
                {
                    string newPath = Path.Combine(options.TargetInfo.FullName, newdir.TrimStart(new char[] { '\\' }));
                    Directory.CreateDirectory(newPath);
                }
            }

            Finished?.Invoke(this, new DirectoryCopyEventArgs(allFilesSize, allTransferred, string.Empty, string.Empty));
        }

        private void Copy(CopyDirectoryOptions options, DirectoryInfo sourceDir)
        {
            string basepath = options.SourceInfo.FullName;
            if (options.IncludeSubDirectories)
            {
                foreach (DirectoryInfo directory in sourceDir.GetDirectories())
                {
                    if (!options.Exclude(directory) && options.Include(directory))
                    {
                        if (!options.FlatCopy)
                        {
                            string relativedir = directory.FullName.Replace(basepath, string.Empty);
                            string targetdir = Path.Combine(options.TargetInfo.FullName.TrimEnd(new char[] { '\\' }), relativedir.TrimStart(new char[] { '\\' }));
                            Directory.CreateDirectory(targetdir);
                        }
                        CopyFiles(options, directory);
                        Copy(options, directory);
                    }
                }
            }
            Directory.CreateDirectory(options.TargetInfo.FullName);
            CopyFiles(options, sourceDir);
        }

        private void CopyFiles(CopyDirectoryOptions options, DirectoryInfo sourceDir)
        {
            if (options.CopyDirectoryStructureOnly)
                return;

            FileInfo[] files = sourceDir.GetFiles();
            foreach (FileInfo sourcefile in files)
            {
                FileInfo targetfile = options.TargetFile(sourcefile);
                if (!options.Exclude(sourcefile) && options.Include(sourcefile))
                {
                    if (targetfile.Exists)
                        File.Delete(targetfile.FullName);

                    CopyFileEx(sourcefile.FullName, targetfile.FullName, new CopyProgressRoutine(this.CopyProgressHandler), IntPtr.Zero, ref pbCancel, CopyFileFlags.COPY_FILE_RESTARTABLE);
                }
            }
        }

        private CopyProgressResult CopyProgressHandler(long total, long transferred, long streamSize, long StreamByteTrans, uint dwStreamNumber, CopyProgressCallbackReason reason, IntPtr hSourceFile, IntPtr hDestinationFile, IntPtr lpData)
        {
            if (hSourceFile.ToInt32() != srcFilePointer)
            {
                prevtransferred = 0;
                srcFilePointer = hSourceFile.ToInt32();
            }
            long newtransferred = transferred - prevtransferred;
            prevtransferred = transferred;

            if (CopyProgress != null)
            {
                allTransferred += newtransferred;
                CopyProgress(this, new DirectoryCopyEventArgs(allFilesSize, allTransferred, currentSource, currentTarget));
            }
            return CopyProgressResult.PROGRESS_CONTINUE;
        }
    }
}