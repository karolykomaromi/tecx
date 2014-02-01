namespace Infrastructure.Dynamic
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Windows;

    public class FrameworkElementAdapter : IControl
    {
        private readonly IControlAdapterFactory factory;

        private readonly WeakReference reference;

        public FrameworkElementAdapter(FrameworkElement element, IControlAdapterFactory factory)
        {
            Contract.Requires(element != null);
            Contract.Requires(!string.IsNullOrEmpty(element.Name));

            this.reference = new WeakReference(element);
            this.factory = factory;

            string name = element.Name;

            if (string.IsNullOrEmpty(name))
            {
                name = element.GetType().Name;
            }

            this.Id = new ControlId(name);
        }

        public ControlId Id { get; protected set; }

        public virtual bool IsAlive
        {
            get { return this.Reference.IsAlive; }
        }

        protected virtual WeakReference Reference
        {
            get { return this.reference; }
        }

        protected virtual IControlAdapterFactory Factory
        {
            get { return this.factory; }
        }

        public virtual void Show()
        {
            UIElement element = this.Reference.Target as UIElement;

            if (element != null)
            {
                element.Visibility = Visibility.Visible;
            }
        }

        public virtual void Hide()
        {
            UIElement element = this.Reference.Target as UIElement;

            if (element != null)
            {
                element.Visibility = Visibility.Collapsed;
            }
        }

        public virtual bool TryFindById(ControlId id, out IControl control)
        {
            FrameworkElement element = this.Reference.Target as FrameworkElement;

            if (element != null)
            {
                FrameworkElement found = element.FindName(id.ToString()) as FrameworkElement;

                if (found != null)
                {
                    control = this.Factory.CreateAdapter(found);
                    return true;
                }
            }

            control = null;
            return false;
        }

        public virtual void Enable()
        {
        }

        public virtual void Disable()
        {
        }
    }
}