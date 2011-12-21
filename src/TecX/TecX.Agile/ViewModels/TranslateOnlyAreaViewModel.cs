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

        private TranslateOnlyArea inputArea;

        public TranslateOnlyAreaViewModel()
        {
            this.Diameter = 2 * DefaultRadius;
            this.inputArea = new NullTranslateOnlyArea();
        }

        public TranslateOnlyArea InputArea
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
            ////this.InputArea = new InputElementTranslateOnlyArea((IInputElement)LogicalTreeHelper.FindLogicalNode((DependencyObject)view, "Toa"));
            DependencyObject depObj = view as DependencyObject;

            if (depObj == null)
            {
                return;
            }

            var node = LogicalTreeHelper.FindLogicalNode(depObj, "Toa");

            if (node == null)
            {
                return;
            }

            var element = node as IInputElement;

            if (element != null)
            {
                this.InputArea = new InputElementTranslateOnlyArea(element);
            }
        }
    }
}
