//using CodeLibrary.PluginPack.Forms;
//using EditorPlugins.Core;
//using System;
//using System.ComponentModel;
//using System.Drawing;
//using System.Text.RegularExpressions;
//using System.Windows.Forms;

//namespace CodeLibrary.PluginPack.Commands
//{
//    [Description("Regex Extract")]
//    public class RegexExtract : IEditorPlugin
//    {
//        public bool IsExtension => false;
//        public string Category => "";
//        public string DisplayName => "Regex Extract";
//        public Guid Id => Guid.Parse("84b70ea9-09e7-43ac-bd60-482eb3084353");
//        public Image Image => ImageResource.replace;
//        public bool OmitResult { get; set; }

//        public string Library { get; set; }

//        public string Find { get; set; }

//        public string Replace { get; set; }

//        public Keys ShortcutKeys => Keys.None;

//        public void Apply(ISelInfo sel)
//        {
//            string text = sel.SelectedText;

//            OmitResult = false;

//            if (!Configure())
//            {
//                OmitResult = true;
//            }

//            // Alter text

//            OmitResult = false;

//            try
//            {
//                Regex _regex = new Regex(Find);
//                foreach (Match match in _regex.Matches(text))
//                {
//                    match.Captures[];
//                }
//                sel.SelectedText = text;
//            }
//            catch
//            {
//            }
//        }

//        public bool Configure()
//        {
//            // Show Regex Editor

//            FormRegexReplace f = new FormRegexReplace();
//            f.Library = Library;
//            f.Find = Find;
//            f.Replace = Replace;

//            DialogResult dr = f.ShowDialog();
//            if (dr == DialogResult.OK)
//            {
//                Find = f.Find;
//                Replace = f.Replace;
//                return true;
//            }
//            Library = f.Library;

//            return false;
//        }
//    }
//}