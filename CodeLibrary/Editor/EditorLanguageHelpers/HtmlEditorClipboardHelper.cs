using CodeLibrary.Core;
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
                        string _text = File.ReadAllText(_filename);
                        CodeType _codeType = LocalUtils.CodeTypeByExtension(new FileInfo(_filename));
                        _sb.AppendLine(string.Format("\r\n~~~{0}\r\n{1}\r\n~~~\r\n", Core.Utils.CodeTypeToString(_codeType), _text));
                        _sb.AppendLine();
                        break;

                    case CodeType.Template:
                        string _csv = File.ReadAllText(_filename);

                        bool _isCsv = CsvUtils.GetCsvSeparator(_csv, out char _separator);
                        if (_isCsv)
                        {
                            if (Core.Utils.isReorderString(SelectedText))
                            {
                                string _reorderString = SelectedText;
                                _csv = CsvUtils.CsvChange(_csv, _separator, _separator, _reorderString);
                            }

                            _csv = CsvUtils.CsvToMdTable(_csv, _separator);

                            MarkDigWrapper _wrapper = new MarkDigWrapper();

                            _sb.AppendLine();
                            _sb.AppendLine(_wrapper.Transform(_csv));
                            _sb.AppendLine();
                            return;
                        }

                        break;

                    case CodeType.System:
                        break;

                    case CodeType.RTF:
                    case CodeType.UnSuported:
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
            SelectedText = string.Format(@"<img src=""data:image/png;base64,{0}"" />", _base64);
        }

        protected override void Paste_CtrlShift_Text()
        {
            MarkDigWrapper _wrapper = new MarkDigWrapper();
            string _text = Clipboard.GetText();

            bool _isCsv = CsvUtils.GetCsvSeparator(_text, out char _separator);
            if (_isCsv)
            {
                if (Core.Utils.isReorderString(SelectedText))
                {
                    string _reorderString = SelectedText;
                    _text = CsvUtils.CsvChange(_text, _separator, _separator, _reorderString);
                }

                _text = CsvUtils.CsvToMdTable(_text, _separator);
                SelectedText = _wrapper.Transform(_text);
                return;
            }
            base.Paste_Text();
        }

        protected override void Paste_CtrlShift_TextImage()
        {
            MarkDigWrapper _wrapper = new MarkDigWrapper();
            string _text = Clipboard.GetText();

            bool _isCsv = CsvUtils.GetCsvSeparator(_text, out char _separator);
            if (_isCsv)
            {
                if (Core.Utils.isReorderString(SelectedText))
                {
                    string _reorderString = SelectedText;
                    _text = CsvUtils.CsvChange(_text, _separator, _separator, _reorderString);
                }

                _text = CsvUtils.CsvToMdTable(_text, _separator);
                SelectedText = _wrapper.Transform(_text);
                return;
            }

            base.Paste_CtrlShift_TextImage();
        }



    }
}