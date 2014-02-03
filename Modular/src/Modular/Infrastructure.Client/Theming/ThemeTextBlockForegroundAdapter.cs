namespace Infrastructure.Theming
{
    using System.Diagnostics.Contracts;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    public class ThemeTextBlockForegroundAdapter : ThemingAdapter<TextBlock>
    {
        public ThemeTextBlockForegroundAdapter(TextBlock textBlock, Brush objectFromTheme)
        {
            Contract.Requires(textBlock != null);
            Contract.Requires(objectFromTheme != null);

            this.Target = textBlock;
            this.ObjectFromTheme = objectFromTheme;

            this.Target.LayoutUpdated += this.OnLayoutUpdated;
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