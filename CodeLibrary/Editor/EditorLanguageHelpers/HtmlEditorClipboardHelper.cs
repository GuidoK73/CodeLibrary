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
    public class HtmlEditorClipboardHelper : EditorClipboardHelperBase
    {
        public HtmlEditorClipboardHelper(FormCodeLibrary mainform, TextBoxHelper textboxHelper, ThemeHelper themeHelper) : base(mainform, textboxHelper, themeHelper)
        {
        }

        protected override void PasteAdvancedFileDropList()
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

                switch (_type)
                {
                    case CodeType.Image:
                        byte[] _imageData = File.ReadAllBytes(_filename);
                        string _base64 = Convert.ToBase64String(_imageData);
                        _sb.AppendLine(string.Format(@"<img src=""data:image/png;base64,{0}"" />", _base64));
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
                    case CodeType.Template:
                    case CodeType.RTF:
                        string _text = File.ReadAllText(_filename);
                        CodeType _codeType = LocalUtils.CodeTypeByExtension(new FileInfo(_filename));
                        _sb.AppendLine(string.Format("\r\n~~~{0}\r\n{1}\r\n~~~\r\n", Core.Utils.CodeTypeToString(_codeType), _text));
                        _sb.AppendLine();
                        break;

                    case CodeType.System:
                    case CodeType.UnSuported:
                        break;
                }
            }
            SelectedText = _sb.ToString();

        }

        protected override void PasteAdvancedImage()
        {
            Image _image = Clipboard.GetImage();
            string _base64 = Convert.ToBase64String(_image.ConvertImageToByteArray());
            SelectedText = string.Format(@"<img src=""data:image/png;base64,{0}"" />", _base64);
        }

        protected override void PasteAdvancedText()
        {
            MarkDigWrapper _wrapper = new MarkDigWrapper();
            string _text = Clipboard.GetText();
            char _separator = ' ';
            string _data = string.Empty;

            bool _isCsv = Core.Utils.GetCsvSeparator(_text, out _separator);
            if (_isCsv)
            {
                _text = Core.Utils.CsvToMdTable(_text , _separator);
                SelectedText = _wrapper.Transform(_text);
                return;
            }
            base.PasteText();
        }

        protected override void PasteAdvancedTextImage()
        {
            MarkDigWrapper _wrapper = new MarkDigWrapper();
            string _text = Clipboard.GetText();
            char _separator = ' ';
            string _data = string.Empty;

            bool _isCsv = Core.Utils.GetCsvSeparator(_text, out _separator);
            if (_isCsv)
            {
                _text = Core.Utils.CsvToMdTable(_text, _separator);
                SelectedText = _wrapper.Transform(_text);
                return;
            }
        }
    }
}