using System.Windows;
using System.Windows.Controls;

using TecX.Common;

namespace TecX.Agile.View.Behavior
{
    public class TossingBehavior : MovementBehaviorBase
    {
        #region Properties

        /// <summary>
        /// This is a dependency property.
        /// </summary>
        public static readonly DependencyProperty IsEnabledProperty =
            DependencyProperty.RegisterAttached(
                "IsEnabled",
                typeof(bool),
                typeof(TossingBehavior),
                new FrameworkPropertyMetadata(false, OnBehaviorEnabledChanged<TossingHandler>));

        /// <summary>
        /// Setter for <see cref="DependencyProperty"/> <see cref="IsEnabledProperty"/>
        /// </summary>
        public static void SetIsEnabled(UserControl userControl, bool value)
        {
            Guard.AssertNotNull(userControl, "userControl");

            userControl.SetValue(IsEnabledProperty, value);
        }

        /// <summary>
        /// Getter for <see cref="DependencyProperty"/> <see cref="IsEnabledProperty"/>
        /// </summary>
        public static bool GetIsEnabled(UserControl userControl)
        {
            Guard.AssertNotNull(userControl, "userControl");

            return (bool)userControl.GetValue(IsEnabledProperty);
        }

        #endregion Properties
    }
}
