using System.Windows;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Win32;

namespace XmlViewer.Editor.Helper
{
    internal static class DialogHelper
    {
        public static string GetFile()
        {
            var dialog = new OpenFileDialog();

            dialog.Filter = "txt files (*.xml)|*.xml";

            return dialog.ShowDialog() == true 
                ? dialog.FileName 
                : string.Empty;
        }

        public static void MessageBox(string title, string description)
        {
            if (Application.Current.MainWindow is MetroWindow view)
            {
                view.ShowModalMessageExternal(title, description);
            }
        }
    }
}
