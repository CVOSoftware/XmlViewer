namespace XmlViewer.Editor.Messages
{
    internal class OpenFileMessage
    {
        public OpenFileMessage(string filePath)
        {
            FilePath = filePath;
        }

        public string FilePath { get; }
    }
}
