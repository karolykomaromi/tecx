namespace Infrastructure.Dynamic
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Windows;

    public abstract class FrameworkElementAdapter : IControl
    {
        private readonly WeakReference reference;
        private readonly ControlId id;

        protected FrameworkElementAdapter(FrameworkElement element)
        {
            Contract.Requires(element != null);
            Contract.Requires(!string.IsNullOrEmpty(element.Name));

            this.reference = new WeakReference(element);

            string name = element.Name;

            if (string.IsNullOrEmpty(name))
            {
                name = element.GetType().Name;
            }

            this.id = new ControlId(name);
        }

        public virtual ControlId Id
        {
            get { return this.id; }
        }

        public virtual bool IsAlive
        {
            get { return this.Reference.IsAlive; }
        }

        protected virtual WeakReference Reference
        {
            get { return this.reference; }
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

        public abstract bool TryFindById(ControlId id, out IControl control);

        public abstract void Enable();

        public abstract void Disable();
    }
}