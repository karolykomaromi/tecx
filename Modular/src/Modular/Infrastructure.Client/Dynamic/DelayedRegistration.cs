namespace Infrastructure.Dynamic
{
    using System;
    using System.Windows;

    public class DelayedRegistration
    {
        private readonly IViewRegistry registry;

        private readonly FrameworkElement element;

        public DelayedRegistration(IViewRegistry registry, FrameworkElement element)
        {
            this.registry = registry;
            this.element = element;

            this.element.LayoutUpdated += this.OnLayoutUpdated;
        }

        private void OnLayoutUpdated(object sender, EventArgs e)
        {
            this.element.LayoutUpdated -= this.OnLayoutUpdated;

            this.registry.Register(this.element);
        }
    }
}
