namespace TecX.Agile.Phone.Triggers
{
    using System;
    using System.Windows;

    public class BindingChangedEventArgs : EventArgs
    {
        public BindingChangedEventArgs(DependencyPropertyChangedEventArgs e)
        {
            this.EventArgs = e;
        }

        public DependencyPropertyChangedEventArgs EventArgs { get; private set; }
    }
}
