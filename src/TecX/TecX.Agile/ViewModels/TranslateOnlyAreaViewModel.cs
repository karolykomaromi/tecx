namespace TecX.Agile.ViewModels
{
    using System.Windows;
    using System.Windows.Media;

    using Caliburn.Micro;

    using TecX.Agile.Behaviors;

    public class TranslateOnlyAreaViewModel : Screen
    {
        /// <summary>75.0 </summary>
        public const double DefaultRadius = 75.0;

        private double _width;

        private double _height;

        private bool _visible;

        private IInputElement _inputArea;

        public double Width
        {
            get
            {
                return _width;
            }

            set
            {
                if (_width == value)
                {
                    return;
                }

                _width = value;
                NotifyOfPropertyChange(() => Width);
            }
        }

        public double Height
        {
            get
            {
                return _height;
            }

            set
            {
                if (_height == value)
                {
                    return;
                }

                _height = value;
                NotifyOfPropertyChange(() => Height);
            }
        }

        public bool Visible
        {
            get
            {
                return _visible;
            }

            set
            {
                if (_visible == value)
                {
                    return;
                }

                _visible = value;
                NotifyOfPropertyChange(() => Visible);
            }
        }

        public IInputElement InputArea
        {
            get
            {
                return _inputArea;
            }

            set
            {
                if (_inputArea == value)
                {
                    return;
                }

                _inputArea = value;
                NotifyOfPropertyChange(() => InputArea);
            }
        }

        protected override void OnViewAttached(object view, object context)
        {
            // ugly but it does the job
            InputArea = (IInputElement)LogicalTreeHelper.FindLogicalNode((DependencyObject)view, "Toa");
        }

        public TranslateOnlyAreaViewModel()
        {
            Width = 2 * DefaultRadius;
            Height = 2 * DefaultRadius;
        }

        public bool IsInsideTranslateOnlyArea(Point point)
        {
            Point center = new Point(Width / 2, Height / 2);

            double distance = GeometryHelper.GetDistanceBetween(point, center);

            //Rect adornedElementRect = new Rect(AdornedElement.DesiredSize);

            //Point center = GeometryHelper.CenterPoint(adornedElementRect);

            //double distance = GeometryHelper.GetDistanceBetween(p, center);

            bool inside = distance < DefaultRadius;

            return inside;
        }
    }
}
