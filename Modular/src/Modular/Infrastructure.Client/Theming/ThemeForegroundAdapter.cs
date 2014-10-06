namespace Infrastructure.Theming
{
    using System.Diagnostics.Contracts;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    public class ThemeForegroundAdapter : ThemingAdapter<Control>
    {
        public ThemeForegroundAdapter(Control control, Brush objectFromTheme)
            : base(control, objectFromTheme)
        {
        }

        public override void Handle(ThemeChanged message)
        {
            Contract.Requires(message != null);

            Brush newBrush = Application.Current.Resources[this.Key] as Brush;

            if (newBrush != null)
            {
                this.Target.Foreground = newBrush;
            }
        }
    }
}