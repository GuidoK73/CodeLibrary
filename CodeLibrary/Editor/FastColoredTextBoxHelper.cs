using CodeLibrary.Core;
using CodeLibrary.Extensions;
using CodeLibrary.Helpers;
using FastColoredTextBoxNS;
using GK.Template;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace CodeLibrary.Editor
{
    public class FastColoredTextBoxHelper : ITextBoxHelper
    {
        private readonly FormCodeLibrary _mainform;
        private readonly FastColoredTextBox _tb;
        private readonly TextBoxHelper _TextBoxHelper;
        private readonly ThemeHelper _ThemeHelper;
        private Idle _Idle = new Idle(new TimeSpan(0, 0, 2));
        private Regex _regexWildCards = new Regex("(?<=#\\[)(.*?)(?=\\]#)");
        private CodeSnippet _StateSnippet;
        private bool _supressTextChanged = false;

        public FastColoredTextBoxHelper(FormCodeLibrary mainform, TextBoxHelper textboxHelper, ThemeHelper themeHelper)
        {
            _mainform = mainform;
            _tb = _mainform.fastColoredTextBox;
            _TextBoxHelper = textboxHelper;
            _ThemeHelper = themeHelper;

            CodeInsight.Instance.Init(_mainform.listBoxInsight, _tb);
            _tb.AllowDrop = true;
            _tb.DragDrop += FastColoredTextBox_DragDrop;
            _tb.DragOver += FastColoredTextBox_DragOver;
            _tb.TextChanged += new System.EventHandler<TextChangedEventArgs>(TbCode_TextChanged);
            _tb.SelectionChanged += _tb_SelectionChanged;
            _tb.SelectionChangedDelayed += _tb_SelectionChangedDelayed;
            _tb.KeyDown += new KeyEventHandler(TbCode_KeyDown);
            _tb.MouseUp += new MouseEventHandler(TbCode_MouseUp);
            _tb.PasteOther += _tb_PasteOther;
        }


        public ITextEditor Editor
        {
            get
            {
                return _tb;
            }
        }

        public FastColoredTextBox FastColoredTextBox
        {
            get
            {
                return _tb;
            }
        }

        public bool IsIdle => _Idle;

        public string SelectedText
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

        public string Text
        {
            get
            {
                return _tb.Text;
            }
            set
            {
                _tb.Text = value;
            }
        }

        public void ApplySnippetSettings()
        {
            if (_StateSnippet == null)
            {
                return;
            }

            _tb.WordWrap = _StateSnippet.Wordwrap;

            _mainform.mnuHTMLPreview.Checked = _StateSnippet.HtmlPreview;
            _mainform.splitContainerCode.Panel2Collapsed = !_StateSnippet.HtmlPreview;

            UpdateHtmlPreview();
        }

        public void BringToFront() => _tb.BringToFront();

        public void Copy()
        {
            _mainform.textBoxClipboard.Text = SelectedText;
            if (!string.IsNullOrEmpty(_mainform.textBoxClipboard.Text))
                Clipboard.SetText(_mainform.textBoxClipboard.Text, TextDataFormat.Text);
            else
                Clipboard.Clear();
        }

        public void CopyWithMarkup()
        {
            _tb.Copy();
        }

        public string CurrentLine()
        {
            Range _line = _tb.GetLine(_tb.Selection.Start.iLine);
            return _line.Text;
        }

        public void Cut()
        {
            _mainform.textBoxClipboard.Text = SelectedText;
            SelectedText = string.Empty;
            if (!string.IsNullOrEmpty(_mainform.textBoxClipboard.Text))
                Clipboard.SetText(_mainform.textBoxClipboard.Text, TextDataFormat.Text);
            else
                Clipboard.Clear();
        }

        public void CutWithMarkup() => _tb.Cut();

        public void Focus() => _tb.Focus();

        public CodeSnippet GetStateSnippet() => _StateSnippet;

        public void GotoLine() => _tb.GotoLine();

        public void GotoLine(int line) => _tb.GotoLine(line);

        public string Merge(string text, CodeType targetType)
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
                        _text = SnippetToText(_snippet, targetType);
                    }
                    else if (_snippet == null)
                    {
                        // try get by pattern.
                        var _snippets = CodeLib.Instance.CodeSnippets.GetChildsByPathAndPattern(match.Value);
                        StringBuilder _sb = new StringBuilder();
                        foreach (CodeSnippet snippet in _snippets)
                        {
                            _sb.Append(SnippetToText(snippet, targetType));
                        }
                        _text = _sb.ToString();
                    }
                    else
                    {
                        _text = SnippetToText(_snippet, targetType);
                    }

                    _newText = _newText.Replace($"#[{match.Value}]#", _text);
                }

                _matches = _regexWildCards.Matches(_newText);
            }

            return _newText;
        }

        public string Merge()
        {
            return Merge(Text, _StateSnippet.CodeType);
        }

        public void PasteAdvanced()
        {
            if (Clipboard.ContainsImage())
            {
                Image _image = Clipboard.GetImage();

                string _base64 = Convert.ToBase64String(_image.ConvertImageToByteArray());
                switch (_StateSnippet.CodeType)
                {
                    case CodeType.MarkDown:
                        SelectedText = string.Format(@"![{0}](data:image/png;base64,{1})", _StateSnippet.GetPath(), _base64);
                        break;

                    case CodeType.HTML:
                        SelectedText = string.Format(@"<img src=""data:image/png;base64,{0}"" />", _base64);
                        break;

                    default:
                        SelectedText = _base64;
                        break;
                }
            }
            else if (Clipboard.ContainsFileDropList())
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
                            switch (_StateSnippet.CodeType)
                            {
                                case CodeType.MarkDown:
                                    _sb.AppendLine(string.Format(@"![{0}](data:image/png;base64,{1})", _StateSnippet.GetPath(), _base64));
                                    _sb.AppendLine();
                                    break;

                                case CodeType.HTML:
                                    _sb.AppendLine(string.Format(@"<img src=""data:image/png;base64,{0}"" />", _base64));
                                    _sb.AppendLine();
                                    break;

                                default:
                                    SelectedText = _base64;
                                    break;
                            }
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
                            _sb.AppendLine(string.Format("\r\n~~~{0}\r\n{1}\r\n~~~\r\n", CodeTypeToString(_codeType), _text));
                            _sb.AppendLine();
                            break;

                        case CodeType.System:
                        case CodeType.UnSuported:
                            break;
                    }
                }
                SelectedText = _sb.ToString();
            }
            else if (Clipboard.ContainsText())
            {
                _tb.Paste();
            }
        }

        public void Paste()
        {
            if (Clipboard.ContainsImage())
            {
                Image _image = Clipboard.GetImage();

                string _id = _mainform._treeHelper.AddImageNode(_mainform._treeHelper.SelectedNode, _image);

                this.SelectedText = $"#[{_id}]#";
            }
            else if (Clipboard.ContainsFileDropList())
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
                    this.SelectedText = _sb.ToString();
                }
            }
            else if (Clipboard.ContainsText())
            {
                _tb.Paste();
            }
        }

        private void _tb_PasteOther(object sender, EventArgs e)
        {
            Paste();
        }

        public void RefreshEditor()
        {
            string _text = _tb.Text;
            _tb.Clear();
            _tb.ClearUndo();
            _tb.Text = _text;
        }

        public bool SaveState()
        {
            bool _result = false;

            if (_StateSnippet == null)
            {
                return false;
            }
            _StateSnippet.SetCode(_tb.Text, out _result);

            _StateSnippet.Wordwrap = _tb.WordWrap;
            _StateSnippet.CurrentLine = _tb.CurrentLineNumber();
            if (_result)
            {
                _StateSnippet.CodeLastModificationDate = DateTime.Now;
            }
            return _result;
        }

        public void SelectAll() => _tb.SelectAll();

        public void SelectLine()
        {
            Range _line = _tb.GetLine(_tb.Selection.Start.iLine);
            Place _start = new Place(0, _tb.Selection.Start.iLine);
            Place _end = new Place(_line.End.iChar, _tb.Selection.Start.iLine);
            _tb.Selection = new Range(_tb, _start, _end);
        }

        public void SetState(CodeSnippet snippet)
        {
            _StateSnippet = snippet;
            _supressTextChanged = true;
            _tb.BeginUpdate();

            _TextBoxHelper.SetEditorView(snippet);

            string _text = snippet.GetCode();
            _tb.Text = !string.IsNullOrEmpty(_text) ? _text : "";
            _tb.ClearUndo();
            _mainform.tbPath.Text = snippet.GetPath();// + $"    [C: {snippet.CreationDate},M:{snippet.CodeLastModificationDate:yyyy-MM-dd HH:mm:ss}]";
            _tb.WordWrap = snippet.Wordwrap;
            _tb.SelectionStart = 0;
            _tb.SelectionLength = 0;
            _tb.ScrollControlIntoView(_tb);

            int _lines = _tb.LinesCount;
            try
            {
                if (_lines > snippet.CurrentLine)
                    _tb.GotoLine(snippet.CurrentLine);
            }
            catch { }

            _mainform.mnuWordwrap.Checked = snippet.Wordwrap;
            _mainform.mnuHTMLPreview.Checked = snippet.HtmlPreview;

            _tb.EndUpdate();

            _supressTextChanged = false;
        }

        public void ShowFindDialog() => _tb.ShowFindDialog();

        public void ShowReplaceDialog() => _tb.ShowReplaceDialog();

        public void SwitchHtmlPreview()
        {
            if (_StateSnippet == null)
                return;

            _StateSnippet.HtmlPreview = !_StateSnippet.HtmlPreview;
            _mainform.mnuHTMLPreview.Checked = _StateSnippet.HtmlPreview;
            _mainform.splitContainerCode.Panel2Collapsed = !_StateSnippet.HtmlPreview;
        }

        public bool SwitchWordWrap()
        {
            if (_StateSnippet == null)
                return false;

            _StateSnippet.Wordwrap = !_StateSnippet.Wordwrap;
            _tb.WordWrap = _StateSnippet.Wordwrap;
            _mainform.mnuWordwrap.Checked = _StateSnippet.Wordwrap;

            return _StateSnippet.Wordwrap;
        }

        public void UpdateHtmlPreview()
        {
            _mainform.webBrowser.AllowNavigation = true;

            if (!_mainform.splitContainerCode.Panel2Collapsed)
            {
                if (_StateSnippet.CodeType == CodeType.MarkDown)
                {
                    try
                    {
                        MarkDigWrapper _markdown = new MarkDigWrapper();
                        string _text = Merge(_tb.Text, CodeType.MarkDown);
                        _text = _markdown.Transform(_text);
                        if (Config.Theme != ETheme.Light)
                        {
                            StringBuilder _sb = new StringBuilder();
                            _sb.Append("<body style =\"background-color:#333333;color:#cccccc;font-family:Arial\"></body>\r\n");
                            _sb.Append("<style>");
                            _sb.Append("a:link { color: green; background-color: transparent; text-decoration: none; }");
                            _sb.Append("a:visited { color: lightgreen; background-color: transparent; text-decoration: none; }");
                            _sb.Append("a:hover { color: lightgreen; background-color: transparent; text-decoration: underline; }");
                            _sb.Append("a:active { color: yellow; background-color: transparent; text-decoration: underline; }");
                            _sb.Append("table, th, td { border: 1px solid lightgray; border-collapse: collapse; padding:4px; }");
                            _sb.Append("</style>");
                            _sb.Append(_text);
                            _text = _sb.ToString();

                            _text = _text.Replace("style=\"color:Black;background-color:White;\"", "style=\"color:White;background-color:Black;\"");
                        }
                        else
                        {
                            StringBuilder _sb = new StringBuilder();
                            _sb.Append("<body style =\"font-family:Arial\"></body>\r\n");
                            _sb.Append("<style>");
                            _sb.Append("table, th, td { border: 1px solid black; border-collapse: collapse; padding:4px; }");
                            _sb.Append("</style>");
                            _sb.Append(_text);
                            _text = _sb.ToString();
                        }
                        _mainform.webBrowser.DocumentText = _text;
                    }
                    catch (Exception)
                    {
                        _mainform.webBrowser.DocumentText = Merge(_tb.Text, CodeType.HTML);
                    }
                }
                else
                {
                    _mainform.webBrowser.DocumentText = Merge(_tb.Text, CodeType.HTML);
                }

                _ThemeHelper.SetWebBrowserTheme(Config.Theme);
            }
        }



        private enum ExportType
        {
            Pdf = 0,
            File = 1
        }


        public void CopyHtml()
        {
            if (_StateSnippet.CodeType == CodeType.MarkDown)
            {
                MarkDigWrapper _markdown = new MarkDigWrapper();

                StringBuilder _sb = new StringBuilder();
                //_sb.Append("<style>\r\n");
                //_sb.Append(LocalUtils.SplendorCSS());
                //_sb.Append("</style>\r\n");
                _sb.Append(Merge(_tb.Text, CodeType.MarkDown));
                string _text = _sb.ToString();
                _text = _markdown.Transform(_text);
                Clipboard.SetText(_text);
            }
        }

        public bool ExportToPdfFile()
        {
            return Export(ExportType.Pdf);
        }

        public bool ExportToFile()
        {
            return Export(ExportType.File);
        }


        private bool Export(ExportType exportType)
        {
            string _exportExtension = CodeTypeToString(_StateSnippet.CodeType);
            string _fileName = string.Empty;

            SaveFileDialog _dialog = new SaveFileDialog();

            if (string.IsNullOrEmpty(_StateSnippet.ExportPath))
            {
                if (Directory.Exists(_StateSnippet.ExportPath))
                {
                    _fileName = Path.Combine(_StateSnippet.ExportPath, $"{_StateSnippet.LabelName()}.{_exportExtension}");
                }
                else
                {
                    _fileName = $"{_StateSnippet.LabelName()}.{_exportExtension}";
                }
            }
            else
            {
                _fileName = _StateSnippet.ExportPath;
            }

            _dialog.Filter = $"*.{_exportExtension}|*.{_exportExtension}";
            if (_StateSnippet.CodeType == CodeType.MarkDown)
            {
                _dialog.Filter = $"*.{_exportExtension}|*.{_exportExtension}|*.html|*.html";

            }

            _dialog.FileName = _fileName;
            DialogResult _dlgresult = _dialog.ShowDialog();
            
            if (_dlgresult == DialogResult.Cancel)
            {
                return false;
            }
            _fileName = _dialog.FileName;

            string _text = string.Empty;

            switch (_StateSnippet.CodeType)
            {
                case CodeType.Image:
                case CodeType.RTF:
                case CodeType.ReferenceLink:
                case CodeType.UnSuported:
                case CodeType.System:
                    MessageBox.Show("Error converting to PDF", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;                    
                default:

                    if (_StateSnippet.CodeType == CodeType.MarkDown && _fileName.EndsWith(".html", StringComparison.OrdinalIgnoreCase))
                    {
                        StringBuilder _sb = new StringBuilder();
                        _sb.Append("<style>\r\n");
                        _sb.Append(LocalUtils.SplendorCSS());
                        _sb.Append("</style>\r\n");
                        _sb.Append(Merge(_tb.Text, CodeType.MarkDown));
                        MarkDigWrapper _markdown = new MarkDigWrapper();                        
                        _text = _markdown.Transform(_sb.ToString());
                    }
                    else
                    {
                        _text = Merge(_tb.Text, _StateSnippet.CodeType);
                    }
                    break;

            }
            if (exportType == ExportType.Pdf)
            {
                MarkDigWrapper _markdown = new MarkDigWrapper();
                _markdown.ToPDF(_fileName, _text);
            }
            if (exportType == ExportType.File)
            {
                File.WriteAllText(_fileName, _text);
            }

            _StateSnippet.ExportPath = _fileName;
            return true;
        }

        private void _tb_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                _mainform.lblStart.Text = _tb.SelectionStart.ToString();
                _mainform.lblEnd.Text = (_tb.SelectionStart + _tb.SelectionLength).ToString();
                _mainform.lblLength.Text = _tb.SelectionLength.ToString();
            }
            catch { }
        }

        private void _tb_SelectionChangedDelayed(object sender, EventArgs e)
        {
            if (_tb.SelectionLength == 0)
            {
                return;
            }
            if (_TextBoxHelper.SelectIsCopy)
            {
                Clipboard.SetText(_tb.SelectedText);
            }
        }

        private bool DocShortCut(KeyEventArgs e)
        {
            KeysLanguage _targetLangue = LocalUtils.KeysLanguageForCodeType(_StateSnippet.CodeType);
            var _snippet = CodeLib.Instance.CodeSnippets.GetByShortCut(e.KeyData, _targetLangue).FirstOrDefault();
            if (_snippet == null)
            {
                _snippet = CodeLib.Instance.CodeSnippets.GetByShortCut(e.KeyData, KeysLanguage.AllLanguages).FirstOrDefault();
            }

            if (_snippet != null)
            {
                StringTemplate stringtemplate = new StringTemplate();
                string result = stringtemplate.Format(_snippet.GetCode(), _tb.SelectedText);
                _tb.SelectedText = result;
                _tb.Focus();
                return true;
            }

            return false;
        }



        private void FastColoredTextBox_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop, false))
            {
                string[] filenames = (string[])(e.Data.GetData(DataFormats.FileDrop, false));
                if (filenames.Length >= 1)
                {
                    string _text = File.ReadAllText(filenames[0]);
                    Text = _text;
                }
            }
        }

        private void FastColoredTextBox_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop, false))
            {
                e.Effect = DragDropEffects.Move;
            }
        }

        private string SnippetToText(CodeSnippet snippet, CodeType targetType)
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
                        _result = string.Format("\r\n~~~{0}\r\n{1}\r\n~~~\r\n", CodeTypeToString(snippet.CodeType), snippet.GetCode());
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

        private string CodeTypeToString(CodeType codeType)
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

        private void TbCode_KeyDown(object sender, KeyEventArgs e)
        {
            FastColoredTextBox tb = sender as FastColoredTextBox;

            if (DocShortCut(e))
                return;

            if (e.Control && e.Shift && e.KeyValue == 86)
            {
                PasteAdvanced();
            }

            if (string.IsNullOrEmpty(tb.SelectedText))
                return;

            if (e.KeyValue == 222 && e.Shift)
            {
                tb.SelectedText = string.Format("\"{0}\"", tb.SelectedText);
                e.Handled = true;
                return;
            }
            if ((e.KeyValue == 57 || e.KeyValue == 48) && e.Shift)
            {
                tb.SelectedText = string.Format("({0})", tb.SelectedText);
                e.Handled = true;
                return;
            }
            if ((e.KeyValue == 219 || e.KeyValue == 221) && e.Shift)
            {
                tb.SelectedText = string.Format("{{{0}}}", tb.SelectedText);
                e.Handled = true;
                return;
            }
            if (e.KeyValue == 219 || e.KeyValue == 221)
            {
                tb.SelectedText = string.Format("[{0}]", tb.SelectedText);
                e.Handled = true;
                return;
            }
            if ((e.KeyValue == 188 || e.KeyValue == 190) && e.Shift)
            {
                tb.SelectedText = string.Format("<{0}>", tb.SelectedText);
                e.Handled = true;
                return;
            }
            if (e.KeyValue == 222)
            {
                tb.SelectedText = string.Format("'{0}'", tb.SelectedText);
                e.Handled = true;
                return;
            }
        }

        private void TbCode_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                _mainform.mncEdit.Show(Cursor.Position.X, Cursor.Position.Y);
            }
        }

        private void TbCode_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateHtmlPreview();

            _Idle.Refresh();

            if (_supressTextChanged)
                return;

            CodeLib.Instance.Changed = true;
        }
    }
}