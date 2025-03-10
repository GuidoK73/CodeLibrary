using CodeLibrary.Core;
using CodeLibrary.Core.MarkDownTables;
using CodeLibrary.Helpers;
using DevToys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace CodeLibrary.Editor.EditorLanguageHelpers
{
    public class TemplateEditorClipboardHelper : EditorClipboardHelperBase
    {
        private CSharpUtils _CSharpUtils = new CSharpUtils();


        public TemplateEditorClipboardHelper(FormCodeLibrary mainform, TextBoxHelper textboxHelper, ThemeHelper themeHelper) : base(mainform, textboxHelper, themeHelper)
        {
        }

        private bool Execute()
        {
            string _text = Clipboard.GetText();
            _text = Utils.TrimText(_text, "\r\n");

            // ConnectionString
            {
                List<string[]> _connItems = _CSharpUtils.GetConnectionStringValues(_text, out bool isConnectionString);
                if (isConnectionString)
                {
                    StringBuilder _sb = new StringBuilder();
                    foreach (string[] item in _connItems)
                    {
                        _sb.AppendLine($"{item[0]};{item[1]}");
                    }
                    this.SelectedText = _sb.ToString();
                    return true;
                }
            }


            // CSV To Comma seperated
            {
                char _separator = ' ';
                string _data = string.Empty;
                bool _isCsv = CsvUtils.GetCsvSeparator(_text, out _separator);
                if (_isCsv)
                {
                    string _reorderString = null;
                    if (Core.Utils.isReorderString(SelectedText))
                    {
                        _reorderString = SelectedText;
                    }

                    _data = CsvUtils.CsvChange(_text, _separator, ';', _reorderString);
                    this.SelectedText = _data;
                    return true;
                }
            }

            // C# class to Comma seperated
            {
                List<string[]> _classProps = _CSharpUtils.GetProperties(_text, out bool isClass);
                if (isClass)
                {
                    StringBuilder _sb = new StringBuilder();
                    foreach (string[] item in _classProps)
                    {
                        _sb.AppendLine($"{item[0]};{item[1]}");
                    }
                    this.SelectedText = _sb.ToString();
                    return true;
                }
            }

            // C# enum to Comma seperated
            {
                List<string[]> _enumValues = _CSharpUtils.GetEnumValues(_text, out bool isEnum);
                if (isEnum)
                {
                    StringBuilder _sb = new StringBuilder();
                    foreach (string[] item in _enumValues)
                    {
                        _sb.AppendLine($"{item[0]};{item[1]}");
                    }
                    this.SelectedText = _sb.ToString();
                    return true;
                }
            }

            // switch case values
            {
                List<string> _caseValues = _CSharpUtils.GetSwitchCaseValues(_text, out bool isCase);
                if (isCase)
                {
                    StringBuilder _sb = new StringBuilder();
                    foreach (string item in _caseValues)
                    {
                        _sb.AppendLine($"{item};");
                    }
                    this.SelectedText = _sb.ToString();
                    return true;
                }
            }

            // Url
            { 
                if (IsUrl(_text))
                {
                    Url _url = _text;
                    StringBuilder _sb = new StringBuilder();
                    _sb.AppendLine(_url.Base);

                    foreach (string item in _url.Path)
                    {
                        _sb.AppendLine(item);
                    }
                    foreach (KeyValuePair<string, string> item in _url.Query.ToList())
                    {
                        _sb.AppendLine($"{item.Key};{item.Value}");
                    }
                    this.SelectedText = _sb.ToString();
                    return true;
                }
            }

            {
                // Markdown Table
                StringBuilder _sbLine = new StringBuilder();
                StringBuilder _sb = new StringBuilder();
                MDTabify _mdTable = new MDTabify();
                List<string[]> _data = _mdTable.MarkDownTableToArray(_text);
                foreach (string[] item in _data)
                {
                    foreach (var cell in item)
                    {
                        _sbLine.Append(cell.Trim());
                        _sbLine.Append(";");
                    }
                    _sbLine.Length--;                   
                    _sb.AppendLine(_sbLine.ToString().Trim(new char[] { ';' }));
                    _sbLine.Clear();
                }
                this.SelectedText = _sb.ToString();
                return true;
            }

            return false;
        }

        private bool IsUrl(string text)
        {
            if (text.Contains("\r\n"))
            {
                return false;
            }
            if (text.StartsWith("http://"))
            {
                return true;
            }
            if (text.StartsWith("https://"))
            {
                return true;
            }
            return false;
        }


        protected override void Paste_CtrlShift_Text()
        {
            bool _succes = Execute();
            if (_succes)
                return;

            base.Paste_CtrlShift_Text();
        }

        protected override void Paste_CtrlShift_TextImage()
        {
            bool _succes = Execute();
            if (_succes)
                return;

            base.Paste_CtrlShift_TextImage();
        }
    }
}