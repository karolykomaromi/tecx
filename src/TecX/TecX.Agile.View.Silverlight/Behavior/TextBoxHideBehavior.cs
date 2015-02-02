using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace TecX.Agile.View.Behavior
{
    public class TextBoxHideBehavior : System.Windows.Interactivity.Behavior<TextBox>
    {
        private Brush _background;

        protected override void OnAttached()
        {
            base.OnAttached();

            if (DesignerProperties.GetIsInDesignMode(AssociatedObject))
                return;

            AssociatedObject.GotFocus += OnGotFocus;
            AssociatedObject.LostFocus += OnLostFocus;

            AssociatedObject.Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            UserControl parent = UIHelper.FindAncestor<UserControl>(AssociatedObject);

            _background = parent.Background;

            AssociatedObject.Loaded -= OnLoaded;
        }

        private void OnLostFocus(object sender, RoutedEventArgs e)
        {
            AssociatedObject.Background = _background;
            AssociatedObject.BorderThickness = new Thickness(0);
        }

        private void OnGotFocus(object sender, RoutedEventArgs e)
        {
            AssociatedObject.Background = new SolidColorBrush(Colors.White);
            AssociatedObject.BorderThickness = new Thickness(1);
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();

            if (DesignerProperties.GetIsInDesignMode(AssociatedObject))
                return;

            AssociatedObject.GotFocus -= OnGotFocus;
            AssociatedObject.LostFocus -= OnLostFocus;
        }
    }
}
