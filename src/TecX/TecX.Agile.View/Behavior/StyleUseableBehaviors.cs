using System.Windows;
using System.Windows.Interactivity;

namespace TecX.Agile.View.Behavior
{
    public class StyleUseableBehaviors
    {
        #region DependencyProperties

        public static readonly DependencyProperty BehaviorsProperty = DependencyProperty.RegisterAttached(
            @"Behaviors",
            typeof(StyleUseableBehaviorCollection),
            typeof(StyleUseableBehaviors),
            new FrameworkPropertyMetadata(null, OnPropertyChanged));

        public static StyleUseableBehaviorCollection GetBehaviors(DependencyObject uie)
        {
            return (StyleUseableBehaviorCollection)uie.GetValue(BehaviorsProperty);
        }

        public static void SetBehaviors(DependencyObject uie, StyleUseableBehaviorCollection value)
        {
            uie.SetValue(BehaviorsProperty, value);
        }

        #endregion DependencyProperties

        #region EventHandling

        private static void OnPropertyChanged(DependencyObject dpo, DependencyPropertyChangedEventArgs e)
        {
            var uie = dpo as UIElement;

            if (uie == null)
            {
                return;
            }

            BehaviorCollection itemBehaviors = Interaction.GetBehaviors(uie);

            var newBehaviors = e.NewValue as StyleUseableBehaviorCollection;
            var oldBehaviors = e.OldValue as StyleUseableBehaviorCollection;

            if (newBehaviors == oldBehaviors)
            {
                return;
            }

            if (oldBehaviors != null)
            {
                foreach (var behavior in oldBehaviors)
                {
                    int index = itemBehaviors.IndexOf(behavior);

                    if (index >= 0)
                    {
                        itemBehaviors.RemoveAt(index);
                    }
                }
            }

            if (newBehaviors != null)
            {
                foreach (var behavior in newBehaviors)
                {
                    int index = itemBehaviors.IndexOf(behavior);

                    if (index < 0)
                    {
                        itemBehaviors.Add(behavior);
                    }
                }
            }
        }

        #endregion EventHandling
    }
}
