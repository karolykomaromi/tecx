namespace TecX.CaliburnEx.Modularization
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
            string assemblyName = this.GetType().Assembly.GetName().Name;

            try
            {
                ResourceDictionary dictionary = new ResourceDictionary
                    {
                        Source = new Uri(assemblyName + ";component/ModuleStyles.xaml", UriKind.RelativeOrAbsolute)
                    };

                Application.Current.Resources.MergedDictionaries.Add(dictionary);
            }
            catch
            {
                // TODO weberse 2011-12-22 see if there is a better way to ignore the non-existence of the dictionary
            }
        }

        protected virtual void Initialize()
        {
        }
    }
}