namespace TecX.Agile
{
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Media;

    using TecX.Common;

    public static class TreeHelper
    {
        public static bool TryFindAncestor<T>(DependencyObject dependencyObject, out T ancestor)
            where T : class
        {
            ancestor = null as T;

            if (dependencyObject == null)
            {
                return false;
            }

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

        public static bool TryGetName(FrameworkElement elementWithNameScope, DependencyObject objectInsideNameScope, out string name)
        {
            name = string.Empty;

            if (elementWithNameScope == null ||
                objectInsideNameScope == null)
            {
                return false;
            }

            try
            {
                name = GetName(elementWithNameScope, objectInsideNameScope);

                return !string.IsNullOrEmpty(name);
            }
            catch
            {
                return false;
            }
        }

        private static T FindAncestor<T>(DependencyObject dependencyObject)
               where T : class
        {
            Guard.AssertNotNull(dependencyObject, "dependencyObject");

            DependencyObject target = dependencyObject;

            do
            {
                target = LogicalTreeHelper.GetParent(target);
            }
            while (target != null && target as T == null);

            return target as T;
        }

        private static string GetName(FrameworkElement elementWithNameScope, DependencyObject objectInsideNameScope)
        {
            Guard.AssertNotNull(elementWithNameScope, "elementWithNameScope");
            Guard.AssertNotNull(objectInsideNameScope, "objectInsideNameScope");

            // to be perfectly honest I don't think this is a recommendable way to find out
            // what the name of a control is. but as INameScope does not give me that name but 
            // its implementation does as of now...
            var scope = NameScope.GetNameScope(elementWithNameScope) as IEnumerable<KeyValuePair<string, object>>;

            if (scope != null)
            {
                foreach (var kvp in scope)
                {
                    if (kvp.Value == objectInsideNameScope)
                    {
                        return kvp.Key;
                    }
                }
            }

            return string.Empty;
        }
    }
}
