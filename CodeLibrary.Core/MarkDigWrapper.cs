using Markdig;
using Markdig.SyntaxHighlighting;

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
    }
}