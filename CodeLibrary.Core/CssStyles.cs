using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeLibrary.Core
{
    public enum CssStyle
    {
        None = 0,
        Splendor = 1,
        Modest = 2,
        Retro = 3,
        Air = 4
    }

    public static class CssStyles
    {
        public static string GetCSS(CssStyle style)
        {
            switch (style)
            {
                case CssStyle.Air:
                    return AirCSS();
                case CssStyle.Modest:
                    return ModestCSS();
                case CssStyle.Splendor:
                    return SplendorCSS();
                case CssStyle.Retro:
                    return RetroCSS();
                case CssStyle.None:
                    return string.Empty;
            }
            return String.Empty;
        }


        private static string SplendorCSS()
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


        private static string ModestCSS()
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
    content: '(' attr(href) ')';
  }

        abbr[title]:after {
    content: ' (' attr(title) ')';
  }

    a[href ^= '#']:after,
  a[href ^= 'javascript:']:after {
    content: '';
  }

pre,
  blockquote
{
border: 1px solid #999;
    page -break-inside: avoid;
}

thead
{
display: table - header - group;
}

tr,
  img
{
    page -break-inside: avoid;
}

img
{
    max - width: 100 % !important;
}

p,
  h2,
  h3
{
orphans: 3;
widows: 3;
}

h2,
  h3
{
    page -break-after: avoid;
}
}

pre,
code
{
    font - family: Menlo, Monaco, 'Courier New', monospace;
}

pre
{
padding: .5rem;
    line - height: 1.25;
    overflow - x: scroll;
}

a,
a: visited {
color: #3498db;
}

a: hover,
a: focus,
a: active {
color: #2980b9;
}

.modest - no - decoration {
    text - decoration: none;
}

html
{
    font - size: 12px;
}

@media screen and (min-width: 32rem) and(max - width: 48rem) {
    html {
        font - size: 15px;
    }
}

@media screen and (min-width: 48rem) {
    html {
        font - size: 16px;
    }
}

body
{
    line - height: 1.85;
}

p,
.modest - p {
    font - size: 1rem;
    margin - bottom: 1.3rem;
}

h1,
.modest - h1,
h2,
.modest - h2,
h3,
.modest - h3,
h4,
.modest - h4 {
margin: 1.414rem 0 .5rem;
    font - weight: inherit;
    line - height: 1.42;
}

h1,
.modest - h1 {
    margin - top: 0;
    font - size: 3.998rem;
}

h2,
.modest - h2 {
    font - size: 2.827rem;
}

h3,
.modest - h3 {
    font - size: 1.999rem;
}

h4,
.modest - h4 {
    font - size: 1.414rem;
}

h5,
.modest - h5 {
    font - size: 1.121rem;
}

h6,
.modest - h6 {
    font - size: .88rem;
}

small,
.modest - small {
    font - size: .707em;
}

/* https://github.com/mrmrs/fluidity */

img,
canvas,
iframe,
video,
svg,
select,
textarea
{
    max - width: 100 %;
}

