using System.Windows;

using Caliburn.Micro;

namespace TecX.Agile.ViewModels
{
    public class TranslateOnlyAreaViewModel : Screen
    {
        /// <summary>75.0 </summary>
        public const double DefaultRadius = 75.0;

        private double diameter;

        private bool _visible;

        private IInputElement _inputArea;

        public double Diameter
        {
            get
            {
                return this.diameter;
            }

            set
            {
                if (diameter == value)
                {
                    return;
                }

                diameter = value;
                NotifyOfPropertyChange(() => Diameter);
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
            Diameter = 2 * DefaultRadius;
        }

        public bool IsInsideTranslateOnlyArea(Point point)
        {
            Point center = new Point(Diameter / 2, Diameter / 2);

            double distance = GeometryHelper.GetDistanceBetween(point, center);

            bool inside = distance < (Diameter / 2);

            return inside;
        }
    }
}
