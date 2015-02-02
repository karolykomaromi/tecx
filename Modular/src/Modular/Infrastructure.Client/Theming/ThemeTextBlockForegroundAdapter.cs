namespace Infrastructure.Theming
{
    using System.Diagnostics.Contracts;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    public class ThemeTextBlockForegroundAdapter : ThemingAdapter<TextBlock>
    {
        public ThemeTextBlockForegroundAdapter(TextBlock textBlock, Brush objectFromTheme)
            : base(textBlock, objectFromTheme)
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