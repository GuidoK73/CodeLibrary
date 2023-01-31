﻿using System;
using System.Linq;
using System.Runtime.Serialization;
using System.Windows.Forms;

namespace CodeLibrary.Core
{
    [Serializable()]
    [DataContract()]
    public class CodeSnippet
    {
        [IgnoreDataMember]
        private string CacheRTF { get; set; } = null;

        [IgnoreDataMember]
        private string CacheCode { get; set; } = null;

        [IgnoreDataMember]
        private string CachePath { get; set; } = null;

        [IgnoreDataMember]
        private string CacheDefaultChildRtf { get; set; } = null;

        [IgnoreDataMember]
        private string CacheDefaultChildCode { get; set; } = null;

        [IgnoreDataMember]
        private string CacheExportPath { get; set; } = null;




        [DataMember(Name = "CD")]
        public string CreationDate { get; set; }


        [DataMember(Name = "AA")]
        public bool AlarmActive { get; set; }

        [DataMember(Name = "AD")]
        public DateTime? AlarmDate { get; set; }

        [DataMember(Name = "BL")]
        public byte[] Blob { get; set; }

        // Do not use directly, use GetCode() SetCode() instead.
        [DataMember(Name = "C")]
        internal string Code { get; set; } = string.Empty; // ENCODED

        [DataMember(Name = "CLMD")]
        public DateTime? CodeLastModificationDate { get; set; }

        [DataMember(Name = "CT")]
        public CodeType CodeType { get; set; }

        [DataMember(Name = "CL")]
        public int CurrentLine { get; set; }

        // Do not use directly, use GetDefaultChildCode() SetDefaultChildCode() instead.
        [DataMember(Name = "DCC")]
        internal string DefaultChildCode { get; set; } = string.Empty; //  Encoded

        [DataMember(Name = "DCCT")]
        public CodeType DefaultChildCodeType { get; set; }

        [DataMember(Name = "DCCTE")]
        public bool DefaultChildCodeTypeEnabled { get; set; }

        [DataMember(Name = "DCN")]
        public string DefaultChildName { get; set; } = string.Empty;

        // Do not use directly, use GetDefaultChildRtf() SetDefaultChildRtf() instead.
        [DataMember(Name = "DCRTF")]
        internal string DefaultChildRtf { get; set; } = string.Empty; // Encoded

        [DataMember(Name = "EXP")]
        public bool Expanded { get; set; }

        [DataMember(Name = "FL")]
        public SnippetFlags Flag { get; set; }

        [DataMember(Name = "HTMLP")]
        public bool HtmlPreview { get; set; } = false;

        [DataMember(Name = "Id")]
        public string Id { get; set; } = string.Empty;

        [DataMember(Name = "IMP")]
        public bool Important { get; set; }

        [DataMember(Name = "LCK")]
        public bool Locked { get; set; }

        [DataMember(Name = "Name")]
        public string Name { get; set; } = string.Empty;

        [DataMember(Name = "Ord")]
        public int Order { get; set; }

        // Do not use directly, use GetPath() SetPath() instead.
        [DataMember(Name = "Path")]
        internal string Path { get; set; } = string.Empty; // ENCODED


        // Do not use directly, use GetPath() SetPath() instead.
        [DataMember(Name = "EPath")]
        public string ExportPath { get; set; } = string.Empty; // ENCODED


        [DataMember(Name = "RL")]
        public string ReferenceLinkId { get; set; }

        // Do not use directly, use GetRtf() SetRtf() instead.
        [DataMember(Name = "RTF")]
        internal string RTF { get; set; } = string.Empty; // ENCODED

        [DataMember(Name = "RTFAW")]
        public bool RTFAlwaysWhite { get; set; } = false;

        [DataMember(Name = "RTFLM")]
        public bool RTFOwnTheme { get; set; } = false;

        [DataMember(Name = "RtfThm")]
        public ETheme RTFTheme { get; set; } = ETheme.Light;

        [DataMember(Name = "IEM")]
        public EImageEmbedAlign ImageEmbedAlign { get; set; } = EImageEmbedAlign.None;

        

        [DataMember(Name = "SCKS")]
        public Keys ShortCutKeys { get; set; }

