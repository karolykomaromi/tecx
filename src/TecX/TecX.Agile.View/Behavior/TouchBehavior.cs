﻿using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace TecX.Agile.View.Behavior
{
    public class TouchBehavior : MovementBehaviorBase
    {
        protected override void OnAttached()
        {
            base.OnAttached();

            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
                return;

            AssociatedObject.IsManipulationEnabled = true;

            AssociatedObject.ManipulationStarting += OnManipulationStarting;
            AssociatedObject.ManipulationDelta += OnManipulationDelta;
            AssociatedObject.ManipulationInertiaStarting += OnManipulationInertiaStarting;
        }

        protected override void OnDetaching()
        {
            AssociatedObject.IsManipulationEnabled = false;

            AssociatedObject.ManipulationStarting -= OnManipulationStarting;
            AssociatedObject.ManipulationDelta -= OnManipulationDelta;
            AssociatedObject.ManipulationInertiaStarting -= OnManipulationInertiaStarting;
        }

        #region EventHandling

        private static void OnManipulationInertiaStarting(object sender, ManipulationInertiaStartingEventArgs e)
        {
            // Decrease the velocity of the Rectangle's movement by 
            // 10 inches per second every second.
            // (10 inches * 96 pixels per inch / 1000ms^2)
            e.TranslationBehavior.DesiredDeceleration = 10.0 * 96.0 / (1000.0 * 1000.0);

            // Decrease the velocity of the Rectangle's resizing by 
            // 0.1 inches per second every second.
            // (0.1 inches * 96 pixels per inch / (1000ms^2)
            e.ExpansionBehavior.DesiredDeceleration = 0.1 * 96 / (1000.0 * 1000.0);

            // Decrease the velocity of the Rectangle's rotation rate by 
            // 2 rotations per second every second.
            // (2 * 360 degrees / (1000ms^2)
            e.RotationBehavior.DesiredDeceleration = 720 / (1000.0 * 1000.0);

            e.Handled = true;
        }

        private void OnManipulationDelta(object sender, ManipulationDeltaEventArgs e)
        {
            Matrix matrix = AssociatedObject.Transform().Matrix;

            // Rotate
            matrix.RotateAt(e.DeltaManipulation.Rotation,
                                 e.ManipulationOrigin.X,
                                 e.ManipulationOrigin.Y);

            //TODO weberse dont want to scale the card right now. dont think that is a good idea
            //// Resize the Rectangle.  Keep it square 
            //// so use only the X value of Scale.
            //matrix.ScaleAt(e.DeltaManipulation.Scale.X,
            //               e.DeltaManipulation.Scale.X,
            //               e.ManipulationOrigin.X,
            //               e.ManipulationOrigin.Y);

            //move
            matrix.Translate(e.DeltaManipulation.Translation.X,
                                  e.DeltaManipulation.Translation.Y);

            // Apply the changes
            AssociatedObject.Transform().Matrix = matrix;

            Rect containingRect = new Rect(((FrameworkElement)e.ManipulationContainer).RenderSize);

            Rect shapeBounds = AssociatedObject.RenderTransform.TransformBounds(new Rect(AssociatedObject.RenderSize));

            // Check if the rectangle is completely in the window.
            // If it is not and intertia is occuring, stop the manipulation.
            if (e.IsInertial && !containingRect.Contains(shapeBounds))
            {
                e.Complete();
            }

            e.Handled = true;
        }

        private static void OnManipulationStarting(object sender, ManipulationStartingEventArgs e)
        {
            e.ManipulationContainer = Tabletop.Surface;
            e.Handled = true;
        }

        #endregion EventHandling
    }
}