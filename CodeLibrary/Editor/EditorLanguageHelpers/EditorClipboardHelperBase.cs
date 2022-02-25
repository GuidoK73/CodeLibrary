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
        protected CodeSnippet _StateSnippet;

        public EditorClipboardHelperBase(FormCodeLibrary mainform, TextBoxHelper textboxHelper, ThemeHelper themeHelper)
        {
            _mainform = mainform;
            _tb = _mainform.fastColoredTextBox;
            _TextBoxHelper = textboxHelper;
            _ThemeHelper = themeHelper;
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

        public void SetState(CodeSnippet snippet)
        {
            _StateSnippet = snippet;
        }

        public void Paste()
        {
            try
            {
                if (Clipboard.ContainsText() && Clipboard.ContainsImage())
                {
                    PasteTextImage();
                    return;
                }
                if (Clipboard.ContainsImage())
                {
                    PasteImage();
                    return;
                }
                if (Clipboard.ContainsFileDropList())
                {
                    PasteFileDropList();
                    return;
                }
                if (Clipboard.ContainsText())
                {
                    PasteText();
                    return;
                }
            }
            catch 
            { }
        }

        protected virtual void PasteTextImage()
        {
            string _text = Clipboard.GetText();
            this.SelectedText = _text;
        }

        protected virtual void PasteImage()
        {
            Image _image = Clipboard.GetImage();
            string _id = _mainform._treeHelper.AddImageNode(_mainform._treeHelper.SelectedNode, _image);
            SelectedText = $"#[{_id}]#";
        }

        protected virtual void PasteFileDropList()
        {
            List<string> items = new List<string>();
            foreach (string s in Clipboard.GetFileDropList())
            {
                items.Add(s);
            }
            if (items.Count > 0)
            {
                TreeNode _parentNode = CodeLib.Instance.TreeNodes.Get(_StateSnippet.Id);
                List<string> _ids = _mainform._treeHelper.AddFiles(_parentNode, items.ToArray(), false);
                StringBuilder _sb = new StringBuilder();

                foreach (string _id in _ids)
                {
                    _sb.AppendLine($"#[{_id}]#\r\n");
                }
                SelectedText = _sb.ToString();
            }
        }

        protected virtual void PasteText()
        {
            _tb.Paste();
        }

        public void PasteAdvanced()
        {
            try
            {
                if (Clipboard.ContainsText() && Clipboard.ContainsImage())
                {
                    PasteAdvancedTextImage();
                    return;
                }
                if (Clipboard.ContainsImage())
                {
                    PasteAdvancedImage();
                    return;
                }
                if (Clipboard.ContainsFileDropList())
                {
                    PasteAdvancedFileDropList();
                    return;
                }
                if (Clipboard.ContainsText())
                {
                    PasteAdvancedText();
                    return;
                }
            }
            catch
            {

            }
        }

        protected virtual void PasteAdvancedTextImage()
        {
            string _text = Clipboard.GetText();
            this.SelectedText = _text;
        }

        protected virtual void PasteAdvancedImage()
        {
            Image _image = Clipboard.GetImage();
            string _base64 = Convert.ToBase64String(_image.ConvertImageToByteArray());
            SelectedText = _base64;
        }

        protected virtual void PasteAdvancedFileDropList()
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

        protected virtual void PasteAdvancedText()
        {
            _tb.Paste();
        }
    }
}