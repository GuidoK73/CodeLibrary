using System;
using System.IO;
using System.Text.RegularExpressions;

namespace CodeLibrary.Controls.DirectoryBrowser
{
    internal class DirectoryBrowserUtils
    {
        public enum FileOrDirectory
        {
            /// <summary>
            /// Is path a file
            /// </summary>
            File = 0,

            /// <summary>
            /// is path a directory
            /// </summary>
            Directory = 1,

            /// <summary>
            /// does path exists
            /// </summary>
            DoesNotExist = 2
        }

        public static bool HasDirectoryAccess(string path)
        {
            return true;
            DirectoryInfo di = new DirectoryInfo(path);
            if (!IsDriveReady(di))
                return false;

            try
            {
                FileInfo[] files = di.GetFiles("*.x");
            }
            catch
            {
                return false;
            }

            return true;
        }

        public static bool IsDriveReady(DirectoryInfo directory)
        {
            if (directory.Root.FullName.StartsWith("\\\\"))
                return true; // it's a network share

            DriveInfo di = new DriveInfo(directory.Root.Name);
            return di.IsReady;
        }

        public static FileOrDirectory IsFileOrDirectory(string path)
        {
            if (string.IsNullOrEmpty(path))
                return FileOrDirectory.DoesNotExist;

            if (File.Exists(path))
                return FileOrDirectory.File;
            else if (Directory.Exists(path))
                return FileOrDirectory.Directory;

            return FileOrDirectory.DoesNotExist;
        }

        public static bool MatchPattern(string s, string pattern)
        {
            if (pattern == null)
            {
                return false;
            }
            if (s == null)
            {
                return false;
            }
            string[] patterns = pattern.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string subpattern in patterns)
            {
                string pat = string.Format("^{0}$", Regex.Escape(subpattern).Replace("\\*", ".*").Replace("\\?", "."));
                Regex regex = new Regex(pat, RegexOptions.IgnoreCase);
                if (regex.IsMatch(s))
                {
                    return true;
                }
            }
            return false;
        }

        public static string[] SplitPath(string path)
        {
            return SplitPath(path, Path.DirectorySeparatorChar);
        }

        public static string[] SplitPath(string path, char separator)
        {
            if (string.IsNullOrEmpty(path))
            {
                return new string[0];
            }
            string[] items = path.Split(new char[] { separator }, StringSplitOptions.RemoveEmptyEntries);
            if (items.Length == 0)
            {
                return new string[0];
            }
            if (items.Length == 1)
            {
                return items;
            }
            string[] items2 = new string[items.Length];
            items2[0] = items[0];
            for (int ii = 1; ii < items.Length; ii++)
            {
                items2[ii] = string.Format("{0}{1}{2}", items2[ii - 1], separator, items[ii]);
            }
            for (int ii = 0; ii < items.Length; ii++)
            {
                items[ii] = items2[(items.Length - 1) - ii];
            }
            return items2;
        }
    }
}