using System.Windows;
using System.Windows.Media;

namespace Infrastructure.Theming
{
    public static class VisualTree
    {
        public static T FindAncestor<T>(DependencyObject obj)
            where T : DependencyObject
        {
            DependencyObject parent = VisualTreeHelper.GetParent(obj);

            if (parent == null)
            {
                return null;
            }

            T ancestor = parent as T;

            if (ancestor != null)
            {
                return ancestor;
            }

            return FindAncestor<T>(parent);
        }
    }
}