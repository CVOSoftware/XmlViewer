using XmlViewer.Parser;

namespace XmlViewer.Editor.Messages
{
    internal class OpenEditorScreenMessage
    {
        public OpenEditorScreenMessage(string filePath, XmlParser parser)
        {
            FilePath = filePath;
            Parser = parser;
        }

        public string FilePath { get; }

        public XmlParser Parser { get; }
    }
}
