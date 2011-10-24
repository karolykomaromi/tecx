using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using TecX.Common;

namespace TecX.Agile.Behaviors
{
    public static class PreviewFocusBehavior
    {
        public static readonly DependencyProperty IsEnabledProperty = DependencyProperty.RegisterAttached(
            "IsEnabled",
            typeof(bool),
            typeof(PreviewFocusBehavior),
            new FrameworkPropertyMetadata(false, OnEnabledChanged));

        public static void SetIsEnabled(TextBox textBox, bool value)
        {
            Guard.AssertNotNull(textBox, "textBox");

            textBox.SetValue(IsEnabledProperty, value);
        }

        public static bool GetIsEnabled(TextBox textBox)
        {
            Guard.AssertNotNull(textBox, "textBox");

            return (bool)textBox.GetValue(IsEnabledProperty);
        }

        private static void OnEnabledChanged(DependencyObject dependencyObject,
            DependencyPropertyChangedEventArgs args)
        {
            if (!DesignerProperties.GetIsInDesignMode(dependencyObject))
            {
                bool isEnabled = (bool)args.NewValue;
                bool oldValue = (bool)args.OldValue;

                // do nothing if the setting did not change
                if (isEnabled == oldValue)
                {
                    return;
                }

                TextBox textBox = dependencyObject as TextBox;

                if (textBox != null)
                {
                    if (isEnabled)
                    {
                        textBox.PreviewMouseLeftButtonDown += OnPreviewMouseLeftButtonDown;
                    }
                    else
                    {
                        textBox.PreviewMouseLeftButtonDown -= OnPreviewMouseLeftButtonDown;
                    }
                }
            }
        }

        private static void OnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            TextBox textBox = sender as TextBox;

            if (textBox != null)
            {
                textBox.Focus();
            }
        }
    }
}