@import url(http://fonts.googleapis.com/css?family=Open+Sans+Condensed:300,300italic,700);

@import url(http://fonts.googleapis.com/css?family=Arimo:700,700italic);

html
{
    font - size: 18px;
    max - width: 100 %;
}

body
{
color: #444;
  font - family: 'Open Sans Condensed', sans - serif;
    font - weight: 300;
margin: 0 auto;
    max - width: 48rem;
    line - height: 1.45;
padding: .25rem;
}

h1,
h2,
h3,
h4,
h5,
h6
{
    font - family: Arimo, Helvetica, sans - serif;
}

h1,
h2,
h3
{
    border - bottom: 2px solid #fafafa;
  margin - bottom: 1.15rem;
    padding - bottom: .5rem;
    text - align: center;
}

blockquote
{
    border - left: 8px solid #fafafa;
  padding: 1rem;
}

pre,
code
{
    background - color: #fafafa;
}
";

            return _result;
        }


        private static string RetroCSS()
        {
            string _result = @"


pre,
code {
  font-family: Menlo, Monaco, 'Courier New', monospace;
}

        pre {
  padding: .5rem;
  line-height: 1.25;
  overflow-x: scroll;
}

    @media print
    {
  *,
  *:before,
  *:after
        {
        background: transparent!important;
        color: #000 !important;
    box - shadow: none!important;
            text - shadow: none!important;
        }

        a,
  a:visited
        {
            text - decoration: underline;
        }

        a [href]:after
        {
        content: ' (' attr(href) ')';
        }

        abbr [title]:after
        {
        content: ' (' attr(title) ')';
        }

        a [href^='#']:after,
  a [href^='javascript:']:after
        {
        content: '';
        }

        pre,
  blockquote
        {
        border: 1px solid #999;
    page -break-inside: avoid;
        }

        thead
        {
        display: table - header - group;
        }

        tr,
  img
        {
            page -break-inside: avoid;
        }

        img
        {
            max - width: 100 % !important;
        }

        p,
  h2,
  h3
        {
        orphans: 3;
        widows: 3;
        }

        h2,
  h3
        {
            page -break-after: avoid;
        }
    }

    a,
a:visited {
  color: #01ff70;
}

a: hover,
a: focus,
a: active {
color: #2ecc40;
}

.retro - no - decoration {
    text - decoration: none;
}

html
{
    font - size: 12px;
}

@media screen and (min-width: 32rem) and(max - width: 48rem) {
    html {
        font - size: 15px;
    }
}

@media screen and (min-width: 48rem) {
    html {
        font - size: 16px;
    }
}

body
{
    line - height: 1.85;
}

p,
.retro - p {
    font - size: 1rem;
    margin - bottom: 1.3rem;
}

h1,
.retro - h1,
h2,
.retro - h2,
h3,
.retro - h3,
h4,
.retro - h4 {
margin: 1.414rem 0 .5rem;
    font - weight: inherit;
    line - height: 1.42;
}

h1,
.retro - h1 {
    margin - top: 0;
    font - size: 3.998rem;
}

h2,
.retro - h2 {
    font - size: 2.827rem;
}

h3,
.retro - h3 {
    font - size: 1.999rem;
}

h4,
.retro - h4 {
    font - size: 1.414rem;
}

h5,
.retro - h5 {
    font - size: 1.121rem;
}

h6,
.retro - h6 {
    font - size: .88rem;
}

small,
.retro - small {
    font - size: .707em;
}

/* https://github.com/mrmrs/fluidity */

img,
canvas,
iframe,
video,
svg,
select,
textarea
{
    max - width: 100 %;
}

html,
body
{
    background - color: #222;
  min - height: 100 %;
}

html
{
    font - size: 18px;
}

body
{
color: #fafafa;
  font - family: 'Courier New';
    line - height: 1.45;
margin: 6rem auto 1rem;
    max - width: 48rem;
padding: .25rem;
}

pre
{
    background - color: #333;
}

blockquote
{
    border - left: 3px solid #01ff70;
  padding - left: 1rem;
}
";

            return _result;
        }

        private static string AirCSS()
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
    content: '(' attr(href) ')';
  }

        abbr[title]:after {
    content: ' (' attr(title) ')';
  }

    a[href ^= '#']:after,
  a[href ^= 'javascript:']:after {
    content: '';
  }

pre,
  blockquote
{
border: 1px solid #999;
    page -break-inside: avoid;
}

thead
{
display: table - header - group;
}

tr,
  img
{
    page -break-inside: avoid;
}

img
{
    max - width: 100 % !important;
}

p,
  h2,
  h3
{
orphans: 3;
widows: 3;
}

h2,
  h3
{
    page -break-after: avoid;
}
}

html
{
    font - size: 12px;
}

@media screen and (min-width: 32rem) and(max - width: 48rem) {
    html {
        font - size: 15px;
    }
}

@media screen and (min-width: 48rem) {
    html {
        font - size: 16px;
    }
}

body
{
    line - height: 1.85;
}

p,
.air - p {
    font - size: 1rem;
    margin - bottom: 1.3rem;
}

h1,
.air - h1,
h2,
.air - h2,
h3,
.air - h3,
h4,
.air - h4 {
margin: 1.414rem 0 .5rem;
    font - weight: inherit;
    line - height: 1.42;
}

h1,
.air - h1 {
    margin - top: 0;
    font - size: 3.998rem;
}

h2,
.air - h2 {
    font - size: 2.827rem;
}

h3,
.air - h3 {
    font - size: 1.999rem;
}

h4,
.air - h4 {
    font - size: 1.414rem;
}

h5,
.air - h5 {
    font - size: 1.121rem;
}

h6,
.air - h6 {
    font - size: .88rem;
}

small,
.air - small {
    font - size: .707em;
}

/* https://github.com/mrmrs/fluidity */

img,
canvas,
iframe,
video,
svg,
select,
textarea
{
    max - width: 100 %;
}

@import url(http://fonts.googleapis.com/css?family=Open+Sans:300italic,300);

body
{
color: #444;
  font - family: 'Open Sans', Helvetica, sans - serif;
    font - weight: 300;
margin: 6rem auto 1rem;
    max - width: 48rem;
    text - align: center;
}

img
{
    border - radius: 50 %;
height: 200px;
margin: 0 auto;
width: 200px;
}

a,
a: visited {
color: #3498db;
}

a: hover,
a: focus,
a: active {
color: #2980b9;
}

pre
{
    background - color: #fafafa;
  padding: 1rem;
    text - align: left;
}

blockquote
{
margin: 0;
    border - left: 5px solid #7a7a7a;
  font - style: italic;
padding: 1.33em;
    text - align: left;
}

ul,
ol,
li
{
    text - align: left;
}

p
{
color: #777;
}
";

            return _result;
        }


    }
}
