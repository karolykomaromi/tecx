﻿namespace Infrastructure.Theming
{
    using System.Diagnostics.Contracts;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    public class ThemeBorderBackgroundAdapter : ThemingAdapter<Border>
    {
        public ThemeBorderBackgroundAdapter(Border border, Brush objectFromTheme)
            : base(border, objectFromTheme)
        {
        }

        public override void Handle(ThemeChanged message)
        {
            Contract.Requires(message != null);

            Brush newBrush = Application.Current.Resources[this.Key] as Brush;

            if (newBrush != null)
            {
                this.Target.Background = newBrush;
            }
        }
    }
}