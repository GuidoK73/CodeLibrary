﻿using CodeLibrary.Core;
using CodeLibrary.Extensions;
using CodeLibrary.Helpers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace CodeLibrary.Editor.EditorLanguageHelpers
{
    public class MarkdownEditorLClipboardHelper : EditorClipboardHelperBase
    {
        public MarkdownEditorLClipboardHelper(FormCodeLibrary mainform, TextBoxHelper textboxHelper, ThemeHelper themeHelper) : base(mainform, textboxHelper, themeHelper)
        {
        }

        protected override void Paste_CtrlShift_FileDropList()
        {
            List<string> _filenames = new List<string>();
            foreach (string s in Clipboard.GetFileDropList())
            {
                _filenames.Add(s);
            }

            StringBuilder _sb = new StringBuilder();

            foreach (string _filename in _filenames)
            {
                FileInfo _file = new FileInfo(_filename);
                var _type = LocalUtils.CodeTypeByExtension(_file);
                byte[] _fileData;
                string _base64;

                switch (_type)
                {
                    case CodeType.Image:
                        _fileData = File.ReadAllBytes(_filename);
                        _base64 = Convert.ToBase64String(_fileData);
                        _sb.AppendLine(string.Format(@"![{0}](data:image/png;base64,{1})", StateSnippet.GetPath(), _base64));
                        _sb.AppendLine();
                        break;

                    case CodeType.CSharp:
                    case CodeType.HTML:
                    case CodeType.MarkDown:
                    case CodeType.JS:
                    case CodeType.Lua:
                    case CodeType.PHP:
                    case CodeType.VB:
                    case CodeType.None:
                    case CodeType.SQL:
                    case CodeType.XML:
                        string _text = File.ReadAllText(_filename);
                        CodeType _codeType = LocalUtils.CodeTypeByExtension(new FileInfo(_filename));
                        _sb.AppendLine(string.Format("\r\n~~~{0}\r\n{1}\r\n~~~\r\n", Core.Utils.CodeTypeToString(_codeType), _text));
                        _sb.AppendLine();
                        break;

                    case CodeType.Template:
                        string _csv = File.ReadAllText(_filename);

                        bool _isCsv = Core.Utils.GetCsvSeparator(_csv, out char _separator);
                        if (_isCsv)
                        {
                            if (Core.Utils.isReorderString(SelectedText))
                            {
                                string _reorderString = SelectedText;
                                _csv = Core.Utils.CsvChange(_csv, _separator, _separator, _reorderString);
                            }

                            _sb.AppendLine();
                            _sb.AppendLine(Core.Utils.CsvToMdTable(_csv, _separator));
                            _sb.AppendLine();
                            return;
                        }

                        break;


                    case CodeType.System:
                        break;

                    case CodeType.UnSuported:
                    case CodeType.RTF:
                        _fileData = File.ReadAllBytes(_filename);
                        _base64 = Convert.ToBase64String(_fileData);
                        FileInfo _fileInfo = new FileInfo(_filename);
                        _sb.AppendLine(string.Format(@"Download: <a href=""data:application/{2};base64,{0}"">{1}</a>", _base64, _fileInfo.Name, _fileInfo.Extension.TrimStart(new char[] { '.' })));
                        _sb.AppendLine();
                        break;
                }
            }
            SelectedText = _sb.ToString();
        }

        protected override void Paste_CtrlShift_Image()
        {
            Image _image = Clipboard.GetImage();
            string _base64 = Convert.ToBase64String(_image.ConvertImageToByteArray());
            SelectedText = string.Format(@"![{0}](data:image/png;base64,{1})", StateSnippet.GetPath(), _base64);
        }

        protected override void Paste_CtrlShift_Text()
        {
            string _text = Clipboard.GetText();
            _text = Core.Utils.TrimText(_text, "\r\n");

            char _separator = ' ';
            string _data = string.Empty;

            bool _isCsv = Core.Utils.GetCsvSeparator(_text, out _separator);
            if (_isCsv)
            {
                _data = Core.Utils.CsvToMdTable(_text, _separator);
                this.SelectedText = _data;
                return;
            }

            base.Paste_Text();
        }

        protected override void Paste_CtrlShift_TextImage()
        {
            string _text = Clipboard.GetText();
            _text = Core.Utils.TrimText(_text, "\r\n");

            bool _isCsv = Core.Utils.GetCsvSeparator(_text, out char _separator);
            if (_isCsv)
            {
                if (Core.Utils.isReorderString(SelectedText))
                {
                    string _reorderString = SelectedText;
                    _text = Core.Utils.CsvChange(_text, _separator, _separator, _reorderString);
                }

                _text = Core.Utils.CsvToMdTable(_text, _separator);
                this.SelectedText = _text;
                return;
            }
        }

        protected override void Paste_CtrlAltShift_TextImage() => Paste_CtrlShift_TextImage();

        protected override void Paste_CtrlAltShift_Image() => Paste_CtrlShift_Image();

        protected override void Paste_CtrlAltShift_FileDropList() => Paste_CtrlShift_FileDropList();

        protected override void Paste_CtrlAltShift_Text()
        {
            string _text = Clipboard.GetText();
            _text = Core.Utils.TrimText(_text, "\r\n");

            char _separator = ' ';

            bool _isCsv = Core.Utils.GetCsvSeparator(_text, out _separator);
            if (_isCsv)
            {

                if (Core.Utils.isReorderString(SelectedText))
                {
                    string _reorderString = SelectedText;
                    _text = Core.Utils.CsvChange(_text, _separator, _separator, _reorderString);
                }

                _text = Core.Utils.CsvToMdTableNoHeader(_text, _separator);
                this.SelectedText = _text;
                return;
            }

            base.Paste_Text();
        }



    }
}