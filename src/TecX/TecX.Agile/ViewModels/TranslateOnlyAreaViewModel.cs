namespace TecX.Agile.ViewModels
{
    using System.Windows;

    using Caliburn.Micro;

    using TecX.Agile.Utilities;
    using TecX.Common.Comparison;

    public class TranslateOnlyAreaViewModel : ViewAware
    {
        /// <summary>75.0 </summary>
        public const double DefaultRadius = 75.0;

        private double diameter;

        private bool visible;

        private IInputElement inputArea;

        public TranslateOnlyAreaViewModel()
        {
            this.Diameter = 2 * DefaultRadius;
        }

        public IInputElement InputArea
        {
            get
            {
                return this.inputArea;
            }

            set
            {
                if (this.inputArea == value)
                {
                    return;
                }

                this.inputArea = value;
                this.NotifyOfPropertyChange(() => this.InputArea);
            }
        }

        public double Diameter
        {
            get
            {
                return this.diameter;
            }

            set
            {
                if (EpsilonComparer.AreEqual(this.diameter, value))
                {
                    return;
                }

                this.diameter = value;
                this.NotifyOfPropertyChange(() => this.Diameter);
            }
        }

        public bool Visible
        {
            get
            {
                return this.visible;
            }

            set
            {
                if (this.visible == value)
                {
                    return;
                }

                this.visible = value;
                this.NotifyOfPropertyChange(() => this.Visible);
            }
        }

        public bool IsInsideTranslateOnlyArea(Point point)
        {
            Point center = new Point(this.Diameter / 2, this.Diameter / 2);

            double distance = GeometryHelper.GetDistanceBetween(point, center);

            bool inside = distance < (this.Diameter / 2);

            return inside;
        }
        
        protected override void OnViewAttached(object view, object context)
        {
            // ugly but it does the job
            this.InputArea = (IInputElement)LogicalTreeHelper.FindLogicalNode((DependencyObject)view, "Toa");
        }
    }
}
