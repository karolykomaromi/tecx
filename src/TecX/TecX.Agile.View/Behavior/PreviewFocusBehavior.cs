using System.Windows;

using TecX.Common;

namespace TecX.Agile.View.Behavior
{
    /// <summary>
    /// Sets the focus to an element when the left mouse button is pressed.
    /// Used when a transparent element is on top of the current one which and 
    /// cannot be invisible to hit testing
    /// </summary>
    public class PreviewFocusBehavior : BehaviorBase
    {
        #region Properties

        /// <summary>
        /// This is a dependency property.
        /// </summary>
        public static readonly DependencyProperty IsEnabledProperty =
            DependencyProperty.RegisterAttached(
                "IsEnabled",
                typeof (bool),
                typeof (PreviewFocusBehavior),
                new FrameworkPropertyMetadata(false, OnBehaviorEnabledChanged<PreviewFocusHandler>));

        /// <summary>
        /// Setter for <see cref="DependencyProperty"/> <see cref="IsEnabledProperty"/>
        /// </summary>
        public static void SetIsEnabled(DependencyObject dependencyObject, bool value)
        {
            Guard.AssertNotNull(dependencyObject, "dependencyObject");

            dependencyObject.SetValue(IsEnabledProperty, value);
        }

        /// <summary>
        /// Getter for <see cref="DependencyProperty"/> <see cref="IsEnabledProperty"/>
        /// </summary>
        public static bool GetIsEnabled(DependencyObject dependencyObject)
        {
            Guard.AssertNotNull(dependencyObject, "dependencyObject");

            return (bool) dependencyObject.GetValue(IsEnabledProperty);
        }

        #endregion Properties

        #region c'tor

        /// <summary>
        /// Initializes a new instance of the <see cref="PreviewFocusBehavior"/> class
        /// </summary>
        public PreviewFocusBehavior()
        {
        }

        #endregion c'tor

    }
}