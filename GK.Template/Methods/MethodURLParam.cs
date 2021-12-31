using GK.Template.Attributes;
using System;
using System.ComponentModel;
using System.Linq;

namespace GK.Template.Methods
{
    [Category("Logic")]
    [FormatMethod(Name = "GetUrlParam", Aliasses = "UrlParam",
        Example = "{0:GetURLParam(\"Id\")}")]
    [Description("Gets a specific url param value")]
    public sealed class MethodGetURLParam : MethodBase
    {
        [FormatMethodParameter(Optional = false, Order = 0)]
        public string Param { get; set; }

        public override string Apply(string value)
        {
            Uri _uri = new Uri(value);
            string[] _items = _uri.Query?.TrimStart(new char[] { '?' }).Split(new char[] { '&' });
            string _result = _items?.Where(s => s.StartsWith($"{Param}=")).Select(s => s.Substring(Param.Length + 1, s.Length - (Param.Length + 1))).FirstOrDefault()?.Trim();

            if (string.IsNullOrEmpty(_result))
            {
                return string.Empty;
            }

            return _result;
        }
    }
}