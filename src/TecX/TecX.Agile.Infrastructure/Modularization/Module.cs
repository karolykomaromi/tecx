namespace TecX.Agile.Infrastructure.Modularization
{
    using System;
    using System.Windows;

    public abstract class Module : IModule
    {
        public abstract string Description { get; }

        void IModule.Initialize()
        {
            this.PreLoadStyles();

            this.LoadStyles();

            this.PostLoadStyles();

            this.PreInitialize();

            this.Initialize();

            this.PostInitialize();
        }

        protected virtual void PostInitialize()
        {
        }

        protected virtual void PreInitialize()
        {
        }

        protected virtual void PostLoadStyles()
        {
        }

        protected virtual void PreLoadStyles()
        {
        }

        protected virtual void LoadStyles()
        {
            string assemblyName = GetType().Assembly.GetName().Name;

            ResourceDictionary dictionary = new ResourceDictionary
            {
                Source = new Uri(assemblyName + ";component/ModuleStyles.xaml", UriKind.RelativeOrAbsolute)
            };

            Application.Current.Resources.MergedDictionaries.Add(dictionary);
        }

        protected virtual void Initialize()
        {
        }
    }
}