        [DataMember(Name = "SCKSL")]
        public KeysLanguage ShortCutKeyLanguage { get; set; }

        [DataMember(Name = "WW")]
        public bool Wordwrap { get; set; }

        public override int GetHashCode() => Code.GetHashCode() + Path.GetHashCode() + Name.GetHashCode() + Order;

        public string LabelName() => GetPath().Split(new char[] { '\\' }).Last();

        public string Title()
        {
            return GetPath().Split(new char[] { '\\' }).LastOrDefault();
        }

        public override string ToString() => string.Format("{0}", GetPath());

        public static CodeSnippet ClipboardMonitorSnippet()
        {
            var _snip = new CodeSnippet() { CodeType = CodeType.System, Name = Constants.CLIPBOARDMONITOR_TITLE, Id = Constants.CLIPBOARDMONITOR };
            _snip.SetCode("", out bool _changed);
            _snip.SetRtf("", out _changed);
            _snip.SetPath(Constants.CLIPBOARDMONITOR_TITLE, out _changed);
            return _snip;
        }

        public static CodeSnippet NewRoot(string text, CodeType codetype, string name)
        {
            var _snip = new CodeSnippet() { CodeType = codetype, Locked = false, Name = name };
            _snip.SetCode(text, out bool _changed);
            _snip.SetRtf("", out _changed);
            _snip.SetPath(name, out _changed);
            return _snip;
        }

        public static CodeSnippet TrashcanSnippet()
        {
            var _snip = new CodeSnippet() { Code = string.Empty, CodeType = CodeType.System, Id = Constants.TRASHCAN, Name = Constants.TRASHCAN_TITLE };
            _snip.SetPath(Constants.TRASHCAN_TITLE, out bool _changed);
            return _snip;
        }

        public CodeSnippet()
        {
            CreationDate = DateTime.Now.ToString("yyyyMMdd HH:mm:ss");
            Id = Guid.NewGuid().ToString();
            Name = Id;
        }

        public CodeSnippet(string code, string rtf, string path) : this()
        {
            bool _changed;
            SetCode(code, out _changed);
            SetRtf(rtf, out _changed);
            SetPath(path, out _changed);
        }

        public CodeSnippet Clone()
        {
            CodeSnippet _clone = new CodeSnippet();
            _clone.AlarmActive = AlarmActive;
            _clone.AlarmDate = AlarmDate;
            if (Blob != null)
            {
                _clone.Blob = new Byte[Blob.Length];
                Blob.CopyTo(_clone.Blob, 0);
            }
            _clone.CacheCode = null;
            _clone.CacheDefaultChildCode = null;
            _clone.CacheDefaultChildRtf = null;
            _clone.CachePath = null;
            _clone.CacheRTF = null;
            _clone.Code = Code;
            _clone.CodeLastModificationDate = CodeLastModificationDate;
            _clone.CodeType = CodeType;
            _clone.CreationDate = CreationDate;
            _clone.CurrentLine = CurrentLine;
            _clone.DefaultChildCode = DefaultChildCode;
            _clone.DefaultChildCodeType = DefaultChildCodeType;
            _clone.DefaultChildCodeTypeEnabled = DefaultChildCodeTypeEnabled;
            _clone.DefaultChildName = DefaultChildName;
            _clone.DefaultChildRtf = DefaultChildRtf;
            _clone.Expanded = Expanded;
            _clone.Flag = Flag;
            _clone.HtmlPreview = HtmlPreview;
            _clone.Id = Guid.NewGuid().ToString();
            _clone.Name = _clone.Id;
            _clone.Important = Important;
            _clone.Locked = Locked;
            _clone.Order = Order;
            _clone.Path = Path;
            _clone.ExportPath = ExportPath;
            _clone.ReferenceLinkId = ReferenceLinkId;
            _clone.RTF = RTF;
            _clone.RTFAlwaysWhite = RTFAlwaysWhite;
            _clone.RTFOwnTheme = RTFOwnTheme;
            _clone.RTFTheme = RTFTheme;
            _clone.ShortCutKeys = ShortCutKeys;
            _clone.Wordwrap = Wordwrap;
            return _clone;
        }

