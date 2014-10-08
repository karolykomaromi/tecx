namespace TecX.Agile.Phone.Triggers
{
    using System;
    using System.Windows;
    using System.Windows.Data;

    public class DependencyPropertyListener
    {
        private static int index;
        private readonly DependencyProperty property;
        private FrameworkElement target;

        public DependencyPropertyListener()
        {
            this.property = DependencyProperty.RegisterAttached(
                "DependencyPropertyListener" + index++,
                typeof(object),
                typeof(DependencyPropertyListener),
                new PropertyMetadata(null, this.OnValueChanged));
        }

        public event EventHandler<BindingChangedEventArgs> Changed = delegate { };

        public void Attach(FrameworkElement element, Binding binding)
        {
            if (this.target != null)
            {
                throw new Exception("Cannot attach an already attached listener");
            }

            this.target = element;
            this.target.SetBinding(this.property, binding);
        }

        public void Detach()
        {
            this.target.ClearValue(this.property);
            this.target = null;
        }

        private void OnValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            this.Changed(this.target, new BindingChangedEventArgs(e));
        }
    }
}