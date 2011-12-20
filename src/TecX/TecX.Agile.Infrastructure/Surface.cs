namespace TecX.Agile.Infrastructure
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    using TecX.Common;

    public interface InputElementAdapter
    {
        IInputElement InputElement { get; }
    }

    public abstract class Surface : InputElementAdapter
    {
        private static Surface current;

        static Surface()
        {
            current = new NullSurface();
        }

        public static Surface Current
        {
            get
            {
                return current;
            }

            set
            {
                Guard.AssertNotNull(value, "Current");

                current = value;
            }
        }

        IInputElement InputElementAdapter.InputElement
        {
            get { return this.InputElement; }
        }

        protected abstract IInputElement InputElement { get; }
        
        public abstract Point GetMousePositionOnSurface();

        public abstract Point GetAbsoluteMousePosition();

        public abstract Point PointFromScreen(Point point);

        public abstract Point PointToScreen(Point point);
    }

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

    public class CanvasSurface : Surface, InputElementAdapter
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
