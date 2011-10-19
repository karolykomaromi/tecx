namespace TecX.Agile.Modules.Gestures.Views
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Ink;

    /// <summary>
    /// Interaction logic for GestureView.xaml
    /// </summary>
    public partial class GestureView : UserControl
    {
        public GestureView()
        {
            InitializeComponent();

            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            InkCanvas canvas = sender as InkCanvas;

            if (canvas != null &&
                canvas.IsGestureRecognizerAvailable)
            {
                canvas.SetEnabledGestures(new[]
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
    }
}
