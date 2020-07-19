using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;

namespace XmlViewer.Parser
{
    public class XmlParser
    {
        private HtmlDocument document;

        public XmlParser(string path)
        {
            document = new HtmlDocument();
            document.Load(path);
        }

        public IEnumerable<string> Select(string xPath)
        {
            var nodes = document.DocumentNode.SelectNodes(xPath);

            return nodes?.Select(node => node.InnerHtml);
        }

        public string GetSource()
        {
            return document.Text;
        }
    }
}
