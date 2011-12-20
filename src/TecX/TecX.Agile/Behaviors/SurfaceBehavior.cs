namespace TecX.Agile.Behaviors
{
    using System.Windows.Controls;
    using System.Windows.Interactivity;

    using TecX.Agile.Infrastructure;

    public class SurfaceBehavior : Behavior<Canvas>
    {
        protected override void OnAttached()
        {
            Surface.Current = new CanvasSurface(AssociatedObject);
        }
    }
}