        public void SetCode(string value, out bool changed)
        {
            if (string.IsNullOrEmpty(value))
            {
                value = string.Empty;
            }
            changed = !value.Equals(CacheCode);
            if (string.IsNullOrEmpty(value))
            {
                Code = string.Empty;
                CacheCode = string.Empty;
                return;
            }
            Code = Utils.CompressString(value);
            CacheCode = value;
        }


        public void SetDefaultChildCode(string value, out bool changed)
        {
            if (string.IsNullOrEmpty(value))
            {
                value = string.Empty;
            }
            changed = !value.Equals(CacheCode);
            if (string.IsNullOrEmpty(value))
            {
                DefaultChildCode = string.Empty;
                CacheDefaultChildCode = string.Empty;
                return;
            }
            DefaultChildCode = Utils.CompressString(value);
            CacheDefaultChildCode = value;
        }

        public void SetPath(string value, out bool changed)
        {
            if (string.IsNullOrEmpty(value))
            {
                value = string.Empty;
            }

            changed = !value.Equals(CachePath);
            Path = Utils.ToBase64(value);
            CachePath = value;
        }


        public void SetRtf(string value, out bool changed)
        {
            if (string.IsNullOrEmpty(value))
            {
                value = string.Empty;
            }

            changed = false;
            if (!string.IsNullOrEmpty(value))
            {
                try
                {
                    if (string.IsNullOrEmpty(value))
                    {
                        RTF = string.Empty;
                        CacheRTF = string.Empty;
                        return;
                    }


                    changed = !value.Equals(CacheRTF);
                    RTF = Utils.CompressString(value);
                    CacheRTF = value;
                }
                catch { }
            }
        }


        public void SetDefaultChildRtf(string value, out bool changed)
        {
            if (string.IsNullOrEmpty(value))
            {
                value = string.Empty;
            }

            changed = false;
            if (!string.IsNullOrEmpty(value))
            {
                try
                {
                    if (string.IsNullOrEmpty(value))
                    {
                        DefaultChildRtf = string.Empty;
                        CacheDefaultChildRtf = string.Empty;
                        return;
                    }


                    changed = !value.Equals(CacheDefaultChildRtf);
                    DefaultChildRtf = Utils.CompressString(value);
                    CacheDefaultChildRtf = value;
                }
                catch { }
            }
        }


        public void Refresh()
        {
            GetCode();
            GetPath();
            GetRTF();
        }


        public string GetCode()
        {
            if (string.IsNullOrEmpty(CacheCode))
            {
                CacheCode = string.Empty;
                if (!string.IsNullOrEmpty(Code))
                {
                    CacheCode = Utils.DecompressString(Code);
                }
            }
            return CacheCode;
        }

        public string GetDefaultChildCode()
        {
            if (string.IsNullOrEmpty(CacheDefaultChildCode))
            {
                CacheDefaultChildCode = string.Empty;
                if (!string.IsNullOrEmpty(DefaultChildCode))
                {
                    CacheDefaultChildCode = Utils.DecompressString(DefaultChildCode);
                }
            }
            return CacheDefaultChildCode;
        }


        public string GetPath()
        {
            if (string.IsNullOrEmpty(CachePath))
            {
                CachePath = string.Empty;
                if (!string.IsNullOrEmpty(Path))
                {
                    CachePath = Utils.FromBase64(Path);
                }
            }
            return CachePath;
        }

        public string GetParentPath() => Utils.ParentPath(GetPath());
  

        public string GetRTF()
        {
            if (string.IsNullOrEmpty(CacheRTF))
            {
                CacheRTF = string.Empty;
                if (!string.IsNullOrEmpty(RTF))
                {
                    try
                    {
                        CacheRTF = Utils.DecompressString(RTF);
                    }
                    catch
                    { }
                }
            }

            return CacheRTF;
        }

        public string GetDefaultChildRtf()
        {
            if (string.IsNullOrEmpty(CacheDefaultChildRtf))
            {
                CacheDefaultChildRtf = string.Empty;
                if (!string.IsNullOrEmpty(DefaultChildRtf))
                {
                    try
                    {
                        CacheDefaultChildRtf = Utils.DecompressString(DefaultChildRtf);
                    }
                    catch
                    { }
                }
            }
            return CacheDefaultChildRtf;
        }
    }
}