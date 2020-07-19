using System;
using System.IO;
using MVVMLight.Messaging;
using XmlViewer.Editor.Messages;
using XmlViewer.Editor.ViewModel.Base;

namespace XmlViewer.Editor.ViewModel
{
    class HistoryItemViewModel : BaseViewModel
    {
        #region Fields

        private bool visibility;

        #endregion

        public HistoryItemViewModel(DateTime date, string filePath)
        {
            visibility = true;
            Date = date;
            FilePath = filePath;
            Name = Path.GetFileName(filePath);
            NameToLower = Name.ToLower();
        }

        #region Properties

        public bool Visibility
        {
            get => visibility;
            set => SetValue(ref visibility, value);
        }

        public DateTime Date { get; }

        public string Name { get; }

        public string NameToLower { get; }

        public string FilePath { get; }

        #endregion

        #region Commands

        #region Open

        private RelayCommand openCommand;

        public RelayCommand OpenCommand => RelayCommand.Register(ref openCommand, OnOpen);

        private void OnOpen(object commandParameter)
        {
            var message = new OpenFileMessage(FilePath);

            Messenger.Default.Send(message);
        }

        #endregion

        #region Remove

        private RelayCommand removeCommand;

        public RelayCommand RemoveCommand => RelayCommand.Register(ref removeCommand, OnRemove);

        private void OnRemove(object commandParameter)
        {
            var message = new RemoveHistoryItemMessage(this);

            Messenger.Default.Send(message);
        }

        #endregion

        #endregion
    }
}
