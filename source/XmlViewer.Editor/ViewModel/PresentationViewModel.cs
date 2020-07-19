using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using MVVMLight.Messaging;
using XmlViewer.Editor.Const;
using XmlViewer.Editor.Helper;
using XmlViewer.Editor.Messages;
using XmlViewer.Editor.Model;
using XmlViewer.Editor.ViewModel.Base;
using XmlViewer.Logger;
using XmlViewer.Parser;

namespace XmlViewer.Editor.ViewModel
{
    class PresentationViewModel : BaseViewModel
    {
        #region Const

        private const string ERROR_TITLE = "Error opened file";

        private const string ERROR_MESSAGE = "File path";

        #endregion

        #region Fields

        private bool screenVisibility;

        private bool searchPanelVisibility;

        private bool isOpenEditor;

        private string searchingText;

        private string filePath;

        #endregion

        public PresentationViewModel()
        {
            screenVisibility = true;
            HistoryItems = new ObservableCollection<HistoryItemViewModel>();

            LoadFromModel();

            Messenger.Default.Register<ToStartScreenMessage>(this, ToStartScreenMessageHandler);
            Messenger.Default.Register<OpenFileMessage>(this, OpenFileMessageHandler);
            Messenger.Default.Register<RemoveHistoryItemMessage>(this, RemoveHistoryItemMessageHandler);
            Messenger.Default.Register<ClosedEditorScreenMessage>(this, ClosedEditorScreenMessageHandler);
        }

        #region Properties

        public bool ScreenVisibility
        {
            get => screenVisibility;
            set => SetValue(ref screenVisibility, value);
        }

        public bool SearchPanelVisibility
        {
            get => searchPanelVisibility;
            set => SetValue(ref searchPanelVisibility, value);
        }

        public bool IsOpenEditor
        {
            get => isOpenEditor;
            set => SetValue(ref isOpenEditor, value);
        }

        public string SearchingText
        {
            get => searchingText;
            set
            {
                if (SetValue(ref searchingText, value?.ToLower()))
                {
                    SearchAndShowHistoryItem();
                }
            }
        }

        public ObservableCollection<HistoryItemViewModel> HistoryItems { get; }

        #endregion

        #region Commands

        #region Clear

        private RelayCommand clearCommand;

        public RelayCommand ClearCommand => RelayCommand.Register(ref clearCommand, OnClear, CanClear);

        private void OnClear(object commandParameter)
        {
            HistoryItems.Clear();
            SaveModel();
        }

        private bool CanClear(object commandParameter)
        {
            return HistoryItems.Count > 0;
        }

        #endregion

        #region Search

        private RelayCommand searchCommand;

        public RelayCommand SearchCommand => RelayCommand.Register(ref searchCommand, OnSearch, CanSearch);

        private void OnSearch(object commandParameter)
        {
            SearchPanelVisibility = !searchPanelVisibility;
        }

        private bool CanSearch(object commandParameter)
        {
            return HistoryItems.Count > 0;
        }

        #endregion

        #region Open

        private RelayCommand openCommand;

        public RelayCommand OpenCommand => RelayCommand.Register(ref openCommand, OnOpen);

        private void OnOpen(object commandParameter)
        {
            var filePath = DialogHelper.GetFile();

            OpenFileAndCreateParser(filePath);
            SaveModel();
        }

        #endregion

        #region Close

        private RelayCommand closeCommand;

        public RelayCommand CloseCommand => RelayCommand.Register(ref closeCommand, OnClose, CanClose);

        private void OnClose(object commandParameter)
        {
            IsOpenEditor = false;
            filePath = null;

            Messenger.Default.Send(CloseEditorScreenMessage.Instance);
        }

        private bool CanClose(object commandParameter)
        {
            return IsOpenEditor;
        }

        #endregion

        #region Return

        private RelayCommand returnCommand;

        public RelayCommand ReturnCommand => RelayCommand.Register(ref returnCommand, OnReturn, CanReturn);

        private void OnReturn(object commandParameter)
        {
            ScreenVisibility = false;
            SearchPanelVisibility = false;

            Messenger.Default.Send(ToEditorScreenMessage.Instance);
        }

        private bool CanReturn(object commandParameter)
        {
            return IsOpenEditor;
        }

        #endregion

        #endregion

        #region Methods

        private void LoadFromModel()
        {
            Task.Run(() =>
            {
                try
                {
                    var model = LoadHistoryItemHelper.Load();

                    if (model == null)
                    {
                        return;
                    }

                    foreach (var item in model.Items)
                    {
                        var itemViewModel = new HistoryItemViewModel(item.Date, item.FilePath);

                        HistoryItems.Add(itemViewModel);
                    }
                }
                catch (Exception e)
                {
                    SystemLogger.Error(e, LoggerConst.ERROR_LOAD_RECENT);
                }
            });
        }

        private void SaveModel()
        {
            Task.Run(() =>
            {
                try
                {
                    var model = new HistoryItemsModel();
                    var items = HistoryItems.Select(item => new HistoryItemModel
                    {
                        Date = item.Date,
                        FilePath = string.Copy(item.FilePath)
                    }).OrderByDescending(item => item.Date).ToArray();

                    model.Items = items;

                    LoadHistoryItemHelper.Save(model);
                }
                catch (Exception e)
                {
                    SystemLogger.Error(e, LoggerConst.ERROR_SAVE_RECENT);
                }
            });
        }

        private void SearchAndShowHistoryItem()
        {
            if (searchingText.Equals(string.Empty))
            {
                foreach (var historyItem in HistoryItems)
                {
                    historyItem.Visibility = true;
                }

                return;
            }

            foreach (var historyItem in HistoryItems)
            {
                historyItem.Visibility = historyItem.NameToLower.StartsWith(searchingText);
            }
        }

        private void OpenFileAndCreateParser(string filePath)
        {
            SearchPanelVisibility = false;

            try
            {
                if (filePath == string.Empty)
                {
                    return;
                }

                if (this.filePath?.Equals(filePath, StringComparison.OrdinalIgnoreCase) == true)
                {
                    ScreenVisibility = false;

                    Messenger.Default.Send(ToEditorScreenMessage.Instance);

                    return;
                }

                var date = DateTime.Now;
                var parser = new XmlParser(filePath);
                var historyItem = new HistoryItemViewModel(date, filePath);

                ScreenVisibility = false;
                IsOpenEditor = true;
                this.filePath = filePath;

                HistoryItems.Insert(0, historyItem);
                Messenger.Default.Send(new OpenEditorScreenMessage(filePath, parser));
                SystemLogger.Info($"{LoggerConst.OPEN_FILE}: {filePath}");
            }
            catch (Exception e)
            {
                SystemLogger.Error(e, $"{LoggerConst.ERROR_OPEN_FILE}: {filePath}");
                DialogHelper.MessageBox(ERROR_TITLE, $"{ERROR_MESSAGE}: {filePath}");
            }
        }

        #endregion

        #region Message handlers

        private void ToStartScreenMessageHandler(ToStartScreenMessage message)
        {
            ScreenVisibility = true;
        }

        private void OpenFileMessageHandler(OpenFileMessage message)
        {
            OpenFileAndCreateParser(message.FilePath);
            SaveModel();
        }

        private void RemoveHistoryItemMessageHandler(RemoveHistoryItemMessage message)
        {
            HistoryItems.Remove(message.Item);
            SaveModel();
        }

        private void ClosedEditorScreenMessageHandler(ClosedEditorScreenMessage message)
        {
            SystemLogger.Info($"{LoggerConst.CLOSE_FILE}: {message.FilePath}");
        }

        #endregion
    }
}
