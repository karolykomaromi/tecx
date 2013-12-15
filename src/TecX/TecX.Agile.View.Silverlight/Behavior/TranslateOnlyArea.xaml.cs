using System.Windows;
using System.Windows.Controls;

namespace TecX.Agile.View.Behavior
{
    /// <summary>
    /// Interaction logic for TranslateOnlyArea.xaml
    /// </summary>
    public partial class TranslateOnlyArea : UserControl
    {
        /// <summary>75</summary>
        public const double DefaultRadius = 75;

        /// <summary>
        /// Initializes a new instance of the <see cref="TranslateOnlyArea"/> class.
        /// </summary>
        public TranslateOnlyArea()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Checks wether a given point is inside the <see cref="TranslateOnlyArea"/>
        /// </summary>
        /// <param name="p">The point</param>
        /// <returns><i>true</i> if the point is inside the <see cref="TranslateOnlyArea"/>; <i>false</i> otherwise</returns>
        public bool IsInsideTranslateOnlyArea(Point p)
        {
            Point center = GeometryHelper.CenterPoint(this);

            double distance = GeometryHelper.GetDistanceBetween(p, center);

            double radius = GeometryHelper.GetWidth(this) / 2;

            bool inside = distance < radius;

            return inside;
        }
    }
}
