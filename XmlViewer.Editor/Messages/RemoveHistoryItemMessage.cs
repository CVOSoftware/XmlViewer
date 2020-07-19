using XmlViewer.Editor.ViewModel;

namespace XmlViewer.Editor.Messages
{
    internal class RemoveHistoryItemMessage
    {
        public RemoveHistoryItemMessage(HistoryItemViewModel item)
        {
            Item = item;
        }
        public HistoryItemViewModel Item { get; }
    }
}
