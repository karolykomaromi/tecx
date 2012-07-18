using System.Windows;
using System.Windows.Interactivity;

using TecX.Common;

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

        public static StyleUseableBehaviorCollection GetBehaviors(DependencyObject obj)
        {
            Guard.AssertNotNull(obj, "obj");

            return (StyleUseableBehaviorCollection)obj.GetValue(BehaviorsProperty);
        }

        public static void SetBehaviors(DependencyObject obj, StyleUseableBehaviorCollection value)
        {
            Guard.AssertNotNull(obj, "obj");

            obj.SetValue(BehaviorsProperty, value);
        }

        private static readonly DependencyProperty OriginalBehaviorProperty = DependencyProperty.RegisterAttached(
            @"OriginalBehaviorInternal",
            typeof(System.Windows.Interactivity.Behavior),
            typeof(StyleUseableBehaviors),
            new UIPropertyMetadata(null));

        private static System.Windows.Interactivity.Behavior GetOriginalBehavior(DependencyObject obj)
        {
            Guard.AssertNotNull(obj, "obj");

            return obj.GetValue(OriginalBehaviorProperty) as System.Windows.Interactivity.Behavior;
        }

        private static void SetOriginalBehavior(DependencyObject obj, System.Windows.Interactivity.Behavior value)
        {
            Guard.AssertNotNull(obj, "obj");

            obj.SetValue(OriginalBehaviorProperty, value);
        }

        #endregion DependencyProperties

        private static int GetIndexOf(BehaviorCollection itemBehaviors, System.Windows.Interactivity.Behavior behavior)
        {
            int index = -1;

            System.Windows.Interactivity.Behavior orignalBehavior = GetOriginalBehavior(behavior);

            for (int i = 0; i < itemBehaviors.Count; i++)
            {
                System.Windows.Interactivity.Behavior currentBehavior = itemBehaviors[i];

                if (currentBehavior == behavior ||
                    currentBehavior == orignalBehavior)
                {
                    return i;
                }

                System.Windows.Interactivity.Behavior currentOrignalBehavior = GetOriginalBehavior(currentBehavior);

                if (currentOrignalBehavior == behavior ||
                    currentOrignalBehavior == orignalBehavior)
                {
                    return i;
                }
            }

            return index;
        }

        private static void OnPropertyChanged(DependencyObject dpo, DependencyPropertyChangedEventArgs e)
        {
            var uie = dpo as UIElement;

            if (uie == null)
                return;

            BehaviorCollection itemBehaviors = Interaction.GetBehaviors(uie);

            var newBehaviors = e.NewValue as StyleUseableBehaviorCollection;
            var oldBehaviors = e.OldValue as StyleUseableBehaviorCollection;

            if (newBehaviors == oldBehaviors)
                return;

            if (oldBehaviors != null)
            {
                foreach (var behavior in oldBehaviors)
                {
                    int index = GetIndexOf(itemBehaviors, behavior);

                    if (index >= 0)
                    {
                        itemBehaviors.RemoveAt(index);
                    }
                }
            }

            if (newBehaviors != null)
            {
                foreach (System.Windows.Interactivity.Behavior behavior in newBehaviors)
                {
                    int index = GetIndexOf(itemBehaviors, behavior);

                    if (index < 0)
                    {
                        var clone = (System.Windows.Interactivity.Behavior)behavior.Clone();
                        SetOriginalBehavior(clone, behavior);
                        itemBehaviors.Add(clone);
                    }
                }
            }
        }
    }
}
