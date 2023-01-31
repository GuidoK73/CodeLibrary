using DevToys;
using EditorPlugins.Core;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace CodeLibrary.PluginPack
{
    [Description("Breaks down Url")]
    public class UrlBreakDown : IEditorPlugin
    {
        public bool IsExtension => false;
        public string Category => "";
        public string DisplayName => "Url Break Down";
        public Guid Id => Guid.Parse("2ca61b26-4266-43a4-8b28-df9d37d04a6e");
        public Image Image => ImageResource.reverse;
        public bool OmitResult => false;
        public Keys ShortcutKeys => Keys.None;

        public void Apply(ISelInfo sel)
        {
            UrlStringBuilder _usb = new UrlStringBuilder(sel.SelectedText);

            StringBuilder _sb = new StringBuilder();

            _sb.AppendLine(_usb.BaseUrl);
            _sb.AppendLine(_usb.PathString());

            foreach(var key in _usb.Query.Keys)
            {
                _sb.AppendLine($"{key}={_usb.Query[key]}" );
            }

            sel.SelectedText = _sb.ToString(); 
        }

        public bool Configure()
        {
            return false;
        }
    }
}