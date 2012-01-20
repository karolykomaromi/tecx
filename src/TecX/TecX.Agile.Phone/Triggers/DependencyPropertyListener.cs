namespace TecX.Agile.Phone.Triggers
{
    using System;
    using System.Windows;
    using System.Windows.Data;

    public class DependencyPropertyListener
    {
        static int index;
        readonly DependencyProperty property;
        FrameworkElement target;

        public DependencyPropertyListener()
        {
            this.property = DependencyProperty.RegisterAttached(
                "DependencyPropertyListener" + index++,
                typeof(object),
                typeof(DependencyPropertyListener),
                new PropertyMetadata(null, this.HandleValueChanged));
        }

        public event EventHandler<BindingChangedEventArgs> Changed;

        void HandleValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            this.OnChanged(new BindingChangedEventArgs(e));
        }

        protected void OnChanged(BindingChangedEventArgs e)
        {
            var temp = this.Changed;
            if (temp != null)
            {
                temp(this.target, e);
            }
        }

        public void Attach(FrameworkElement element, Binding binding)
        {
            if (this.target != null)
            {
                throw new Exception(
                    "Cannot attach an already attached listener");
            }

            this.target = element;
            this.target.SetBinding(this.property, binding);
        }

        public void Detach()
        {
            this.target.ClearValue(this.property);
            this.target = null;
        }
    }
}