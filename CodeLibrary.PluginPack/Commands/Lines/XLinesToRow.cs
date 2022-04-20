using CodeLibrary.PluginPack.Common;
using CodeLibrary.PluginPack.Forms;
using EditorPlugins.Core;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace CodeLibrary.PluginPack
{
    [Description("X Lines to Row")]
    public class XLinesToRow : IEditorPlugin
    {
        public bool IsExtension => false;
        public string Category => "Lines";
        public string DisplayName => "X Lines to Row";
        public Guid Id => Guid.Parse("358fec78-b205-4f5c-9657-ad41050ee1bf");
        public Image Image => null;
        public bool OmitResult { get; set; } = false;
        public Keys ShortcutKeys => Keys.None;
        public string Separator { get; set; } = ";";

        public int NumberOfLines { get; set; } = 5;

        public void Apply(ISelInfo sel)
        {
            string text = sel.SelectedText;

            OmitResult = false;

            if (!Configure())
                OmitResult = true;

            string[] _lines = Utils.Lines(text);

            StringBuilder _sbLine = new StringBuilder();
            StringBuilder _result = new StringBuilder();

            for (int ii = 0, jj = 1; ii < _lines.Length; ii++, jj++)
            {
                _sbLine.Append(_lines[ii]);
                _sbLine.Append(Separator);

                if (jj >= NumberOfLines)
                {
                    _sbLine.Length = _sbLine.Length - Separator.Length;
                    jj = 0;
                    _result.AppendLine(_sbLine.ToString());
                    _sbLine.Clear();
                }
            }
            if (_sbLine.Length > 0)
            {
                _result.AppendLine(_sbLine.ToString());
            }

            sel.SelectedText = _result.ToString();
        }

        public bool Configure()
        {
            FormXLinesToRow f = new FormXLinesToRow();
            f.Separator = Separator;
            f.LineCount = NumberOfLines;
 

            DialogResult dr = f.ShowDialog();
            if (dr == DialogResult.OK)
            {
                Separator = f.Separator;
                NumberOfLines = f.LineCount;
                return true;
            }
            return false;
        }
    }
}