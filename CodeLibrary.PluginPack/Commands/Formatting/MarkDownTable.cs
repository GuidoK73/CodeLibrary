using CodeLibrary.Core;
using EditorPlugins.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CodeLibrary.PluginPack.Commands.Formatting
{
    [Description("Markdown Table")]
    public class MarkdownTable : IEditorPlugin
    {
        public bool IsExtension => false;
        public string Category => "Formatting";
        public string DisplayName => "Markdown Table Formatter";
        public Guid Id => Guid.Parse("73c1846a-22fe-469f-8580-f7b79b658533");
        public Image Image => null;
        public bool OmitResult => false;
        public Keys ShortcutKeys => Keys.None;

        public void Apply(ISelInfo sel)
        {
            sel.SelectedText = Format(sel.SelectedText);
        }

        public bool Configure()
        {
            return false;
        }

        private static string Format(string text)
        {
            try
            {
                MDTabify _markdown = new MDTabify();
                return _markdown.TabifyTable(text, 4);
            }
            catch
            {

            }
            return string.Empty;
        }
    }
}
