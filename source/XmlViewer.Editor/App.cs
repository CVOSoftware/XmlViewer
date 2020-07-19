using System;
using System.Collections.Generic;
using System.Windows;
using XmlViewer.Editor.View;
using XmlViewer.Editor.ViewModel;

namespace XmlViewer.Editor
{
    internal class App : Application
    {
        private List<Uri> uriCollection;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            InitializeResourceUri();
            LoadResources();
            OpenWindow();
        }

        private void InitializeResourceUri()
        {
            uriCollection = new List<Uri>
            {
                new Uri("pack://application:,,,/MahApps.Metro;component/Styles/Controls.xaml"),
                new Uri("pack://application:,,,/MahApps.Metro;component/Styles/Fonts.xaml"),
                new Uri("pack://application:,,,/MahApps.Metro;component/Styles/Themes/Dark.Blue.xaml")
            };
        }

        private void LoadResources()
        {
            foreach (var uri in uriCollection)
            {
                Resources.MergedDictionaries.Add(new ResourceDictionary { Source = uri });
            }
        }

        private void OpenWindow()
        {
            var viewModel = new MainViewModel();
            var view = new MainWindow();

            view.DataContext = viewModel;
            view.Show();
        }
    }
}
