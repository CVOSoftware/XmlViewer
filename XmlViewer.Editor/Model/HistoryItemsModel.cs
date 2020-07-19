using System;

namespace XmlViewer.Editor.Model
{
    [Serializable]
    public class HistoryItemsModel
    {
        public HistoryItemsModel()
        {

        }

        public HistoryItemModel[] Items { get; set; }
    }
}
