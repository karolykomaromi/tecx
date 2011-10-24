using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TecX.Agile.ViewModels
{
    /// <summary>
    /// Interaction logic for TranslateOnlyAreaView.xaml
    /// </summary>
    public partial class TranslateOnlyAreaView : UserControl
    {
        public TranslateOnlyAreaView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Checks wether a given point is inside the <see cref="TranslateOnlyAdorner"/>
        /// </summary>
        /// <param name="p">The point</param>
        /// <returns><i>true</i> if the point is inside the <see cref="TranslateOnlyAdorner"/>; <i>false</i> otherwise</returns>
        public bool IsInsideTranslateOnlyArea(Point p)
        {
            //Rect adornedElementRect = new Rect(AdornedElement.DesiredSize);

            //Point center = GeometryHelper.CenterPoint(adornedElementRect);

            //double distance = GeometryHelper.GetDistanceBetween(p, center);

            //bool inside = distance < DefaultRadius;

            //return inside;

            return false;
        }
    }
}
