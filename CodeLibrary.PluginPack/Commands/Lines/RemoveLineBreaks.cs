using CodeLibrary.PluginPack.Common;
using EditorPlugins.Core;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace CodeLibrary.PluginPack
{
    [Description("Removes all line breaks from selection.")]
    public class RemoveLineBreaks : IEditorPlugin
    {
        public bool IsExtension => false;
        public string Category => "Lines";
        public string DisplayName => "Remove Line Breaks";
        public Guid Id => Guid.Parse("61431de3-24cb-4f06-9fcb-2bb1aa512a69");
        public Image Image => null;
        public bool OmitResult => false;
        public Keys ShortcutKeys => Keys.None;

        public void Apply(ISelInfo sel)
        {
            if (string.IsNullOrWhiteSpace(sel.SelectedText))
            {
                return;
            }
            sel.SelectedText = string.Join("", Utils.Lines(sel.SelectedText));
        }

        public bool Configure()
        {
            return false;
        }
    }
}