﻿using CodeLibrary.Core;
using EditorPlugins.Core;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace CodeLibrary.PluginPack
{
    [Description("Markdown to HTML")]
    public class MarkdownFormatter : IEditorPlugin
    {
        public bool IsExtension => false;
        public string Category => "Formatting";
        public string DisplayName => "Markdown to Html Formatter";
        public Guid Id => Guid.Parse("73c1846a-22fe-469f-8580-f7b79b658531");
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
                MarkDigWrapper _markdown = new MarkDigWrapper();
                return _markdown.Transform(text);
            }
            catch
            {

            }
            return string.Empty;
        }
    }
}