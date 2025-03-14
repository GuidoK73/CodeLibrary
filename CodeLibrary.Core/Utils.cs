﻿using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Reflection;
using System.Runtime.Serialization.Json;
using System.Security.AccessControl;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace CodeLibrary.Core
{
    public static class Utils
    {
        public const char _DOUBLEQUOTE = '"';
        public const char _SINGLEQUOTE = '\'';

        public const string REG_USRSETTING = @"Software\{0}\Settings";
        private static Regex _regexWildCards = new Regex("(?<=#\\[)(.*?)(?=\\]#)");

        public enum FileOrDirectory
        {
            File = 0,
            Directory = 1,
            DoesNotExist = 2
        }

        public enum TextEncoding
        {
            Default = 0,
            ASCII = 1,
            BigEndianUnicode = 2,
            Unicode = 3,
            UTF32 = 4,
            UTF7 = 5,
            UTF8 = 6
        }

        public static decimal Bound(decimal value, decimal lowbound, decimal highbound) => (value > highbound) ? highbound : (value < lowbound) ? lowbound : value;

        public static Int16 Bound(Int16 value, Int16 lowbound, Int16 highbound) => (value >= highbound) ? highbound : (value <= lowbound) ? lowbound : value;

        public static Int32 Bound(Int32 value, Int32 lowbound, Int32 highbound) => (value >= highbound) ? highbound : (value <= lowbound) ? lowbound : value;

        public static Int64 Bound(Int64 value, Int64 lowbound, Int64 highbound) => (value >= highbound) ? highbound : (value <= lowbound) ? lowbound : value;

        public static double Bound(double value, double lowbound, double highbound) => (value >= highbound) ? highbound : (value <= lowbound) ? lowbound : value;

        public static string ByteArrayToString(byte[] bytes) => ByteArrayToString(bytes, TextEncoding.UTF8);

        public static string ByteArrayToString(byte[] bytes, TextEncoding encoding) => GetEncoder(encoding).GetString(bytes);

        public static string CamelCaseLower(string name)
        {
            StringBuilder result = new StringBuilder();
            char[] chars = name.ToCharArray();
            bool nextCap = false;
            bool isFirst = true;
            foreach (char c in chars)
            {
                if (char.IsLetter(c))
                {
                    if (isFirst)
                    {
                        result.Append(char.ToLower(c));
                        isFirst = false;
                        continue;
                    }
                    if (nextCap)
                    {
                        result.Append(char.ToUpper(c));
                        nextCap = false;
                    }
                    else
                        result.Append(c);
                }
                else
                    nextCap = true;
                if (nextCap)
                    continue;
            }
            return result.ToString();
        }

        public static string CamelCaseUpper(string name)
        {
            StringBuilder result = new StringBuilder();
            char[] chars = name.ToCharArray();
            bool nextCap = true;
            foreach (char c in chars)
            {
                if (char.IsLetter(c))
                {
                    if (nextCap)
                    {
                        result.Append(char.ToUpper(c));
                        nextCap = false;
                    }
                    else
                        result.Append(c);
                }
                else
                    nextCap = true;
                if (nextCap)
                    continue;
            }
            return result.ToString();
        }

        public static string CodeTypeToString(CodeType codeType)
        {
            switch (codeType)
            {
                case CodeType.SQL:
                    return "sql";

                case CodeType.JS:
                    return "js";

                case CodeType.XML:
                    return "xml";

                case CodeType.CSharp:
                    return "cs";

                case CodeType.HTML:
                    return "html";

                case CodeType.Lua:
                    return "lua";

                case CodeType.PHP:
                    return "php";

                case CodeType.VB:
                    return "vb";

                case CodeType.MarkDown:
                    return "md";
            }
            return String.Empty;
        }

        public static string CombinePath(string path1, string path2)
        {
            path1 = path1.Trim(new char[] { '\\' });
            path2 = path2.Trim(new char[] { '\\' });
            return $"{path1}\\{path2}";
        }

        public static string CompressString(string s)
        {
            if (string.IsNullOrEmpty(s))
                return string.Empty;

            byte[] buffer = Encoding.UTF8.GetBytes(s);
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (GZipStream gZipStream = new GZipStream(memoryStream, CompressionMode.Compress, true))
                    gZipStream.Write(buffer, 0, buffer.Length);

                memoryStream.Position = 0;

                byte[] compressedData = new byte[memoryStream.Length];
                memoryStream.Read(compressedData, 0, compressedData.Length);

                byte[] gZipBuffer = new byte[compressedData.Length + 4];
                Buffer.BlockCopy(compressedData, 0, gZipBuffer, 4, compressedData.Length);
                Buffer.BlockCopy(BitConverter.GetBytes(buffer.Length), 0, gZipBuffer, 0, 4);
                return Convert.ToBase64String(gZipBuffer);
            }
        }

        public static string DecompressString(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            byte[] gZipBuffer = Convert.FromBase64String(s);
            using (MemoryStream memoryStream = new MemoryStream())
            {
                int dataLength = BitConverter.ToInt32(gZipBuffer, 0);
                memoryStream.Write(gZipBuffer, 4, gZipBuffer.Length - 4);

                byte[] buffer = new byte[dataLength];

                memoryStream.Position = 0;
                using (GZipStream gZipStream = new GZipStream(memoryStream, CompressionMode.Decompress))
                    gZipStream.Read(buffer, 0, buffer.Length);

                return Encoding.UTF8.GetString(buffer);
            }
        }

        public static bool DerivesFromBaseType(Type type, Type basetype)
        {
            if (type == basetype)
                return true;

            if (type == null)
                return false;

            if (type.BaseType != null)
            {
                if (type.BaseType == basetype)
                    return true;

                return DerivesFromBaseType(type.BaseType, basetype);
            }
            return false;
        }

        public static string FromBase64(string text) => ByteArrayToString(Convert.FromBase64String(text));

        public static T FromJson<T>(string json)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));
            using (MemoryStream memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(json)))
                return (T)serializer.ReadObject(memoryStream);
        }

        public static List<T> FromJsonToList<T>(string json)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(List<T>));
            using (MemoryStream memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(json)))
                return (List<T>)serializer.ReadObject(memoryStream);
        }

        public static string GetCurrentUserRegisterKey(string regpath, string key, string defaultvalue)
        {
            if (string.IsNullOrEmpty(regpath))
                return string.Empty;

            if (string.IsNullOrEmpty(key))
                return string.Empty;

            string keyValue = defaultvalue;
            try
            {
                RegistryKey regKey = Registry.CurrentUser.OpenSubKey(regpath);
                if (regKey != null)
                {
                    keyValue = regKey.GetValue(key) as string;
                }
            }
            catch
            {
            }

            return keyValue;
        }

        public static string GetCurrentUserRegisterKey(string regpath, string key)
        {
            return GetCurrentUserRegisterKey(regpath, key, string.Empty);
        }

        public static Encoding GetEncoder(TextEncoding encoding)
        {
            switch (encoding)
            {
                case TextEncoding.ASCII:
                    return Encoding.ASCII;

                case TextEncoding.BigEndianUnicode:
                    return Encoding.BigEndianUnicode;

                case TextEncoding.Default:
                    return Encoding.Default;

                case TextEncoding.Unicode:
                    return Encoding.Unicode;

                case TextEncoding.UTF32:
                    return Encoding.UTF32;

                case TextEncoding.UTF7:
                    return Encoding.UTF7;

                case TextEncoding.UTF8:
                    return Encoding.UTF8;
            }
            return Encoding.Default;
        }

        /// <summary>
        /// <para>when path does not exist, it returns the closest directory to the specified path</para>
        /// this function is usefull when starting a browse file / directory dialog.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        /// <example>
        /// <code lang="cs">
        /// string s = GetExistingPath(@"D:\temp\test");
        ///
        /// // returns: "E:\temp\test" when test exists
        /// // returns: "E:\temp" when test does not exist.
        /// // returns: "E:" when temp does not exist.
        /// // returns: Environment.CurrentDirectory when path is null or empty.
        /// // returns: Environment.CurrentDirectory when E: does not exist
        /// // returns: DesktopDirectory when Environment.CurrentDirectory does not exist
        /// </code>
        /// </example>
        public static string GetExistingPath(string path)
        {
            // Remove invalid chars
            path = NormalizeFullPath(path, ' ');
            if (string.IsNullOrEmpty(path))
            {
                // Get currentdirectory when empty
                path = Environment.CurrentDirectory;
            }
            if (string.IsNullOrEmpty(path))
            {
                // when still empty get desktop directory.
                path = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            }

            // use FormatPath object to itterate through parts.

            string[] items = SplitPath(path);

            // while not exist shrink the path.

            foreach (string p in items)
            {
                FileOrDirectory exist = IsFileOrDirectory(p);
                if (exist == FileOrDirectory.Directory || exist == FileOrDirectory.File)
                {
                    path = p;
                }
            }
            if (string.IsNullOrEmpty(path))
            {
                // apperently the path was not valid in it's whole.
                path = Environment.CurrentDirectory;
            }
            if (string.IsNullOrEmpty(path))
            {
                path = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            }
            return path;
        }

        public static List<Type> GetObjectsWithBaseType(Type basetype, bool skipBaseType)
        {
            List<Type> types = new List<Type>();
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (Assembly assembly in assemblies)
                foreach (Type t in assembly.GetTypes())
                    if (DerivesFromBaseType(t, basetype))
                        if (!(t == basetype && skipBaseType))
                            types.Add(t);

            return types;
        }

        /// <summary>
        /// Berekend het weeknummer voor een opgegeven datum.
        /// </summary>
        /// <param name="date"></param>
        /// <returns>Weeknummer</returns>
        public static int GetWeekNumber(DateTime date)
        {
            const int JAN = 1;
            const int DEC = 12;
            const int LASTDAYOFDEC = 31;
            const int FIRSTDAYOFJAN = 1;
            const int THURSDAY = 4;
            bool ThursdayFlag = false;

            // Get the day number since the beginning of the year
            int DayOfYear = date.DayOfYear;

            // Get the numeric weekday of the first day of the
            // year (using sunday as FirstDay)
            int StartWeekDayOfYear = (int)(new DateTime(date.Year, JAN, FIRSTDAYOFJAN)).DayOfWeek;
            int EndWeekDayOfYear = (int)(new DateTime(date.Year, DEC, LASTDAYOFDEC)).DayOfWeek;

            // Compensate for the fact that we are using monday
            // as the first day of the week
            if (StartWeekDayOfYear == 0)
                StartWeekDayOfYear = 7;
            if (EndWeekDayOfYear == 0)
                EndWeekDayOfYear = 7;

            // Calculate the number of days in the first and last week
            int DaysInFirstWeek = 8 - (StartWeekDayOfYear);
            int DaysInLastWeek = 8 - (EndWeekDayOfYear);

            // If the year either starts or ends on a thursday it will have a 53rd week
            if (StartWeekDayOfYear == THURSDAY || EndWeekDayOfYear == THURSDAY)
                ThursdayFlag = true;

            // We begin by calculating the number of FULL weeks between the start of the year and
            // our date. The number is rounded up, so the smallest possible value is 0.
            int FullWeeks = (int)System.Math.Ceiling((DayOfYear - (DaysInFirstWeek)) / 7.0);

            int WeekNum = FullWeeks;

            // If the first week of the year has at least four days, then the actual week number for our date
            // can be incremented by one.
            if (DaysInFirstWeek >= THURSDAY)
                WeekNum = WeekNum + 1;

            // If week number is larger than week 52 (and the year doesn't either start or end on a thursday)
            // then the correct week number is 1.
            if (WeekNum > 52 && !ThursdayFlag)
                WeekNum = 1;

            // If week number is still 0, it means that we are trying to evaluate the week number for a
            // week that belongs in the previous year (since that week has 3 days or less in our date's year).
            // We therefore make a recursive call using the last day of the previous year.
            if (WeekNum == 0)
                WeekNum = GetWeekNumber(new DateTime(date.Year - 1, DEC, LASTDAYOFDEC));

            return WeekNum;
        }

        /// <summary>
        /// Returns the number of weeks for a year.
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public static int GetWeeksInYear(int year)
        {
            const int JAN = 1;
            const int DEC = 12;
            const int LASTDAYOFDEC = 31;
            const int FIRSTDAYOFJAN = 1;
            const int THURSDAY = 4;

            // Get the numeric weekday of the first day of the
            // year (using sunday as FirstDay)
            int StartWeekDayOfYear = (int)(new DateTime(year, JAN, FIRSTDAYOFJAN)).DayOfWeek;
            int EndWeekDayOfYear = (int)(new DateTime(year, DEC, LASTDAYOFDEC)).DayOfWeek;

            // Compensate for the fact that we are using monday
            // as the first day of the week
            StartWeekDayOfYear = (StartWeekDayOfYear == 0) ? 7 : StartWeekDayOfYear;
            EndWeekDayOfYear = (EndWeekDayOfYear == 0) ? 7 : EndWeekDayOfYear;

            // If the year either starts or ends on a thursday it will have a 53rd week
            return (StartWeekDayOfYear == THURSDAY || EndWeekDayOfYear == THURSDAY) ? 53 : 52;
        }

        /// <summary>
        /// Calculates start date for the week
        /// </summary>
        /// <param name="week"></param>
        /// <param name="year"></param>
        /// <returns>StartDate or endDate</returns>
        public static DateTime GetWeekStartDate(int week, int year)
        {
            const int FIRSTDAYOFJAN = 1;
            const int JAN = 1;
            const int THURSDAY = 4;

            week = Bound(week, 1, GetWeeksInYear(year));

            // Calculate Day of the week
            int StartWeekDayOfYear = (int)(new DateTime(year, JAN, FIRSTDAYOFJAN)).DayOfWeek;
            StartWeekDayOfYear = (StartWeekDayOfYear == 0) ? 7 : StartWeekDayOfYear;

            // Calculate the number of days in the first
            int DaysInFirstWeek = 8 - (StartWeekDayOfYear);
            DateTime startDateFirstWeek = new DateTime(year, JAN, FIRSTDAYOFJAN);

            if (DaysInFirstWeek >= THURSDAY)
                startDateFirstWeek = startDateFirstWeek.AddDays(-(StartWeekDayOfYear - 1)); // Date for first day of the week, might be in december previous year.
            else
                startDateFirstWeek = startDateFirstWeek.AddDays(DaysInFirstWeek); // To few days in the week for january the first, shift to the next week

            return startDateFirstWeek.AddDays((week - 1) * 7);
        }

        public static string InsertLine(string text, int line, string insert)
        {
            string[] _lines = CodeLibrary.Core.Utils.SplitLines(text);
            StringBuilder _sb = new StringBuilder();
            for (int index = 0; index < _lines.Length; index++)
            {
                _sb.AppendLine(_lines[index]);
                if (index == line)
                {
                    _sb.AppendLine(insert);
                }
            }
            return _sb.ToString();
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

        public static bool IsNumeric(this string s)
        {
            if (string.IsNullOrEmpty(s)) return false;

            Char[] chars = s.ToCharArray();
            for (int ii = 0; ii < chars.Length; ii++)
            {
                if (!Char.IsDigit(chars[ii]))
                    return false;
            }
            return true;
        }

        public static bool isReorderString(string text)
        {
            string[] _reorderString = text.Split(new char[] { ',' });

            if (_reorderString.Length == 0)
                return false;

            foreach (string item in _reorderString)
            {
                if (!IsNumeric(item))
                    return false;
            }

            return true;
        }

        public static bool MatchPattern(string s, string pattern)
        {
            if (pattern == null || s == null) return false;

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

        public static string Merge(string text, CodeType targetType)
        {
            string _newText = text;
            var _matches = _regexWildCards.Matches(text);
            if (_matches == null)
            {
                return text;
            }

            int _counter = 0;

            while (_matches.Count > 0)
            {
                _counter++;
                if (_counter > 300)
                {
                    return "CIRCULAR REFERENCE ERROR!";
                }
                string _text = string.Empty;

                foreach (Match match in _matches)
                {
                    // Get by path
                    CodeSnippet _snippet = CodeLib.Instance.CodeSnippets.GetByPath(match.Value);
                    if (_snippet == null)
                    {
                        // Get by id
                        _snippet = CodeLib.Instance.CodeSnippets.Get(match.Value);
                        _text = Core.Utils.SnippetToText(_snippet, targetType);
                    }
                    else if (_snippet == null)
                    {
                        // try get by pattern.
                        var _snippets = CodeLib.Instance.CodeSnippets.GetChildsByPathAndPattern(match.Value);
                        StringBuilder _sb = new StringBuilder();
                        foreach (CodeSnippet snippet in _snippets)
                        {
                            _sb.Append(Core.Utils.SnippetToText(snippet, targetType));
                        }
                        _text = _sb.ToString();
                    }
                    else
                    {
                        _text = Core.Utils.SnippetToText(_snippet, targetType);
                    }

                    _newText = _newText.Replace($"#[{match.Value}]#", _text);
                }

                _matches = _regexWildCards.Matches(_newText);
            }

            return _newText;
        }

        /// <summary>
        /// Fixes a full path, removes invalid chars like :\/?*|: from string.
        /// </summary>
        /// <param name="fullpath">full path to normalize.</param>
        /// <returns>string.Empty when fullpath is null or empty.</returns>
        public static string NormalizeFullPath(string fullpath)
        {
            return NormalizeFullPath(fullpath, ' ');
        }

        /// <summary>
        /// Fixes a full path, removes invalid chars like :\/?*|: from string.
        /// </summary>
        /// <param name="fullpath">full path to normalize.</param>
        /// <param name="invalidCharReplacement">Replacementchar for invalid chars.</param>
        /// <returns>string.Empty when fullpath is null or empty.</returns>
        public static string NormalizeFullPath(string fullpath, char invalidCharReplacement)
        {
            if (string.IsNullOrEmpty(fullpath))
            {
                return string.Empty;
            }
            int filenameIndex = fullpath.LastIndexOf(Path.DirectorySeparatorChar);

            char[] chpath = fullpath.ToCharArray();
            char[] invalidPathChars = Path.GetInvalidPathChars();
            char[] invalidFileChars = Path.GetInvalidFileNameChars();

            for (int ii = 0; ii < chpath.Length; ii++)
            {
                if (ii <= filenameIndex)
                {
                    for (int xx = 0; xx < invalidPathChars.Length; xx++)
                    {
                        if (chpath[ii] == invalidPathChars[xx])
                            chpath[ii] = invalidCharReplacement;
                    }
                }
                else
                {
                    for (int xx = 0; xx < invalidFileChars.Length; xx++)
                    {
                        if (chpath[ii] == invalidFileChars[xx])
                            chpath[ii] = invalidCharReplacement;
                    }
                }
            }
            return new string(chpath);
        }

        public static string NormalizeQuotes(string value)
        {
            if (string.IsNullOrEmpty(value))
                return string.Empty;

            char[] array = value.ToCharArray();
            for (int ii = 0; ii < array.Length; ii++)
            {
                if (array[ii] == '‘' || array[ii] == '’' || array[ii] == '‚')
                    array[ii] = _SINGLEQUOTE;

                if (array[ii] == '“' || array[ii] == '”' || array[ii] == '„' || array[ii] == '¨')
                    array[ii] = _DOUBLEQUOTE;
            }
            string s = new string(array);
            return s.Replace("'", "''");
        }

        public static string ParentPath(string path)
        {
            return ParentPath(path, '\\');
        }

        public static string ParentPath(string path, char separator)
        {
            string[] paths = SplitPath(path, separator);
            if (paths.Length <= 1)
                return string.Empty;
            return paths[paths.Length - 2];
        }

        public static string PathCombine(params string[] paths)
        {
            StringBuilder sb = new StringBuilder();
            string _path;
            for (int ii = 0; ii < paths.Length; ii++)
            {
                if (ii > 0)
                    _path = $"\\{paths[ii].Trim(new char[] { ' ', '\\' })}";
                else
                    _path = $"{paths[ii].Trim(new char[] { ' ', '\\' })}";
                sb.Append(_path);
            }
            _path = sb.ToString();
            if (_path.Contains("\\\\"))
                _path = _path.Replace("\\\\", "\\");

            return _path;
        }

        public static string PathName(string path) => PathName(path, '\\');

        public static string PathName(string path, char separator)
        {
            if (string.IsNullOrEmpty(path))
                return string.Empty;

            string[] paths = path.Split(separator);
            if (paths.Length == 1)
                return paths[0];
            return paths[paths.Length - 1];
        }

        public static void SetCurrentUserRegisterKey(string regpath, string key, string value)
        {
            if (string.IsNullOrEmpty(regpath) || string.IsNullOrEmpty(key))
                return;

            if (string.IsNullOrEmpty(value))
                value = string.Empty;

            string[] keytree = SplitPath(regpath, '\\');

            // Check whether each leave in the tree exists, if not create the leave.
            foreach (string s in keytree)
            {
                RegistryKey regKey = Registry.CurrentUser.OpenSubKey(s, RegistryKeyPermissionCheck.ReadWriteSubTree, RegistryRights.FullControl);
                if (regKey == null)
                    Registry.CurrentUser.CreateSubKey(s);
            }

            RegistryKey editKey = Registry.CurrentUser.OpenSubKey(regpath, RegistryKeyPermissionCheck.ReadWriteSubTree, RegistryRights.FullControl);
            if (editKey != null)
            {
                editKey.SetValue(key, value);
            }
        }

        public static string SnippetToText(CodeSnippet snippet, CodeType targetType)
        {
            string _result = string.Empty;
            if (snippet == null)
            {
                return String.Empty;
            }

            if (snippet.CodeType == CodeType.ReferenceLink)
            {
                snippet = CodeLib.Instance.CodeSnippets.Get(snippet.ReferenceLinkId);
            }
            switch (snippet.CodeType)
            {
                case CodeType.Image:
                    string _base64 = Convert.ToBase64String(snippet.Blob);
                    switch (targetType)
                    {
                        case CodeType.MarkDown:
                            _result = string.Format(@"![{0}](data:image/png;base64,{1})", snippet.GetPath(), _base64);
                            break;

                        case CodeType.HTML:
                            _result = string.Format(@"<img src=""data:image/png;base64,{0}"" />", _base64);
                            break;

                        default:
                            _result = _base64;
                            break;
                    }
                    break;

                case CodeType.CSharp:
                case CodeType.HTML:
                case CodeType.JS:
                case CodeType.Lua:
                case CodeType.PHP:
                case CodeType.SQL:
                case CodeType.VB:
                case CodeType.XML:
                case CodeType.MarkDown:
                    if (snippet.CodeType == CodeType.MarkDown && targetType == CodeType.HTML)
                    {
                        MarkDigWrapper _markDig = new MarkDigWrapper();
                        _result = _markDig.Transform(snippet.GetCode());
                    }
                    else if (targetType == CodeType.MarkDown)
                    {
                        if (snippet.CodeType == CodeType.MarkDown)
                        {
                            _result = snippet.GetCode();
                        }
                        else
                        {
                            _result = string.Format("\r\n~~~{0}\r\n{1}\r\n~~~\r\n", CodeTypeToString(snippet.CodeType), snippet.GetCode());
                        }
                    }
                    else
                    {
                        _result = snippet.GetCode();
                    }
                    break;

                default:
                    _result = snippet.GetCode();
                    break;
            }

            return _result;
        }

        public static List<string> Split(string text, string splitter, bool skipEmpty)
        {
            int _splitterLen = splitter.Length;
            List<string> _items = new List<string>();
            int _start = 0;
            int _end = text.IndexOf(splitter);
            while (_end > -1)
            {
                string _item = text.Substring(_start, _end - _start);
                if (_item.Length == 0 && skipEmpty)
                    _items.Add(_item);
                else
                    _items.Add(_item);

                _start = _end + _splitterLen;
                _end = text.IndexOf(splitter, _start);
            }
            if (_start < text.Length)
            {
                string _item = text.Substring(_start, text.Length - _start);
                if (_item.Length == 0 && skipEmpty)
                    _items.Add(_item);
                else
                    _items.Add(_item);
            }

            return _items;
        }

        // #TODO Misses last line
        public static string[] SplitLines(string text)
        {
            var _result = new List<string>();
            var _partBuilder = new StringBuilder();
            var _textCharArray = text.ToCharArray();
            var _prevChar = (char)0;

            for (int ii = 0; ii < _textCharArray.Length; ii++)
            {
                char _currChar = _textCharArray[ii];

                if (_currChar == '\n' && _prevChar == '\r')
                {
                    _partBuilder.Length--;
                    _result.Add(_partBuilder.ToString());
                    _partBuilder = new StringBuilder();
                    _prevChar = (char)0;
                    continue;
                }
                if (_currChar == '\n' && _prevChar != '\r')
                {
                    _result.Add(_partBuilder.ToString());
                    _partBuilder = new StringBuilder();
                    _prevChar = (char)0;
                    continue;
                }
                if (_prevChar == '\n' || _prevChar == '\r')
                {
                    if (_currChar != '\r' && _currChar != '\n')
                        _partBuilder.Append(_currChar);

                    _result.Add(_partBuilder.ToString());
                    _partBuilder = new StringBuilder();
                    _prevChar = _textCharArray[ii];
                    continue;
                }
                _partBuilder.Append(_currChar);
                _prevChar = _textCharArray[ii];
            }

            if (_partBuilder.Length > 0)
            {
                _result.Add(_partBuilder.ToString());
            }

            return _result.ToArray();
        }

        public static string[] SplitPath(string path, char separator)
        {
            if (string.IsNullOrEmpty(path))
                return new string[0];

            string[] items = path.Split(new char[] { separator }, StringSplitOptions.RemoveEmptyEntries);
            if (items.Length == 0)
                return new string[0];

            if (items.Length == 1)
                return items;

            string[] items2 = new string[items.Length];
            items2[0] = items[0];
            for (int ii = 1; ii < items.Length; ii++)
                items2[ii] = string.Format("{0}{1}{2}", items2[ii - 1], separator, items[ii]);

            for (int ii = 0; ii < items.Length; ii++)
                items[ii] = items2[(items.Length - 1) - ii];

            return items2;
        }

        public static string[] SplitPath(string path) => SplitPath(path, Path.DirectorySeparatorChar);

        public static string StreamToString(Stream stream)
        {
            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
            {
                stream.Position = 0;
                return reader.ReadToEnd();
            }
        }

        public static byte[] StringToByteArray(string s) => StringToByteArray(s, TextEncoding.UTF8);

        public static byte[] StringToByteArray(string s, TextEncoding encoding) => GetEncoder(encoding).GetBytes(s);

        public static string ToBase64(string text) => Convert.ToBase64String(StringToByteArray(text));

        public static string ToJson<T>(IEnumerable<T> items)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(IEnumerable<T>));
                serializer.WriteObject(stream, items);
                stream.Position = 0;
                using (StreamReader streamReader = new StreamReader(stream))
                    return streamReader.ReadToEnd();
            }
        }

        public static string ToJson<T>(T items)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));
                serializer.WriteObject(stream, items);
                stream.Position = 0;
                using (StreamReader streamReader = new StreamReader(stream))
                    return streamReader.ReadToEnd();
            }
        }

        public static string ToSqlAscii(string value)
        {
            string s = NormalizeQuotes(value); // do not accept strange quotes.
            return s.Replace("'", "''");
        }

        public static string TrimText(string text, string trim)
        {
            try
            {
                char[] _trims = trim.ToCharArray();
                char[] _text = text.ToCharArray();
                bool _exit = false;
                int _start = 0;
                int _end = 0;

                for (int ii = 0; ii < _text.Length; ii += _trims.Length)
                {
                    for (int jj = 0; jj < _trims.Length; jj++)
                    {
                        if (_text[ii + jj] != _trims[jj])
                        {
                            _exit = true;
                            break;
                        }
                    }
                    _start = ii;
                    if (_exit)
                        break;
                }

                _end = _text.Length;
                _exit = false;

                for (int ii = _text.Length - _trims.Length, xx = 0; ii > 0; ii -= _trims.Length, xx++)
                {
                    for (int jj = 0; jj < _trims.Length; jj++)
                    {
                        if (_text[ii + jj] != _trims[jj])
                        {
                            _exit = true;
                            break;
                        }
                    }
                    if (!_exit)
                    {
                        _end -= _trims.Length;
                    }
                    else
                    {
                        break;
                    }
                }

                return text.Substring(_start, _end - _start);
            }
            catch
            {
            }

            return text;
        }

        public static string FormatXml(string xml, out bool succes)
        {
            string result = "";

            using (MemoryStream mStream = new MemoryStream())
            {
                using (XmlTextWriter writer = new XmlTextWriter(mStream, Encoding.Unicode))
                {
                    XmlDocument document = new XmlDocument();
                    try
                    {
                        // Load the XmlDocument with the XML.
                        document.LoadXml(xml);

                        writer.Formatting = Formatting.Indented;
                        //writer.Settings.Indent = true;

                        // Write the XML into a formatting XmlTextWriter
                        document.WriteContentTo(writer);
                        writer.Flush();
                        mStream.Flush();

                        // Have to rewind the MemoryStream in order to read
                        // its contents.
                        mStream.Position = 0;

                        // Read MemoryStream contents into a StreamReader.
                        StreamReader sReader = new StreamReader(mStream);

                        // Extract the text from the StreamReader.
                        string formattedXml = sReader.ReadToEnd();

                        result = formattedXml;
                    }
                    catch (XmlException)
                    {
                        succes = false;
                        return null;
                        // Handle the exception
                    }
                    succes = true;
                    return result;
                }
            }
        }
    }
}