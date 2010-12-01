using System.Windows;

using TecX.Common;

namespace TecX.Agile.View.Behavior
{
    public class RntBehavior : MovementBehaviorBase
    {        
        #region Properties

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
                    OnMovementBehaviorEnabledChanged<RntHandler>));

        /// <summary>
        /// Setter for <see cref="DependencyProperty"/> <see cref="IsEnabledProperty"/>
        /// </summary>
        public static void SetIsEnabled(StoryCard storyCard, bool value)
        {
            Guard.AssertNotNull(storyCard, "storyCard");

            storyCard.SetValue(IsEnabledProperty, value);
        }

        /// <summary>
        /// Getter for <see cref="DependencyProperty"/> <see cref="IsEnabledProperty"/>
        /// </summary>
        public static bool GetIsEnabled(StoryCard storyCard)
        {
            Guard.AssertNotNull(storyCard, "storyCard");

            return (bool) storyCard.GetValue(IsEnabledProperty);
        }

        #endregion Properties
    }
}
