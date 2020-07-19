using System.Collections.ObjectModel;
using System.Linq;
using MVVMLight.Messaging;
using XmlViewer.Editor.Messages;
using XmlViewer.Editor.ViewModel.Base;
using XmlViewer.Parser;

namespace XmlViewer.Editor.ViewModel
{
    class EditorViewModel : BaseViewModel
    {
        #region Const

        private const string NOT_FOUND = "(not found)";

        #endregion

        #region Fields

        private bool openFirst;

        private bool screenVisibility;

        private string filePath;

        private string sourceData;

        private string xPathExpression;

        private string searchingCount;

        private string historyCount;

        private string selectedHistoryItem;

        private XmlParser parser;

        #endregion

        public EditorViewModel()
        {
            SearchingResultItems = new ObservableCollection<string>();
            SearchHistoryItems = new ObservableCollection<string>();

            Messenger.Default.Register<OpenEditorScreenMessage>(this, OpenEditorScreenMessageHandler);
            Messenger.Default.Register<CloseEditorScreenMessage>(this, CloseEditorScreenMessageHandler);
            Messenger.Default.Register<ToEditorScreenMessage>(this, ToEditorScreenMessageHandler);
        }

        #region Properties

        public bool ScreenVisibility
        {
            get => screenVisibility;
            set => SetValue(ref screenVisibility, value);
        }

        public string FilePath
        {
            get => filePath;
            set => SetValue(ref filePath, value);
        }

        public string SourceData
        {
            get => sourceData;
            set => SetValue(ref sourceData, value);
        }
        
        public string XPathExpression
        {
            get => xPathExpression;
            set
            {
                if (SetValue(ref xPathExpression, value) && searchingCount?.Equals(NOT_FOUND) == true)
                {
                    SearchingCount = string.Empty;
                }
            }
        }

        public string SearchingCount
        {
            get => searchingCount;
            set => SetValue(ref searchingCount, value);
        }

        public string HistoryCount
        {
            get => historyCount;
            set => SetValue(ref historyCount, value);
        }

        public string SelectedHistoryItem
        {
            get => selectedHistoryItem;
            set
            {
                if (SetValue(ref selectedHistoryItem, value))
                {
                    XPathExpression = selectedHistoryItem;
                }
            }
        }

        public ObservableCollection<string> SearchingResultItems { get; }

        public ObservableCollection<string> SearchHistoryItems { get; }

        #endregion

        #region Commands

        #region ToStartScreen

        private RelayCommand toStartScreenCommand;

        public RelayCommand ToStartScreenCommand => RelayCommand.Register(ref toStartScreenCommand, OnToStartScreen);

        private void OnToStartScreen(object commandParameter)
        {
            ScreenVisibility = false;
            Messenger.Default.Send(ToStartScreenMessage.Instance);
        }

        #endregion

        #region SearchByXPathCommand

        private RelayCommand searchByXPathCommand;

        public RelayCommand SearchByXPathCommand => RelayCommand.Register(ref searchByXPathCommand, OnSearchByXPathCommand, CanSearchByXPathCommand);

        private void OnSearchByXPathCommand(object commandParameter)
        {
            var historyItem = string.Copy(xPathExpression);

            SearchHistoryItems.Add(historyItem);
            SearchingResultItems.Clear();

            HistoryCount = SearchHistoryItems.Count.ToString();

            try
            {
                var searchingResults = parser.Select(xPathExpression);

                if (searchingResults == null || searchingResults.Any() == false)
                {
                    SearchingCount = NOT_FOUND;

                    return;
                }

                foreach (var searchingResultItem in searchingResults)
                {
                    SearchingResultItems.Add(searchingResultItem);
                }

                SearchingCount = SearchingResultItems.Count.ToString();

            }
            catch
            {
                SearchingCount = NOT_FOUND;
            }
        }

        private bool CanSearchByXPathCommand(object commandParameter)
        {
            return xPathExpression?.Length > 0;
        }

        #endregion

        #region ClearHistory

        private RelayCommand clearHistoryCommand;

        public RelayCommand ClearHistoryCommand => RelayCommand.Register(ref clearHistoryCommand, OnClearHistoryCommand, CanClearHistoryCommand);

        private void OnClearHistoryCommand(object commandParameter)
        {
            HistoryCount = null;
            SearchHistoryItems.Clear();
        }

        private bool CanClearHistoryCommand(object commandParameter)
        {
            return SearchHistoryItems.Any();
        }

        #endregion

        #endregion

        #region Methods

        private void ClearEditor()
        {
            XPathExpression = null;
            SearchingCount = null;
            HistoryCount = null;
            SelectedHistoryItem = null;
            SearchingResultItems.Clear();
            SearchHistoryItems.Clear();
        }

        #endregion

        #region Message handlers

        private void OpenEditorScreenMessageHandler(OpenEditorScreenMessage message)
        {
            parser = message.Parser;
            FilePath = message.FilePath;
            SourceData = parser.GetSource();
            ScreenVisibility = true;

            ClearEditor();

            if (openFirst == false)
            {
                openFirst = true;

                return;
            }

            var responseMessage = new ClosedEditorScreenMessage(filePath);

            Messenger.Default.Send(responseMessage);
        }

        private void CloseEditorScreenMessageHandler(CloseEditorScreenMessage message)
        {
            var responseMessage = new ClosedEditorScreenMessage(filePath);

            parser = null;
            FilePath = null;
            SourceData = null;

            ClearEditor();

            Messenger.Default.Send(responseMessage);
        }

        private void ToEditorScreenMessageHandler(ToEditorScreenMessage message)
        {
            ScreenVisibility = true;
        }

        #endregion
    }
}
