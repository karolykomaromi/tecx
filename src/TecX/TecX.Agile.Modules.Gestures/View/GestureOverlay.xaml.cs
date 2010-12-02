using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using TecX.Agile.Modules.Gestures.ViewModel;
using TecX.Common;

namespace TecX.Agile.Modules.Gestures.View
{
    /// <summary>
    /// Interaction logic for GestureOverlay.xaml
    /// </summary>
    public partial class GestureOverlay : UserControl
    {
        #region Fields

        private readonly GestureRecognition _gestureRecognition;

        #endregion Fields

        #region c'tor

        public GestureOverlay()
        {
            InitializeComponent();
        }

        public GestureOverlay(GestureRecognition gestureRecognition)
            : this()
        {
            Guard.AssertNotNull(gestureRecognition, "gestureRecognition");

            DataContext = _gestureRecognition = gestureRecognition;
        }

        #endregion c'tor

        #region EventHandling

        private void OnGestureRecognized(object sender, InkCanvasGestureEventArgs e)
        {
            _gestureRecognition.OnGestureRecognized(e);
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (_canvas != null &&
                _canvas.IsGestureRecognizerAvailable)
            {
                _canvas.SetEnabledGestures(new[]
                                               {
                                                   ApplicationGesture.ChevronDown,
                                                   ApplicationGesture.ChevronLeft,
                                                   ApplicationGesture.ChevronRight,
                                                   ApplicationGesture.ChevronUp,
                                                   ApplicationGesture.Square
                                               });
            }
            else
            {
                throw new InvalidOperationException("No gesture recognition available on this machine!");
            }
        }

        #endregion EventHandling
    }
}
