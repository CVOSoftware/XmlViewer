using System.Windows;
using System.Windows.Controls;

namespace XmlViewer.Editor.View.Behaviors
{
    internal class TextBoxFocusBehavior
    {
        public static readonly DependencyProperty KeepFocusProperty =
            DependencyProperty.RegisterAttached(
                "KeepFocus",
                typeof(bool),
                typeof(TextBoxFocusBehavior),
                new UIPropertyMetadata(false, OnKeepFocusChanged));

        public static bool GetKeepFocus(DependencyObject obj)
        {
            return (bool)obj.GetValue(KeepFocusProperty);
        }

        public static void SetKeepFocus(DependencyObject obj, bool value)
        {
            obj.SetValue(KeepFocusProperty, value);
        }

        private static void OnKeepFocusChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (obj is TextBox element)
            {
                element.Focus();
            }
        }
    }
}
