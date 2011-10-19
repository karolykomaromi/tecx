namespace TecX.Agile.Infrastructure
{
    using System;
    using System.Windows;

    public abstract class ModuleBase : IModule
    {
        public abstract string Description { get; }

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

        public void Initialize()
        {
            OnLoadLocalStyles();

            OnInitialize();
        }
    }
}