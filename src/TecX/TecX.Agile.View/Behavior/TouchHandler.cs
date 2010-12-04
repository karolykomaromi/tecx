using System.Windows;
using System.Windows.Input;

namespace TecX.Agile.View.Behavior
{
    public class TouchHandler : BehaviorHandler
    {
        protected override void DoAttach(FrameworkElement element)
        {
            element.IsManipulationEnabled = true;

            element.ManipulationStarting += OnManipulationStarting;
            element.ManipulationDelta += OnManipulationDelta;
            element.ManipulationInertiaStarting += OnManipulationInertiaStarting;
        }

        protected override void DoDetach()
        {
            Element.IsManipulationEnabled = false;

            Element.ManipulationStarting -= OnManipulationStarting;
            Element.ManipulationDelta -= OnManipulationDelta;
            Element.ManipulationInertiaStarting -= OnManipulationInertiaStarting;
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
            // Get the Rectangle and its RenderTransform matrix.

            //Rectangle rectToMove = e.OriginalSource as Rectangle;
            //Matrix rectsMatrix = ((MatrixTransform)rectToMove.RenderTransform).Matrix;

            Element.Rotation().Angle += e.DeltaManipulation.Rotation;
            //TODO weberse what about using another center for the rotation?

            //// Rotate the Rectangle.
            //rectsMatrix.RotateAt(e.DeltaManipulation.Rotation,
            //                     e.ManipulationOrigin.X,
            //                     e.ManipulationOrigin.Y);


            //// Resize the Rectangle.  Keep it square 
            //// so use only the X value of Scale.
            //rectsMatrix.ScaleAt(e.DeltaManipulation.Scale.X,
            //                    e.DeltaManipulation.Scale.X,
            //                    e.ManipulationOrigin.X,
            //                    e.ManipulationOrigin.Y);
            //TODO weberse dont want to scale the card right now. dont think that is a good idea


            //// Move the Rectangle.
            //rectsMatrix.Translate(e.DeltaManipulation.Translation.X,
            //                      e.DeltaManipulation.Translation.Y);
            Element.Translation().X += e.DeltaManipulation.Translation.X;
            Element.Translation().Y += e.DeltaManipulation.Translation.Y;


            // Apply the changes to the Rectangle.
            //rectToMove.RenderTransform = new MatrixTransform(rectsMatrix);

            Rect containingRect =
                new Rect(((FrameworkElement)e.ManipulationContainer).RenderSize);

            Rect shapeBounds =
                Element.RenderTransform.TransformBounds(
                    new Rect(Element.RenderSize));

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