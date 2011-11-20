namespace TecX.Agile.Infrastructure.Modularization
{
    using System;
    using System.Windows;

    public abstract class Module : IModule
    {
        public abstract string Description { get; }

        public void Initialize()
        {
            this.OnLoadLocalStyles();

            this.OnInitialize();
        }

        protected virtual void OnLoadLocalStyles()
        {
            string assemblyName = GetType().Assembly.GetName().Name;

            ResourceDictionary dictionary = new ResourceDictionary
            {
                Source = new Uri(assemblyName + ";component/ModuleStyles.xaml", UriKind.RelativeOrAbsolute)
            };

            Application.Current.Resources.MergedDictionaries.Add(dictionary);
        }

        protected virtual void OnInitialize()
        {
        }
    }
}