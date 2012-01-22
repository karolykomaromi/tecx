namespace System.Windows.Controls
{
    using System.Windows;

    public class ScrollChangedEventArgs : RoutedEventArgs
    {
        public double ExtentHeight { get; set; }

        public double ExtensHeightChange { get; set; }

        public double ExtentWidth { get; set; }

        public double ExtentWidthChange { get; set; }

        public double HorizontalChange { get; set; }

        public double HorizontalOffset { get; set; }

        public double VerticalOffset { get; set; }

        public double ViewportHeight { get; set; }

        public double ViewportHeightChange { get; set; }

        public double ViewportWidthChange { get; set; }

        public double ViewportWidth { get; set; }
    }
}