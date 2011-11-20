namespace TecX.Agile.Behaviors
{
    using System;
    using System.Windows.Controls;
    using System.Windows.Interactivity;

    public class SurfaceBehavior : Behavior<Canvas>
    {
        private static Canvas surface;

        public static Canvas Surface
        {
            get
            {
                if (surface == null)
                {
                    throw new InvalidOperationException("No Canvas is decorated with the SurfaceBehavior.");
                }

                return surface;
            }

            set
            {
                if (surface != null)
                {
                    throw new InvalidOperationException("There are multiple Canvas decorated with the SurfaceBehavior");
                }

                surface = value;
            }
        }

        protected override void OnAttached()
        {
            Surface = AssociatedObject;
        }
    }
}
