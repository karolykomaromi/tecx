namespace TecX.Agile.Infrastructure
{
    using System.Windows;

    public class NullSurface : Surface
    {
        protected override IInputElement InputElement
        {
            get { return Application.Current.MainWindow; }
        }

        public override Point GetMousePositionOnSurface()
        {
            return new Point();
        }

        public override Point GetAbsoluteMousePosition()
        {
            return new Point();
        }

        public override Point PointFromScreen(Point point)
        {
            return point;
        }

        public override Point PointToScreen(Point point)
        {
            return point;
        }
    }
}