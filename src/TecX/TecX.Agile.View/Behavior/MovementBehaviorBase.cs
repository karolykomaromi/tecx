using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;

using TecX.Common;
using TecX.Common.Extensions.Error;

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

        //transformgroup rotate, scale, translate
        /// <summary>
        /// Asserts that certain preconditions to use a <see cref="FrameworkElement"/>
        /// as a host for this behavior are met. First of all the element's
        /// <see cref="UIElement.RenderTransform"/> property must be set to a
        /// <see cref="TransformGroup"/> containing a <see cref="RotateTransform"/>,
        /// a <see cref="ScaleTransform"/> and a <see cref="TranslateTransform"/>
        /// as children. The order of these children is important!
        /// </summary>
        /// <param name="element">The element.</param>
        private static void AssertPreconditions(FrameworkElement element)
        {
            Guard.AssertNotNull(element, "element");

            if(element.RenderTransform == null || element.RenderTransform == Transform.Identity)
            {
                element.RenderTransform = new MatrixTransform(Matrix.Identity);
            }

            SetAttachedHandlers(element, new List<IBehaviorHandler>());
        }

        /// <summary>
        /// Throws an exception that explains the preconditions for using
        /// this <see cref="UIElement"/> as a host for this behavior
        /// </summary>
        /// <param name="transform">The transform.</param>
        private static void ThrowTransformsException(Transform transform)
        {
            throw new InvalidOperationException("The UIElement's RenderTransform " +
                                                "property is not set to a TransformGroup containing a " +
                                                "RotateTransform, ScaleTransform and TranslateTransform. " +
                                                "The order of the elements in the group matter!")
                .WithAdditionalInfo("transform", transform);
        }
    }
}
