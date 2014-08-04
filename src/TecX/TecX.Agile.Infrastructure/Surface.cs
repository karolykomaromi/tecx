namespace TecX.Agile.Infrastructure
{
    using System.Windows;

    using TecX.Common;

    public abstract class Surface : IInputElementAdapter
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

        IInputElement IInputElementAdapter.InputElement
        {
            get { return this.InputElement; }
        }

        protected abstract IInputElement InputElement { get; }
        
        public abstract Point GetMousePositionOnSurface();

        public abstract Point GetAbsoluteMousePosition();

        public abstract Point PointFromScreen(Point point);

        public abstract Point PointToScreen(Point point);
    }
}
