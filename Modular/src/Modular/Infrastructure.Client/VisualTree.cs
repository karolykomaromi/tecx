namespace Infrastructure
{
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Media;

    public static class VisualTree
    {
        public static T Ancestor<T>(DependencyObject obj)
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

            return Ancestor<T>(parent);
        }

        public static IEnumerable<T> Ancestors<T>(DependencyObject obj)
            where T : DependencyObject
        {
            DependencyObject parent = VisualTreeHelper.GetParent(obj);

            if (parent == null)
            {
                yield break;
            }

            T ancestor = parent as T;

            if (ancestor != null)
            {
                yield return ancestor;
            }

            foreach (T grandAncestor in Ancestors<T>(parent))
            {
                yield return grandAncestor;
            }
        }
    }
}