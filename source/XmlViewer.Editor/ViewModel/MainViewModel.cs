using XmlViewer.Editor.ViewModel.Base;

namespace XmlViewer.Editor.ViewModel
{
    internal class MainViewModel : BaseViewModel
    {
        public MainViewModel()
        {
            PresentationVM = new PresentationViewModel();
            EditorVM = new EditorViewModel();
        }

        public PresentationViewModel PresentationVM { get; }

        public EditorViewModel EditorVM { get; }
    }
}
