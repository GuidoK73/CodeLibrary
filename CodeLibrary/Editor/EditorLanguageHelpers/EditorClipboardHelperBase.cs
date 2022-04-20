using CodeLibrary.Core;
using CodeLibrary.Extensions;
using CodeLibrary.Helpers;
using FastColoredTextBoxNS;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace CodeLibrary.Editor.EditorLanguageHelpers
{
    public class EditorClipboardHelperBase
    {
        protected readonly FormCodeLibrary _mainform;
        protected readonly FastColoredTextBox _tb;
        protected readonly TextBoxHelper _TextBoxHelper;
        protected readonly ThemeHelper _ThemeHelper;

        public EditorClipboardHelperBase(FormCodeLibrary mainform, TextBoxHelper textboxHelper, ThemeHelper themeHelper)
        {
            _mainform = mainform;
            _tb = _mainform.fastColoredTextBox;
            _TextBoxHelper = textboxHelper;
            _ThemeHelper = themeHelper;
        }

        public CodeSnippet StateSnippet
        {
            get
            {
                return _TextBoxHelper.StateSnippet;
            }
        }

        protected string SelectedText
        {
            get
            {
                return _tb.SelectedText;
            }
            set
            {
                _tb.SelectedText = value;
            }
        }

        public void Paste()
        {
            try
            {
                if (Clipboard.ContainsText() && Clipboard.ContainsImage())
                {
                    Paste_TextImage();
                    return;
                }
                if (Clipboard.ContainsImage())
                {
                    Paste_Image();
                    return;
                }
                if (Clipboard.ContainsFileDropList())
                {
                    Paste_FileDropList();
                    return;
                }
                if (Clipboard.ContainsText())
                {
                    Paste_Text();
                    return;
                }
            }
            catch
            { }
        }

        protected virtual void Paste_TextImage()
        {
            string _text = Clipboard.GetText();
            this.SelectedText = _text;
        }

        protected virtual void Paste_Image()
        {
            Image _image = Clipboard.GetImage();
            string _id = _mainform._treeHelper.AddImageNode(_mainform._treeHelper.SelectedNode, _image);
            SelectedText = $"#[{_id}]#";
        }

        protected virtual void Paste_FileDropList()
        {
            List<string> items = new List<string>();
            foreach (string s in Clipboard.GetFileDropList())
            {
                items.Add(s);
            }
            if (items.Count > 0)
            {
                TreeNode _parentNode = CodeLib.Instance.TreeNodes.Get(StateSnippet.Id);
                List<string> _ids = _mainform._treeHelper.AddFiles(_parentNode, items.ToArray(), false);
                StringBuilder _sb = new StringBuilder();

                foreach (string _id in _ids)
                {
                    _sb.AppendLine($"#[{_id}]#\r\n");
                }
                SelectedText = _sb.ToString();
            }
        }

        protected virtual void Paste_Text()
        {
            _tb.Paste();
        }

        public void Paste_CtrlShift()
        {
            try
            {
                if (Clipboard.ContainsText() && Clipboard.ContainsImage())
                {
                    Paste_CtrlShift_TextImage();
                    return;
                }
                if (Clipboard.ContainsImage())
                {
                    Paste_CtrlShift_Image();
                    return;
                }
                if (Clipboard.ContainsFileDropList())
                {
                    Paste_CtrlShift_FileDropList();
                    return;
                }
                if (Clipboard.ContainsText())
                {
                    Paste_CtrlShift_Text();
                    return;
                }
            }
            catch
            {
            }
        }

        protected virtual void Paste_CtrlShift_TextImage()
        {
            string _text = Clipboard.GetText();
            this.SelectedText = _text;
        }

        protected virtual void Paste_CtrlShift_Image()
        {
            Image _image = Clipboard.GetImage();
            string _base64 = Convert.ToBase64String(_image.ConvertImageToByteArray());
            SelectedText = _base64;
        }

        protected virtual void Paste_CtrlShift_FileDropList()
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
                        SelectedText = _base64;
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

        protected virtual void Paste_CtrlShift_Text()
        {
            _tb.Paste();
        }

        public void Paste_CtrlAltShift()
        {
            try
            {
                if (Clipboard.ContainsText() && Clipboard.ContainsImage())
                {
                    Paste_CtrlAltShift_TextImage();
                    return;
                }
                if (Clipboard.ContainsImage())
                {
                    Paste_CtrlAltShift_Image();
                    return;
                }
                if (Clipboard.ContainsFileDropList())
                {
                    Paste_CtrlAltShift_FileDropList();
                    return;
                }
                if (Clipboard.ContainsText())
                {
                    Paste_CtrlAltShift_Text();
                    return;
                }
            }
            catch
            {
            }
        }


        protected virtual void Paste_CtrlAltShift_TextImage()
        {
            string _text = Clipboard.GetText();
            this.SelectedText = _text;
        }

        protected virtual void Paste_CtrlAltShift_Image()
        {
            Image _image = Clipboard.GetImage();
            string _base64 = Convert.ToBase64String(_image.ConvertImageToByteArray());
            SelectedText = _base64;
        }

        protected virtual void Paste_CtrlAltShift_FileDropList()
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
                        SelectedText = _base64;
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

        protected virtual void Paste_CtrlAltShift_Text()
        {
            _tb.Paste();
        }


    }
}