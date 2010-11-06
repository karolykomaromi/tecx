using System.Windows;
using System.Windows.Controls;

using TecX.Common;

namespace TecX.Agile.View.Behavior
{
    public class HighlightTextBoxBehavior : ItemBehavior
    {
        #region Properties

        /// <summary>
        /// This is a dependency property.
        /// </summary>
        public static readonly DependencyProperty IsEnabledProperty =
            DependencyProperty.RegisterAttached(
                "IsEnabled",
                typeof (bool),
                typeof (HighlightTextBoxBehavior),
                new FrameworkPropertyMetadata(false, OnBehaviorEnabledChanged<HighlightTextBoxHandler>));

        /// <summary>
        /// Setter for <see cref="DependencyProperty"/> <see cref="IsEnabledProperty"/>
        /// </summary>
        public static void SetIsEnabled(TextBox textBox, bool value)
        {
            Guard.AssertNotNull(textBox, "textBox");

            textBox.SetValue(IsEnabledProperty, value);
        }

        /// <summary>
        /// Getter for <see cref="DependencyProperty"/> <see cref="IsEnabledProperty"/>
        /// </summary>
        public static bool GetIsEnabled(TextBox textBox)
        {
            Guard.AssertNotNull(textBox, "textBox");

            return (bool) textBox.GetValue(IsEnabledProperty);
        }

        #endregion Properties
    }
}