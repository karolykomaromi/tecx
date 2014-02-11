namespace Infrastructure.Theming
{
    using System.Diagnostics.Contracts;
    using System.Windows;

    public class ThemeStyleAdapter : ThemingAdapter<FrameworkElement>
    {
        public ThemeStyleAdapter(FrameworkElement target, Style style)
            : base(target, style)
        {
        }

        public override void Handle(ThemeChanged message)
        {
            Contract.Requires(message != null);
            Contract.Requires(message.ThemeUri != null);

            Style newStyle = Application.Current.Resources[this.Key] as Style;

            if (newStyle != null)
            {
                this.Target.Style = newStyle;
            }
        }
    }
}