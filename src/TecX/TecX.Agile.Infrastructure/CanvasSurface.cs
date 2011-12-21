namespace TecX.Agile.Infrastructure
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    using TecX.Common;

    public class CanvasSurface : Surface, IInputElementAdapter
    {
        private readonly Canvas canvas;

        public CanvasSurface(Canvas canvas)
        {
            Guard.AssertNotNull(canvas, "canvas");

            this.canvas = canvas;
        }

        protected override IInputElement InputElement
        {
            get
            {
                return this.canvas;
            }
        }

        public override Point GetMousePositionOnSurface()
        {
            return Mouse.GetPosition(this.canvas);
        }

        public override Point GetAbsoluteMousePosition()
        {
            var relative = Mouse.GetPosition(this.canvas);

            var absolute = this.canvas.PointToScreen(relative);

            return absolute;
        }

        public override Point PointFromScreen(Point point)
        {
            return this.canvas.PointFromScreen(point);
        }

        public override Point PointToScreen(Point point)
        {
            return this.canvas.PointToScreen(point);
        }
    }
}