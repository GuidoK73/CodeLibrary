using CodeLibrary.PluginPack.Common;
using DevToys;
using EditorPlugins.Core;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace CodeLibrary.PluginPack
{
    [Description("Url Break Down")]
    public class UrlBreakDown : IEditorPlugin
    {
        public string Category => "Formatting";
        public string DisplayName => "Url Break Down";
        public Guid Id => Guid.Parse("e7583d69-2b0e-4d8a-b22a-4c41b7e83025");
        public Image Image => null;
        public bool OmitResult => false;
        public Keys ShortcutKeys => Keys.None;

        public bool IsExtension => false;

        public void Apply(ISelInfo sel)
        {
            string _text = sel.SelectedText;
            Url _url = sel.SelectedText;
            StringBuilder _sb = new StringBuilder();
            _sb.AppendLine(_url.PathString);
            foreach (var item in _url.Query)
            {
                _sb.AppendLine($"{item.Key}={item.Value}");
            }
        }

        public bool Configure()
        {
            return false;
        }
    }
}