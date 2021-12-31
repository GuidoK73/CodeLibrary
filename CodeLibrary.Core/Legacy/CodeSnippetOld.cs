﻿using System;
using System.Linq;
using System.Runtime.Serialization;
using System.Windows.Forms;

namespace CodeLibrary.Core
{
    [DataContract()]
    public class CodeSnippetOld
    {
        [DataMember(Name = "CreationDate")]
        private string _CreationDate;

        public CodeSnippetOld()
        {
            CreationDate = DateTime.Now.ToString("yyyyMMdd HH:mm:ss");
            Id = Guid.NewGuid().ToString();
        }

        [DataMember(Name = "AlarmActive")]
        public bool AlarmActive { get; set; }

        [DataMember(Name = "AlarmDate")]
        public DateTime? AlarmDate { get; set; }

        [DataMember(Name = "Blob")]
        public byte[] Blob { get; set; }

        [DataMember(Name = "Code")]
        public string Code { get; set; } = string.Empty;

        [DataMember(Name = "CodeLastModificationDate")]
        public DateTime? CodeLastModificationDate { get; set; }

        [DataMember(Name = "CodeType")]
        public CodeType CodeType { get; set; }

        public string CreationDate
        {
            get
            {
                //fix incorrect creation date format
                if (_CreationDate != null && _CreationDate.Contains(":nn"))
                {
                    _CreationDate = _CreationDate.Replace(":nn", ":00:00");
                }
                else if (_CreationDate != null && _CreationDate.Length == "yyyyMMdd HH:mm".Length)
                {
                    _CreationDate = _CreationDate + ":00";
                }
                return _CreationDate;
            }
            set
            {
                if (value != null && value.Contains(":nn"))
                {
                    value = value.Replace(".nn", ".00.00");
                }
                _CreationDate = value;
            }
        }

        [DataMember(Name = "CurrentLine")]
        public int CurrentLine { get; set; }

        [DataMember(Name = "CyclicShortCut")]
        public bool CyclicShortCut { get; set; }

        [DataMember(Name = "DefaultChildCode")]
        public string DefaultChildCode { get; set; } = string.Empty;

        [DataMember(Name = "DefaultChildCodeType")]
        public CodeType DefaultChildCodeType { get; set; }

        [DataMember(Name = "DefaultChildCodeTypeEnabled")]
        public bool DefaultChildCodeTypeEnabled { get; set; }

        [DataMember(Name = "DefaultChildName")]
        public string DefaultChildName { get; set; } = string.Empty;

        [DataMember(Name = "DefaultChildRtf")]
        public string DefaultChildRtf { get; set; } = string.Empty;

        [DataMember(Name = "Expanded")]
        public bool Expanded { get; set; }

        [DataMember(Name = "Flag")]
        public SnippetFlags Flag { get; set; }

        [DataMember(Name = "HtmlPreview")]
        public bool HtmlPreview { get; set; } = false;

        [DataMember(Name = "Id")]
        public string Id { get; set; } = string.Empty;

        [DataMember(Name = "Important")]
        public bool Important { get; set; }

        public bool Locked { get; set; }

        [DataMember(Name = "Name")]
        public string Name { get; set; } = string.Empty;

        [DataMember(Name = "Order")]
        public int Order { get; set; }

        [DataMember(Name = "Path")]
        public string Path { get; set; } = string.Empty;

        [DataMember(Name = "RL")]
        public string ReferenceLinkId { get; set; }

        [DataMember(Name = "RTF")]
        public string RTF { get; set; } = string.Empty;

        [DataMember(Name = "RTFAlwaysWhite")]
        public bool RTFAlwaysWhite { get; set; } = false;

        [DataMember(Name = "RTFLM")]
        public bool RTFOwnTheme { get; set; } = false;

        [DataMember(Name = "RtfTheme")]
        public ETheme RTFTheme { get; set; } = ETheme.Light;

        [DataMember(Name = "ShortCutKeys")]
        public Keys ShortCutKeys { get; set; }

        [DataMember(Name = "Wordwrap")]
        public bool Wordwrap { get; set; }

        public static CodeSnippetOld ClipboardMonitorSnippet() => new CodeSnippetOld() { Code = string.Empty, CodeType = CodeType.System, Name = Constants.CLIPBOARDMONITOR_TITLE, Path = Constants.CLIPBOARDMONITOR_TITLE, Id = Constants.CLIPBOARDMONITOR };

        public static CodeSnippetOld NewRoot(string text, CodeType codetype, string name) => new CodeSnippetOld() { Code = text, CodeType = codetype, Locked = false, Name = name, Path = name };

        public static CodeSnippetOld TrashcanSnippet() => new CodeSnippetOld() { Code = string.Empty, CodeType = CodeType.System, Name = Constants.TRASHCAN_TITLE, Path = Constants.TRASHCAN_TITLE, Id = Constants.TRASHCAN };

        public override int GetHashCode() => Code.GetHashCode() + Path.GetHashCode() + Name.GetHashCode() + Order;

        public string LabelName() => Utils.SplitPath(this.Path).Last();

        public string Title()
        {
            return Path.Split(new char[] { '\\' }).LastOrDefault();
        }

        public override string ToString() => string.Format("{0}", Path);


    }
}