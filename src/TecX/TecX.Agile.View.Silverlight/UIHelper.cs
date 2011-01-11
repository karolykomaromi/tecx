using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

using TecX.Common;

namespace TecX.Agile.View
{
    public static class UIHelper
    {
        public static T FindAncestor<T>(DependencyObject dependencyObject)
            where T : class
        {
            Guard.AssertNotNull(dependencyObject, "dependencyObject");

            DependencyObject target = dependencyObject;

            do
            {
                target = VisualTreeHelper.GetParent(target);

            } while (target != null &&
                     target as T == null);

            return target as T;
        }

        public static bool TryFindAncestor<T>(DependencyObject dependencyObject, out T ancestor)
            where T : class
        {
            ancestor = null as T;

            if (dependencyObject == null)
                return false;

            try
            {
                T found = FindAncestor<T>(dependencyObject);

                if (found != null)
                {
                    ancestor = found;
                    return true;
                }

                return false;
            }
            catch
            {
                return false;
            }
        }

        public static IEnumerable<T> FindChildren<T>(DependencyObject dependencyObject)
            where T : DependencyObject
        {
            Guard.AssertNotNull(dependencyObject, "dependencyObject");

            if (dependencyObject != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(dependencyObject); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(dependencyObject, i);
                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }

                    foreach (T childOfChild in FindChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }
    }
}
