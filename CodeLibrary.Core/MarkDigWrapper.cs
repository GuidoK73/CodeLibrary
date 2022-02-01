using Markdig;
using Markdig.SyntaxHighlighting;
using SelectPdf;

namespace CodeLibrary.Core
{
    public class MarkDigWrapper
    {
        public string Transform(string text)
        {
            var _pipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().UseSyntaxHighlighting().Build();
            var _result = Markdown.ToHtml(text, _pipeline);
            return _result;
        }

        public void ToPDF(string file, string html)
        {
            SelectPdf.HtmlToPdf htmlToPdf = new SelectPdf.HtmlToPdf();
            PdfDocument _pdf = htmlToPdf.ConvertHtmlString(html);
            _pdf.Save(file);
        }
    }
}