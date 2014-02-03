namespace Infrastructure.Theming
{
    using System;
    using System.Collections;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;
    using Infrastructure.Events;

    public class ThemeBackgroundAdapter : ThemingAdapter<Control>
    {
        public ThemeBackgroundAdapter(Control control, Brush objectFromTheme)
        {
            Contract.Requires(control != null);
            Contract.Requires(objectFromTheme != null);

            this.Target = control;
            this.ObjectFromTheme = objectFromTheme;

            this.Target.LayoutUpdated += this.OnLayoutUpdated;
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

    public class ThemeBorderBackgroundAdapter : ThemingAdapter<Border>
    {
        public ThemeBorderBackgroundAdapter(Border border, Brush objectFromTheme)
        {
            Contract.Requires(border != null);
            Contract.Requires(objectFromTheme != null);

            this.Target = border;
            this.ObjectFromTheme = objectFromTheme;

            this.Target.LayoutUpdated += this.OnLayoutUpdated;
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

    public class ThemeForegroundAdapter : ThemingAdapter<Control>
    {
        public ThemeForegroundAdapter(Control control, Brush objectFromTheme)
        {
            Contract.Requires(control != null);
            Contract.Requires(objectFromTheme != null);

            this.Target = control;
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

    public class ThemeStyleAdapter : ThemingAdapter<FrameworkElement>
    {
        public ThemeStyleAdapter(FrameworkElement target, Style style)
        {
            Contract.Requires(target != null);
            Contract.Requires(style != null);

            this.Target = target;
            this.ObjectFromTheme = style;

            this.Target.LayoutUpdated += this.OnLayoutUpdated;
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

    public abstract class ThemingAdapter<T> : ISubscribeTo<ThemeChanged>
        where T : FrameworkElement
    {
        protected T Target { get; set; }
        protected object ObjectFromTheme { get; set; }
        protected object Key { get; set; }
        
        public abstract void Handle(ThemeChanged message);

        protected static object GetKey(ResourceDictionary dictionary, object o)
        {
            foreach (DictionaryEntry resource in dictionary)
            {
                if (resource.Value == o)
                {
                    return resource.Key;
                }
            }

            foreach (ResourceDictionary mergedDictionary in dictionary.MergedDictionaries)
            {
                object key = GetKey(mergedDictionary, o);

                if (key != null)
                {
                    return key;
                }
            }

            return null;
        }

        protected virtual void OnLayoutUpdated(object sender, EventArgs e)
        {
            this.Target.LayoutUpdated -= this.OnLayoutUpdated;

            UserControl uc = VisualTree.FindAncestor<UserControl>(this.Target);

            if (uc == null)
            {
                return;
            }

            this.Key = GetKey(Application.Current.Resources, this.ObjectFromTheme);

            if (this.Key == null)
            {
                ResourceDictionary dictionary = uc
                    .Resources
                    .MergedDictionaries
                    .FirstOrDefault(dict => dict.Source != null && dict.Source.ToString().EndsWith("Theme.xaml", StringComparison.OrdinalIgnoreCase));

                if (dictionary == null)
                {
                    return;
                }

                this.Key = GetKey(dictionary, this.ObjectFromTheme);
            }

            this.ObjectFromTheme = null;
        }
    }
}