﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeLibrary.Core.DevToys
{
    /// <summary>
    /// Indicate how lines are ended when writing CSV Files.
    /// </summary>
    public enum CRLFMode
    {
        /// <summary>
        /// \r\n = CR + LF → Used as a new line character in Windows
        /// </summary>
        CRLF = 0,

        /// <summary>
        /// \r = CR(Carriage Return) → Used as a new line character in Mac OS before X
        /// </summary>
        CR = 1,

        /// <summary>
        ///  \n = LF(Line Feed) → Used as a new line character in Unix/Mac OS X
        /// </summary>
        LF = 2,
    }


    /// <summary>
    /// Implements a System.IO.TextWriter for writing characters to a stream in a particular encoding.
    /// Extended with WriteCsvLine to write a Csv Line conform RFC 4180.
    /// </summary>
    public sealed class CsvStreamWriter : StreamWriter
    {
        private char _Separator = ',';
        private const char _CR = '\r';
        private const char _LF = '\n';
        private const string _CRLF = "\r\n";
        private char[] _EscapeChars = null;
        private int[] _Indexes = new int[0];
        private int _IndexesIndex = 0;

        /// <summary>
        /// Initializes a new instance of the System.IO.StreamWriter class for the specified file by using the default encoding and buffer size.
        /// </summary>
        /// <param name="path">The complete file path to write to. path can be a file name.</param>
        public CsvStreamWriter(string path) : base(path)
        {
            Separator = ',';
        }

        /// <summary>
        /// Initializes a new instance of the System.IO.StreamWriter class for the specified file on the specified path, using the specified encoding and buffer size. If the file exists, it can be either overwritten or appended to. If the file does not exist, this constructor creates a new file.
        /// </summary>
        /// <param name="path">The complete file path to write to. path can be a file name.</param>
        /// <param name="append">true to append data to the file; false to overwrite the file. If the specified file does not exist, this parameter has no effect, and the constructor creates a new file.</param>
        /// <param name="encoding">The character encoding to use.</param>
        /// <param name="bufferSize">The buffer size, in bytes.</param>
        public CsvStreamWriter(string path, bool append = false, Encoding encoding = null, int bufferSize = -1) : base(path, append, encoding, bufferSize)
        {
            Separator = ',';
        }

#if NET60 || NET70
        /// <summary>
        ///
        /// </summary>
        /// <param name="path"></param>
        /// <param name="encoding"></param>
        /// <param name="options"></param>
        public CsvStreamWriter(string path, Encoding encoding, FileStreamOptions options) : base(path, encoding, options)
        {
        }
#endif

        /// <summary>
        /// Initializes a new instance of the System.IO.StreamWriter class for the specified stream by using UTF-8 encoding and the default buffer size.
        /// </summary>
        /// <param name="stream">The stream to write to.</param>
        public CsvStreamWriter(Stream stream) : base(stream)
        {
            Separator = ',';
        }

        /// <summary>
        /// Initializes a new instance of the System.IO.StreamWriter class for the specified file by using the default encoding and buffer size.
        /// </summary>
        /// <param name="stream">The stream to write to.</param>
        /// <param name="encoding">The character encoding to use.</param>
        /// <param name="bufferSize">The buffer size, in bytes.</param>
        /// <param name="leaveOpen">true to leave the stream open after the System.IO.StreamWriter object is disposed; otherwise, false.</param>
        public CsvStreamWriter(Stream stream, Encoding encoding = null, int bufferSize = -1, bool leaveOpen = false) : base(stream, encoding, bufferSize, leaveOpen)
        {
            Separator = ',';
        }

        /// <summary>
        /// Limit the output to selected indexes.
        /// </summary>
        public void SetColumnIndexes(params int[] indexes)
        {
            if (indexes == null)
            {
                _Indexes = new int[0];
                return;
            }
            _Indexes = indexes.OrderBy(p => p).ToArray();
        }

        /// <summary>
        /// Csv Seperator to use default ','
        /// </summary>
        public char Separator
        {
            get
            {
                return _Separator;
            }
            set
            {
                _Separator = value;
                InitializeEscapeChars();
            }
        }

        /// <summary>
        /// \r\n = CR + LF → Used as a new line character in Windows.
        /// \r = CR(Carriage Return) → Used as a new line character in Mac OS before X.
        /// \n = LF(Line Feed) → Used as a new line character in Unix/Mac OS X
        /// </summary>
        public CRLFMode CRLFMode { get; set; } = CRLFMode.CRLF;

        /// <summary>
        /// Write an array of strings to the Csv Stream and escapes when nececary.
        /// </summary>
        /// <param name="values">Array of strings</param>
        public void WriteCsvLine(params string[] values)
        {
            if (BaseStream.Position == 0)
            {
                Flush();
            }

            if (BaseStream.Position > 0)
            {
                if (CRLFMode == CRLFMode.CRLF)
                {
                    Write(_CRLF);
                }
                else if (CRLFMode == CRLFMode.CR)
                {
                    Write(_CR);
                }
                else if (CRLFMode == CRLFMode.LF)
                {
                    Write(_LF);
                }
            }

            bool _first = true;
            _IndexesIndex = 0;
            for (int ii = 0; ii < values.Length; ii++)
            {
                if (_Indexes.Length == 0 || _IndexesIndex < _Indexes.Length && _Indexes[_IndexesIndex] == ii)
                {
                    if (_first == false)
                    {
                        Write(Separator);
                    }
                    _first = false;
                    Write(Escape(values[ii] ?? ""));
                    _IndexesIndex++;
                }
            }
        }

        /// <summary>
        /// Write an array of strings to the Csv Stream and escapes when nececary.
        /// </summary>
        /// <param name="values">Array of objects</param>
        public void WriteCsvLine(params object[] values)
        {
            if (BaseStream.Position == 0)
            {
                Flush();
            }

            if (BaseStream.Position > 0)
            {
                if (CRLFMode == CRLFMode.CRLF)
                {
                    Write(_CRLF);
                }
                else if (CRLFMode == CRLFMode.CR)
                {
                    Write(_CR);
                }
                else if (CRLFMode == CRLFMode.LF)
                {
                    Write(_LF);
                }
            }

            bool _first = true;
            _IndexesIndex = 0;
            for (int ii = 0; ii < values.Length; ii++)
            {
                if (_Indexes.Length == 0 || _IndexesIndex < _Indexes.Length && _Indexes[_IndexesIndex] == ii)
                {
                    if (_first == false)
                    {
                        Write(Separator);
                    }
                    _first = false;
                    Write(Escape(values[ii] != null ? values[ii].ToString() : ""));
                    _IndexesIndex++;
                }
            }
        }

        /// <summary>: 
        /// Write an array of strings to the Csv Stream and escapes when nececary.
        /// </summary>
        /// <param name="values">Array of strings</param>
        public void WriteCsvLine(IEnumerable<string> values)
        {
            if (BaseStream.Position == 0)
            {
                Flush();
            }

            if (BaseStream.Position > 0)
            {
                if (CRLFMode == CRLFMode.CRLF)
                {
                    Write(_CRLF);
                }
                else if (CRLFMode == CRLFMode.CR)
                {
                    Write(_CR);
                }
                else if (CRLFMode == CRLFMode.LF)
                {
                    Write(_LF);
                }
            }

            bool _first = true;
            int _index = 0;
            _IndexesIndex = 0;

            foreach (string value in values)
            {
                if (_Indexes.Length == 0 || _IndexesIndex < _Indexes.Length && _Indexes[_IndexesIndex] == _index)
                {
                    if (_first == false)
                    {
                        Write(Separator);
                    }
                    _first = false;
                    Write(Escape(value ?? ""));
                    _IndexesIndex++;
                }
                _index++;
            }
        }

        private void InitializeEscapeChars()
        {
            _EscapeChars = new char[] { '\r', '\n', '"', _Separator };
        }

        private string Escape(string s)
        {
            if (s.IndexOfAny(_EscapeChars) == -1)
            {
                return s;
            }
            if (s.IndexOf('"') == -1)
            {
                return $"\"{s}\""; // No need for replace.
            }
            return $"\"{s.Replace("\"", "\"\"")}\"";
        }
    }
}
