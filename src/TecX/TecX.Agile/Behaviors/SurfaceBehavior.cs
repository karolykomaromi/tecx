using System;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace TecX.Agile.Behaviors
{
    public class SurfaceBehavior : Behavior<Canvas>
    {
        private static Canvas _surface;

        public static Canvas Surface
        {
            get
            {
                if(_surface == null)
                {
                    throw new InvalidOperationException("No Canvas is decorated with the SurfaceBehavior.");
                }

                return _surface;
            }

            set
            {
                if(_surface != null)
                {
                    throw new InvalidOperationException("There are multiple Canvas decorated with the SurfaceBehavior");
                }

                _surface = value;
            }
        }

        protected override void OnAttached()
        {
            Surface = AssociatedObject;
        }
    }
}
