using System.Windows;

using TecX.Common;

namespace TecX.Agile.View.Behavior
{
    public class RntBehavior : MovementBehaviorBase
    {        #region Properties

        /// <summary>
        /// This is a dependency property.
        /// </summary>
        public static readonly DependencyProperty IsEnabledProperty =
            DependencyProperty.RegisterAttached(
                "IsEnabled",
                typeof (bool),
                typeof (RntBehavior),
                new FrameworkPropertyMetadata(
                    false,
                    FrameworkPropertyMetadataOptions.AffectsArrange |
                    FrameworkPropertyMetadataOptions.AffectsMeasure,
                    OnBehaviorEnabledChanged<RntHandler>));

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
    }
}
