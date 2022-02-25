using CodeLibrary.Core;
using CodeLibrary.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CodeLibrary
{
    public class BackupHelper
    {
        private string _patternDate = $"[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]_[0-9][0-9][0-9][0-9][0-9][0-9]";
        private Regex _regExDate;
        private FormCodeLibrary _mainForm;


        public BackupHelper(string currentFile) : this(currentFile, null)
        {
        }

        public BackupHelper(string currentFile, FormCodeLibrary mainForm)
        {
            if (string.IsNullOrEmpty(currentFile))
                throw new FileLoadException("currentfile not specified for backuphelper.");

            _mainForm = mainForm;

            CurrentFile = new FileInfo(currentFile);
            _regExDate = new Regex(_patternDate);
        }

        private FileInfo CurrentFile { get; set; }

        public void Backup()
        {
            
            if (!CurrentFile.Exists)
                return;

            string newName = $"{CurrentFile.Name.Replace($".{CurrentFile.Extension}", string.Empty)}_{DateTime.Now.ToString("yyyyMMdd_HHmmss")}.bak";

            FileInfo bakfile = new FileInfo(Path.Combine(CurrentFile.Directory.FullName, newName));

            if (!bakfile.Exists)
            {
                try
                {
                    File.Move(CurrentFile.FullName, bakfile.FullName);
                }
                catch (UnauthorizedAccessException)
                {
                    return;
                }
                catch (Exception)
                {
                    return;
                }
            }

            if (Directory.Exists(Config.BackupLocation))
            {
                // Move backup files.
                Task t = new Task(StartBackup);
                t.Start();
            }
        }

        private string GetBackupLocation()
        {
            if (Directory.Exists(Config.BackupLocation))
                return Config.BackupLocation;

            return CurrentFile.Directory.FullName;
        }

        public IEnumerable<BackupInfo> GetBackups()
        {
            string _pattern = $"^{CurrentFile.Name.Replace($".{CurrentFile.Extension}", string.Empty)}_[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]_[0-9][0-9][0-9][0-9][0-9][0-9].bak$";

            Regex _regEx = new Regex(_pattern);

            DirectoryInfo _directory = new DirectoryInfo(GetBackupLocation());
            
            foreach (FileInfo file in _directory.GetFiles())
            {
                if (_regEx.Match(file.Name).Success)
                {
                    DateTime _date = GeDateFromFileName(file.Name);

                    var _backupInfo = new BackupInfo()
                    {
                        Name = CurrentFile.Name,
                        FileName = file.Name,
                        Path = file.FullName,
                        DateTime = _date
                    };

                    yield return _backupInfo;
                }
            }
        }


        private void DeleteOlderbackupFiles(string directory, string name, int days)
        {
            FileInfo file = new FileInfo(Path.Combine(directory, name));

            if (!file.Directory.Exists)
                return;

            string pattern = $"{file.Name.Replace($".{file.Extension}", string.Empty)}_*.bak";

            DateTime filterDate = DateTime.Now.AddDays(-2);

            IEnumerable<FileInfo> files = file.Directory.GetFiles()
                .Where(p => Utils.MatchPattern(p.Name, pattern))
                .Where(p => p.LastAccessTime < filterDate);

            foreach (FileInfo fileInfo in files)
                fileInfo.Delete();
        }

        private void DeletebackupFiles(string directory, string name)
        {
            FileInfo file = new FileInfo(Path.Combine(directory, name));

            if (!file.Directory.Exists)
                return;

            string pattern = $"{file.Name.Replace($".{file.Extension}", string.Empty)}_*.bak";


            IEnumerable<FileInfo> files = file.Directory.GetFiles()
                .Where(p => Utils.MatchPattern(p.Name, pattern));

            foreach (FileInfo fileInfo in files)
                fileInfo.Delete();
        }


        private void backupFiles(string directory, string name)
        {
            FileInfo file = new FileInfo(Path.Combine(directory, name));

            if (!file.Directory.Exists)
                return;

            string pattern = $"{file.Name.Replace($".{file.Extension}", string.Empty)}_*.bak";


            IEnumerable<FileInfo> files = file.Directory.GetFiles()
                .Where(p => Utils.MatchPattern(p.Name, pattern));

            foreach (FileInfo fileInfo in files)
            {
                string _target = Path.Combine(GetBackupLocation(), fileInfo.Name);
                File.Copy(fileInfo.FullName, _target);
            }       
        }

        private void DeleteOlderbackupFilesKeepOnePerDay(string directory, string name, int days)
        {

            FileInfo file = new FileInfo(Path.Combine(directory, name));

            if (!file.Directory.Exists)
                return;

            string pattern = $"{file.Name.Replace($".{file.Extension}", string.Empty)}_*.bak";

            DateTime filterDate = DateTime.Now.AddDays(-2);

            IEnumerable<FileInfo> files = file.Directory.GetFiles()
                .Where(p => Utils.MatchPattern(p.Name, pattern))
                .Where(p => p.LastAccessTime < filterDate)
                .GroupBy(d => d.LastAccessTime.Date)
                .Select(g => g.OrderBy(o => o.LastAccessTime).Last());

            foreach (FileInfo fileInfo in files)
                fileInfo.Delete();
        }

        private DateTime GeDateFromFileName(string filename)
        {
            string _date = _regExDate.Match(filename).Value;

            int _y = Convert.ToInt32(_date.Substring(0, 4));
            int _m = Convert.ToInt32(_date.Substring(4, 2));
            int _d = Convert.ToInt32(_date.Substring(6, 2));
            int _hr = Convert.ToInt32(_date.Substring(9, 2));
            int _mn = Convert.ToInt32(_date.Substring(11, 2));
            int _sc = Convert.ToInt32(_date.Substring(13, 2));

            return new DateTime(_y, _m, _d, _hr, _mn, _sc);
        }

        private bool _started = false;

        public async void StartBackup()
        {
            if (_started)
                return;

            _started = true;
            // Config.BackupLocation

            _mainForm._stateIconHelper.SetBackupOn();

            await Task.Run(() => {
                try
                {
                    // Copy backups to target location
                    backupFiles(CurrentFile.Directory.FullName, CurrentFile.Name);
                    // Delete local files.
                    DeletebackupFiles(CurrentFile.Directory.FullName, CurrentFile.Name);
                    if (!CurrentFile.Directory.FullName.Equals(GetBackupLocation(), StringComparison.OrdinalIgnoreCase))
                    {
                        // Cleanup target location.
                        DeleteOlderbackupFiles(GetBackupLocation(), CurrentFile.Name, -21); // Delete everything.
                        DeleteOlderbackupFilesKeepOnePerDay(GetBackupLocation(), CurrentFile.Name, -2); // keep latest per day.
                    }
                }
                catch
                {

                }
            });

            _mainForm._stateIconHelper.SetBackupOff();


            _started = false;
        }
    }
}