﻿using DevToys;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace CodeLibrary.Core
{
    public static class Config
    {
        public static string AppFolder { get; set; }

        public static string BackupLocation { get; set; }

        public static EBackupMode BackupMode { get; set; }

        public static ETheme Theme { get; set; } = ETheme.Dark;

        public static ESortMode SortMode { get; set; } = ESortMode.Alphabetic;

        public static string DefaultNoteType { get; set; }
        public static string FavoriteFile => Utils.PathCombine(AppFolder, "Favorite.json");

        public static List<string> FavoriteLibs { get; set; } = new List<string>();

        public static string LastOpenedDir { get; set; }
        public static string LastOpenedFile { get; set; }
        public static bool OpenDefaultOnStart { get; set; }
        public static string PluginPath { get; set; }
        public static string RtfStylesFile => Utils.PathCombine(AppFolder, "RtfStyles.json");
        public static int Zoom { get; set; } = 100;

        public static int SplitterDistance { get; set; } = 380;

        public static VersionNumber CurrentVersion()
        {
            return new VersionNumber(Assembly.GetEntryAssembly().GetName().Version.ToString());
        }

        public static bool IsNewVersion()
        {
            VersionNumber _prevVersion = PreviousVersion();
            VersionNumber _currentVersion = CurrentVersion();

            return _currentVersion > _prevVersion;
        }

        public static void Load()
        {
            string regpath = Regpath();
            LastOpenedDir = Utils.GetCurrentUserRegisterKey(regpath, Constants.LASTOPENEDDIR);
            LastOpenedFile = Utils.GetCurrentUserRegisterKey(regpath, Constants.LASTOPENEDFILE);

            BackupLocation = Utils.GetCurrentUserRegisterKey(regpath, Constants.BACKUPLOCATION);

            OpenDefaultOnStart = ConvertUtility.ToBoolean(Utils.GetCurrentUserRegisterKey(regpath, Constants.OPENDEFAULTONSTART), false);
            Zoom = ConvertUtility.ToInt32(Utils.GetCurrentUserRegisterKey(regpath, Constants.ZOOM), 100);

            try
            {
                SortMode = (ESortMode)Enum.Parse(typeof(ESortMode), Utils.GetCurrentUserRegisterKey(regpath, Constants.SORTMODE));
            }
            catch { }

            try
            {
                Theme = (ETheme)Enum.Parse(typeof(ETheme), Utils.GetCurrentUserRegisterKey(regpath, Constants.THEME));
            }
            catch { }

            try
            {
                BackupMode = (EBackupMode)Enum.Parse(typeof(EBackupMode), Utils.GetCurrentUserRegisterKey(regpath, Constants.BACKUPMODE));
            }
            catch { }

            DefaultNoteType = Utils.GetCurrentUserRegisterKey(regpath, Constants.DEFAULT_NOTE_TYPE);

            PluginPath = Utils.GetCurrentUserRegisterKey(regpath, Constants.PLUGINPATH);

            AppFolder = Utils.PathCombine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Guido Utilities", "CodeLibrary");

            SplitterDistance = ConvertUtility.ToInt32(Utils.GetCurrentUserRegisterKey(regpath, Constants.SPLITTERDISTANCE), 380);

            StyleCollection.Instance.Load(RtfStylesFile);

            if (File.Exists(FavoriteFile))
            {
                string _json = File.ReadAllText(FavoriteFile);
                FavoriteLibs = Utils.FromJson<List<string>>(_json);
            }
        }

        public static VersionNumber PreviousVersion() => new VersionNumber(Utils.GetCurrentUserRegisterKey(Regpath(), Constants.VERSION));

        public static void Save()
        {
            string regpath = string.Format(Utils.REG_USRSETTING, Constants.CODELIBRARY);

            LastOpenedDir = string.IsNullOrEmpty(LastOpenedDir) ? string.Empty : LastOpenedDir;
            LastOpenedFile = string.IsNullOrEmpty(LastOpenedFile) ? string.Empty : LastOpenedFile;


            Utils.SetCurrentUserRegisterKey(regpath, Constants.THEME, Theme.ToString());
            Utils.SetCurrentUserRegisterKey(regpath, Constants.SORTMODE, SortMode.ToString());
            Utils.SetCurrentUserRegisterKey(regpath, Constants.BACKUPMODE, BackupMode.ToString());
            Utils.SetCurrentUserRegisterKey(regpath, Constants.LASTOPENEDDIR, LastOpenedDir);
            Utils.SetCurrentUserRegisterKey(regpath, Constants.LASTOPENEDFILE, LastOpenedFile);
            Utils.SetCurrentUserRegisterKey(regpath, Constants.BACKUPLOCATION, BackupLocation);
            Utils.SetCurrentUserRegisterKey(regpath, Constants.OPENDEFAULTONSTART, OpenDefaultOnStart.ToString());
            Utils.SetCurrentUserRegisterKey(regpath, Constants.ZOOM, Zoom.ToString());
            Utils.SetCurrentUserRegisterKey(regpath, Constants.SPLITTERDISTANCE, SplitterDistance.ToString());
             
            if (string.IsNullOrEmpty(DefaultNoteType))
            {
                DefaultNoteType = string.Empty;
            }
            Utils.SetCurrentUserRegisterKey(regpath, Constants.DEFAULT_NOTE_TYPE, DefaultNoteType);
            Utils.SetCurrentUserRegisterKey(regpath, Constants.PLUGINPATH, PluginPath);

            if (!Directory.Exists(AppFolder))
                Directory.CreateDirectory(AppFolder);

            if (File.Exists(FavoriteFile))
                File.Delete(FavoriteFile);

            string _json = Utils.ToJson<List<string>>(FavoriteLibs);

            File.WriteAllText(FavoriteFile, _json);

            StyleCollection.Instance.Save(RtfStylesFile);

            try
            {
                VersionNumber _version = new VersionNumber(Assembly.GetEntryAssembly().GetName().Version.ToString());
                string _versionString = _version.ToString();
                if (string.IsNullOrEmpty(_versionString))
                    _versionString = string.Empty;
                Utils.SetCurrentUserRegisterKey(regpath, Constants.VERSION, _versionString);
            }
            catch { }
        }

        private static string Regpath() => string.Format(Utils.REG_USRSETTING, Constants.CODELIBRARY);
    }
}