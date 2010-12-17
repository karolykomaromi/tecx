using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

using TecX.Common;

namespace TecX.Agile.View.Behavior
{
    public class MovementBehaviorBase : BehaviorBase
    {
        #region DependencyProperties

        /// <summary>
        /// Value indicating wether a <see cref="UIElement"/> should be considered
        /// as immobile. This is a <see cref="DependencyProperty"/>
        /// </summary>
        public static readonly DependencyProperty IsPinnedProperty =
            DependencyProperty.RegisterAttached(
                "IsPinned",
                typeof(bool),
                typeof(MovementBehaviorBase),
                new FrameworkPropertyMetadata(false));


        /// <summary>
        /// Getter for <see cref="DependencyProperty"/> <see cref="IsPinnedProperty"/>
        /// </summary>
        public static bool GetIsPinned(DependencyObject dependencyObject)
        {
            Guard.AssertNotNull(dependencyObject, "dependencyObject");

            return (bool)dependencyObject.GetValue(IsPinnedProperty);
        }

        /// <summary>
        /// Setter for <see cref="DependencyProperty"/> <see cref="IsPinnedProperty"/>
        /// </summary>
        public static void SetIsPinned(DependencyObject dependencyObject, bool value)
        {
            Guard.AssertNotNull(dependencyObject, "dependencyObject");

            dependencyObject.SetValue(IsPinnedProperty, value);
        }

        #endregion DependencyProperties

        protected static void OnMovementBehaviorEnabledChanged<THandler>(DependencyObject dependencyObject,
                                                                 DependencyPropertyChangedEventArgs args)
            where THandler : IBehaviorHandler, new()
        {
            FrameworkElement element = dependencyObject as FrameworkElement;

            if(element != null)
            {
                AssertPreconditions(element);
            }
            else
            {
                throw new InvalidOperationException("Can't attach a MovementBehavior to an object that is not a FrameworkElement");
            }

            OnBehaviorEnabledChanged<THandler>(dependencyObject, args);
        }
        
        private static void AssertPreconditions(FrameworkElement element)
        {
            Guard.AssertNotNull(element, "element");

            if(element.RenderTransform == null || element.RenderTransform == Transform.Identity)
            {
                element.RenderTransform = new MatrixTransform(Matrix.Identity);
            }

            SetAttachedHandlers(element, new List<IBehaviorHandler>());
        }
    }
}
