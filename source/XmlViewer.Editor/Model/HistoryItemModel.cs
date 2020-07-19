using System;

namespace XmlViewer.Editor.Model
{
    [Serializable]
    public class HistoryItemModel
    {
        public HistoryItemModel()
        {

        }

        public DateTime Date { get; set; }

        public string FilePath { get; set; }
    }
}
