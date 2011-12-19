namespace TecX.Agile.Behaviors
{
    using System;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Interactivity;
    using System.Windows.Media;

    using TecX.Agile.Utilities;
    using TecX.Common.Extensions.Error;

    public class TransformBehavior : Behavior<UserControl>
    {
        protected override void OnAttached()
        {
            if (DesignerProperties.GetIsInDesignMode(AssociatedObject))
            {
                return;
            }

            var transform = AssociatedObject.RenderTransform;

            if (transform == null ||
                transform == Transform.Identity)
            {
                var transformGroup = new TransformGroup();

                transformGroup.Children.Add(new RotateTransform());
                transformGroup.Children.Add(new ScaleTransform());
                transformGroup.Children.Add(new TranslateTransform());

                AssociatedObject.RenderTransform = transformGroup;
            }

            this.AssertPreconditions();

            Binding x = new Binding("X")
            {
                Source = AssociatedObject.DataContext,
                Mode = BindingMode.TwoWay,
                NotifyOnSourceUpdated = true,
                NotifyOnTargetUpdated = true
            };

            BindingExpressionBase hookForInitialUpdate = BindingOperations.SetBinding(
                AssociatedObject.Translation(),
                TranslateTransform.XProperty,
                x);

            hookForInitialUpdate.UpdateTarget();

            Binding y = new Binding("AbsoluteY")
            {
                Source = AssociatedObject.DataContext,
                Mode = BindingMode.TwoWay,
                NotifyOnSourceUpdated = true,
                NotifyOnTargetUpdated = true
            };

            hookForInitialUpdate = BindingOperations.SetBinding(
                AssociatedObject.Translation(),
                TranslateTransform.YProperty,
                y);

            hookForInitialUpdate.UpdateTarget();

            Binding angle = new Binding("Angle")
            {
                Source = AssociatedObject.DataContext,
                Mode = BindingMode.TwoWay,
                NotifyOnSourceUpdated = true,
                NotifyOnTargetUpdated = true
            };

            hookForInitialUpdate = BindingOperations.SetBinding(
                AssociatedObject.Rotation(),
                RotateTransform.AngleProperty,
                angle);

            hookForInitialUpdate.UpdateTarget();

            this.AssociatedObject.Loaded += this.OnLoaded;
        }

        private static void ThrowTransformsException(Transform transform)
        {
            throw new InvalidOperationException("The UIElement's RenderTransform " +
                                                "property is not set to a TransformGroup containing a " +
                                                "RotateTransform, ScaleTransform and TranslateTransform. " +
                                                "The order of the elements in the group matters!")
                .WithAdditionalInfo("transform", transform);
        }

        private void AssertPreconditions()
        {
            var group = AssociatedObject.RenderTransform as TransformGroup;

            if (group == null)
            {
                ThrowTransformsException(null);
            }

            int count = group.Children.Count;

            if (count != 3)
            {
                ThrowTransformsException(group);
            }

            RotateTransform rotation = group.Children[0] as RotateTransform;

            if (rotation == null)
            {
                ThrowTransformsException(group);
            }

            ScaleTransform scale = group.Children[1] as ScaleTransform;

            if (scale == null)
            {
                ThrowTransformsException(group);
            }

            TranslateTransform translation = group.Children[2] as TranslateTransform;

            if (translation == null)
            {
                ThrowTransformsException(group);
            }
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (DesignerProperties.GetIsInDesignMode(AssociatedObject))
            {
                return;
            }

            var rotation = AssociatedObject.Rotation();

            var center = AssociatedObject.Center();

            rotation.CenterX = center.X;
            rotation.CenterY = center.Y;

            var scale = AssociatedObject.Scale();

            scale.CenterX = center.X;
            scale.CenterY = center.Y;

            this.AssociatedObject.Loaded -= this.OnLoaded;
        }
    }
}
