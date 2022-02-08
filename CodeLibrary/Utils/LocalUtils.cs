using CodeLibrary.Core;
using FastColoredTextBoxNS;
using System.IO;
using System.Windows.Forms;

namespace CodeLibrary
{
    public static class LocalUtils
    {

        public static string SplendorCSS()
        {
            string _result = @"
@media print {
  *,
  *:before,
  *:after {
    background: transparent !important;
    color: #000 !important;
    box-shadow: none !important;
    text-shadow: none !important;
  }

  a,
  a:visited {
    text-decoration: underline;
  }

  a[href]:after {
    content: ' (' attr(href) ')';
  }

  abbr[title]:after {
    content: ' (' attr(title) ')';
  }

  a[href^='#']:after,
  a[href^='javascript:']:after {
    content: '';
  }

  pre,
  blockquote {
    border: 1px solid #999;
    page-break-inside: avoid;
  }

  thead {
    display: table-header-group;
  }

  tr,
  img {
    page-break-inside: avoid;
  }

  img {
    max-width: 100% !important;
  }

  p,
  h2,
  h3 {
    orphans: 3;
    widows: 3;
  }

  h2,
  h3 {
    page-break-after: avoid;
  }
}

html {
  font-size: 12px;
}

@media screen and (min-width: 32rem) and (max-width: 48rem) {
  html {
    font-size: 15px;
  }
}

@media screen and (min-width: 48rem) {
  html {
    font-size: 16px;
  }
}

body {
  line-height: 1.85;
}

p,
.splendor-p {
  font-size: 1rem;
  margin-bottom: 1.3rem;
}

h1,
.splendor-h1,
h2,
.splendor-h2,
h3,
.splendor-h3,
h4,
.splendor-h4 {
  margin: 1.414rem 0 .5rem;
  font-weight: inherit;
  line-height: 1.42;
}

h1,
.splendor-h1 {
  margin-top: 0;
  font-size: 3.998rem;
}

h2,
.splendor-h2 {
  font-size: 2.827rem;
}

h3,
.splendor-h3 {
  font-size: 1.999rem;
}

h4,
.splendor-h4 {
  font-size: 1.414rem;
}

h5,
.splendor-h5 {
  font-size: 1.121rem;
}

h6,
.splendor-h6 {
  font-size: .88rem;
}

small,
.splendor-small {
  font-size: .707em;
}

/* https://github.com/mrmrs/fluidity */

img,
canvas,
iframe,
video,
svg,
select,
textarea {
  max-width: 100%;
}

@import url(http://fonts.googleapis.com/css?family=Merriweather:300italic,300);

html {
  font-size: 18px;
  max-width: 100%;
}

body {
  color: #444;
  font-family: 'Merriweather', Georgia, serif;
  margin: 0;
  max-width: 100%;
}

/* === A bit of a gross hack so we can have bleeding divs/blockquotes. */

p,
*:not(div):not(img):not(body):not(html):not(li):not(blockquote):not(p) {
  margin: 1rem auto 1rem;
  max-width: 36rem;
  padding: .25rem;
}

div {
  width: 100%;
}

div img {
  width: 100%;
}

blockquote p {
  font-size: 1.5rem;
  font-style: italic;
  margin: 1rem auto 1rem;
  max-width: 48rem;
}

li {
  margin-left: 2rem;
}

/* Counteract the specificity of the gross *:not() chain. */

h1 {
  padding: 4rem 0 !important;
}

/*  === End gross hack */

p {
  color: #555;
  height: auto;
  line-height: 1.45;
}

pre,
code {
  font-family: Menlo, Monaco, 'Courier New', monospace;
}

pre {
  background-color: #fafafa;
  font-size: .8rem;
  overflow-x: scroll;
  padding: 1.125em;
}

a,
a:visited {
  color: #3498db;
}

a:hover,
a:focus,
a:active {
  color: #2980b9;
}
";

            return _result;
        }


        public static KeysLanguage KeysLanguageForCodeType(CodeType codetype)
        {
            switch (codetype)
            {
                case CodeType.Folder:
                    return KeysLanguage.None;
                case CodeType.CSharp:
                    return KeysLanguage.CSharp;
                case CodeType.SQL:
                    return KeysLanguage.SQL;
                case CodeType.VB:
                    return KeysLanguage.VB;
                case CodeType.None:
                    return KeysLanguage.None;
                case CodeType.HTML:
                    return KeysLanguage.HTML;
                case CodeType.Template:
                    return KeysLanguage.Template;
                case CodeType.XML:
                    return KeysLanguage.XML;
                case CodeType.PHP:
                    return KeysLanguage.PHP;
                case CodeType.Lua:
                    return KeysLanguage.Lua;
                case CodeType.JS:
                    return KeysLanguage.JS;
                case CodeType.RTF:
                    return KeysLanguage.RTF;
                case CodeType.MarkDown:
                    return KeysLanguage.MarkDown;
            }
            return KeysLanguage.AllLanguages;
        }

        public static CodeType CodeTypeByExtension(FileInfo file)
        {
            string _extension = file.Extension.Trim(new char[] { '.' }).ToLower();
            switch (_extension)
            {
                case "vb":
                    return CodeType.VB;

                case "cs":
                    return CodeType.CSharp;

                case "js":
                case "ts":
                case "json":
                    return CodeType.JS;

                case "txt":
                case "inf":
                case "info":
                case "nfo":
                    return CodeType.None;

                case "md":
                    return CodeType.MarkDown;

                case "html":
                case "htm":
                    return CodeType.HTML;

                case "resx":
                case "xml":
                case "xmlt":
                case "xlt":
                case "xslt":
                    return CodeType.XML;

                case "sql":
                    return CodeType.SQL;

                case "rtf":
                    return CodeType.RTF;

                case "jpg":
                case "jpeg":
                case "png":
                case "bmp":
                    return CodeType.Image;
            }
            return CodeType.UnSuported;
        }

        public static Language CodeTypeToLanguage(CodeType codeType)
        {
            switch (codeType)
            {
                case CodeType.CSharp:
                    return Language.CSharp;

                case CodeType.HTML:
                    return Language.HTML;

                case CodeType.Image:
                    return Language.Custom;

                case CodeType.JS:
                    return Language.JS;

                case CodeType.Lua:
                    return Language.Lua;

                case CodeType.PHP:
                    return Language.PHP;

                case CodeType.SQL:
                    return Language.SQL;

                case CodeType.Folder:
                case CodeType.MarkDown:
                case CodeType.None:
                case CodeType.RTF:
                case CodeType.Template:
                case CodeType.UnSuported:
                case CodeType.System:
                    return Language.Custom;

                case CodeType.VB:
                    return Language.VB;

                case CodeType.XML:
                    return Language.XML;

                default:
                    return Language.Custom;
            }
        }

        public static int GetImageIndex(CodeSnippet snippet)
        {
            if (snippet.CodeType == CodeType.ReferenceLink)
                return 14;

            if (snippet.Important)
                return 2;

            if (snippet.CodeType == CodeType.System && snippet.Id == Constants.TRASHCAN)
                return 3;

            if (snippet.CodeType == CodeType.System && snippet.Id == Constants.CLIPBOARDMONITOR)
                return 11;

            if (snippet.AlarmActive)
                return 5;

            return GetImageIndex(snippet.CodeType);
        }

        public static int GetImageIndex(CodeType type)
        {
            switch (type)
            {
                case CodeType.Template:
                    return 1;

                case CodeType.CSharp:
                case CodeType.HTML:
                case CodeType.VB:
                case CodeType.JS:
                case CodeType.PHP:
                case CodeType.XML:
                case CodeType.Lua:
                case CodeType.None:
                case CodeType.RTF:
                case CodeType.SQL:
                case CodeType.MarkDown:
                    return 1;

                case CodeType.Folder:
                    return 0;

                case CodeType.Image:
                    return 10;

                case CodeType.ReferenceLink:
                    return 14;
            }
            return 0;
        }

        public static TreeNode GetNodeByParentPath(TreeNodeCollection collection, string path)
        {
            foreach (TreeNode node in collection)
            {
                if (node.FullPath.Equals(path))
                    return node;
            }
            foreach (TreeNode node in collection)
            {
                TreeNode subnode = GetNodeByParentPath(node.Nodes, path);
                if (subnode != null)
                    return subnode;
            }
            return null;
        }

        public static string LastPart(string path)
        {
            int ii = path.IndexOf('\\');
            if (ii < 0)
                return path;

            return path.Substring(ii, path.Length - ii);
        }
    }
}