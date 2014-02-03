namespace Infrastructure.Theming
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics.Contracts;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;
    using Infrastructure.Events;

    public class Theme
    {
        #region Attached Properties

        public static readonly DependencyProperty StyleProperty = DependencyProperty.RegisterAttached(
            "Style",
            typeof(Style),
            typeof(Theme),
            new PropertyMetadata(null, OnStyleChanged));

        public static readonly DependencyProperty BackgroundProperty = DependencyProperty.RegisterAttached(
            "Background",
            typeof(Brush),
            typeof(Theme),
            new PropertyMetadata(null, OnBackgroundChanged));

        public static readonly DependencyProperty ForegroundProperty = DependencyProperty.RegisterAttached(
            "Foreground",
            typeof(Brush),
            typeof(Theme),
            new PropertyMetadata(null, OnForegroundChanged));

        #endregion Attached Properties

        #region Attached Property Setters/Getters

        public static Brush GetBackground(FrameworkElement element)
        {
            Contract.Requires(element != null);

            return (Brush)element.GetValue(BackgroundProperty);
        }

        public static void SetBackground(FrameworkElement element, Brush brush)
        {
            Contract.Requires(element != null);
            Contract.Requires(brush != null);

            element.SetValue(BackgroundProperty, brush);
        }

        public static Brush GetForeground(FrameworkElement element)
        {
            Contract.Requires(element != null);

            return (Brush)element.GetValue(ForegroundProperty);
        }

        public static void SetForeground(FrameworkElement element, Brush brush)
        {
            Contract.Requires(element != null);
            Contract.Requires(brush != null);

            element.SetValue(ForegroundProperty, brush);
        }

        public static Style GetStyle(FrameworkElement element)
        {
            return (Style)element.GetValue(StyleProperty);
        }

        public static void SetStyle(FrameworkElement element, Style style)
        {
            element.SetValue(StyleProperty, style);
        }

        #endregion Attached Property Setters/Getters

        #region Attached Property Changed Handlers

        private static void OnForegroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (DesignerProperties.IsInDesignTool)
            {
                return;
            }

            Brush brush = e.NewValue as Brush;

            if (brush == null)
            {
                return;
            }

            Control control = d as Control;
            TextBlock textBlock;

            if (control != null)
            {
                var adapter = new ThemeForegroundAdapter(control, brush);

                EventAggregator.Current.Subscribe(adapter);

                control.Foreground = brush;
            }
            else if ((textBlock = d as TextBlock) != null)
            {
                var adapter = new ThemeTextBlockForegroundAdapter(textBlock, brush);

                EventAggregator.Current.Subscribe(adapter);

                textBlock.Foreground = brush;
            }
            else
            {
                throw new InvalidOperationException(
                "You must attach the 'Infrastructure.Theming.Theme.ForegroundProperty' to an element that is either a 'TextBlock' or a 'Control'.");
            }
        }

        private static void OnBackgroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (DesignerProperties.IsInDesignTool)
            {
                return;
            }

            Brush brush = e.NewValue as Brush;

            if (brush == null)
            {
                return;
            }

            Control control = d as Control;
            Border border;

            if (control != null)
            {
                var adapter = new ThemeBackgroundAdapter(control, brush);

                EventAggregator.Current.Subscribe(adapter);

                control.Background = brush;
            }
            else if ((border = d as Border) != null)
            {
                var adapter = new ThemeBorderBackgroundAdapter(border, brush);

                EventAggregator.Current.Subscribe(adapter);

                border.Background = brush;
            }
            else
            {
                throw new InvalidOperationException(
                "You must attach the 'Infrastructure.Theming.Theme.BackgroundProperty' to an element that is either a 'Border' or a 'Control'.");
            }
        }

        private static void OnStyleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (DesignerProperties.IsInDesignTool)
            {
                return;
            }

            FrameworkElement element = d as FrameworkElement;
            Style style = e.NewValue as Style;

            if (element == null || e.NewValue == null)
            {
                return;
            }

            var adapter = new ThemeStyleAdapter(element, style);

            EventAggregator.Current.Subscribe(adapter);

            element.Style = style;
        }

        #endregion Attached Property Changed Handlers
    }
}
