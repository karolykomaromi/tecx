namespace TecX.CaliburnEx
{
    using System;

    using System.Windows.Controls;

    public class ScrollChangedConverter
    {
        public object Convert(object providedValue, object context)
        {
            ScrollChangedEventArgs eventArgs = providedValue as ScrollChangedEventArgs;

            if (eventArgs == null)
            {
                throw new ArgumentException("Provided value must be of type ScrollChangedEventArgs.", "providedValue");
            }

            return new ScrollChangedParameter
                {
                    ExtentHeight = eventArgs.ExtentHeight,
                    ExtentWidth = eventArgs.ExtentWidth,
                    HorizontalOffset = eventArgs.HorizontalOffset,
                    VerticalOffset = eventArgs.VerticalOffset,
                    ViewportHeight = eventArgs.ViewportHeight,
                    ViewportWidth = eventArgs.ViewportWidth
                };
        }
    }
}