namespace Infrastructure.Theming
{
    using System.Diagnostics.Contracts;
    using System.Windows;

    public class ThemeStyleAdapter
    {
        private readonly FrameworkElement element;
        private readonly object styleKey;

        public ThemeStyleAdapter(FrameworkElement element, object styleKey)
        {
            Contract.Requires(element != null);
            Contract.Requires(styleKey != null);

            this.element = element;
            this.styleKey = styleKey;
        }

        public void OnThemeChanged(object sender, ThemeChangedEventArgs args)
        {
            Contract.Requires(args != null);
            Contract.Requires(args.ThemeUri != null);

            Style newStyle = Application.Current.Resources[this.styleKey] as Style;

            if (newStyle != null)
            {
                this.element.Style = newStyle;
            }
        }
    }
}