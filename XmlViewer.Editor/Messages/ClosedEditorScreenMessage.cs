namespace XmlViewer.Editor.Messages
{
    internal class ClosedEditorScreenMessage
    {
        public ClosedEditorScreenMessage(string filePath)
        {
            FilePath = filePath;
        }

        public string FilePath { get; }
    }
}
