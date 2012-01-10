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

            return new ScrollChangedMessageParameter();
        }
    }
}