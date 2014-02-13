namespace Infrastructure.Theming
{
    using System;
    using System.Collections;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using Infrastructure.Events;

    public abstract class ThemingAdapter<T> : ISubscribeTo<ThemeChanged>
        where T : FrameworkElement
    {
        protected ThemingAdapter(T target, object objectFromTheme)
        {
            Contract.Requires(target != null);
            Contract.Requires(objectFromTheme != null);

            this.Target = target;
            this.ObjectFromTheme = objectFromTheme;

            this.Target.LayoutUpdated += this.OnLayoutUpdated;

            // to avoid gc'ing we need to add a backward reference from target to adapter
            this.Target.Resources.Add(Guid.NewGuid().ToString(), this);
        }

        protected T Target { get; set; }

        protected object ObjectFromTheme { get; set; }

        protected object Key { get; set; }
        
        public abstract void Handle(ThemeChanged message);

        protected virtual void OnLayoutUpdated(object sender, EventArgs e)
        {
            this.Target.LayoutUpdated -= this.OnLayoutUpdated;
            this.Target.Unloaded += this.OnUnloaded;

            // don't try to find the key twice because we reset ObjectFromTheme
            // when we find the key for the first time.
            if (this.Key != null)
            {
                return;
            }

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

        private static object GetKey(ResourceDictionary dictionary, object o)
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

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            this.Target.Unloaded -= this.OnUnloaded;
            this.Target.LayoutUpdated += this.OnLayoutUpdated;
        }
    }
}