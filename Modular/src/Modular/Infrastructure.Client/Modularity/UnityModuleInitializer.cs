namespace Infrastructure.Modularity
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using Microsoft.Practices.Prism.Logging;
    using Microsoft.Practices.Prism.Modularity;
    using Microsoft.Practices.Unity;

    public class UnityModuleInitializer : Microsoft.Practices.Prism.Modularity.IModuleInitializer
    {
        private readonly IUnityContainer container;
        private readonly ILoggerFacade logger;

        public UnityModuleInitializer(IUnityContainer container, ILoggerFacade logger)
        {
            Contract.Requires(container != null);
            Contract.Requires(logger != null);

            this.container = container;
            this.logger = logger;
        }

        public void Initialize(ModuleInfo moduleInfo)
        {
            Contract.Requires(moduleInfo != null);

            IModule moduleInstance = null;
            try
            {
                moduleInstance = this.CreateModule(moduleInfo);

                UnityModule module = moduleInstance as UnityModule;

                if (module != null)
                {
                    // wondering wether I should call all the methods on the unitymodule here or not...
                }

                moduleInstance.Initialize();
            }
            catch (Exception ex)
            {
                this.HandleModuleInitializationError(
                    moduleInfo,
                    moduleInstance != null ? moduleInstance.GetType().Assembly.FullName : null,
                    ex);
            }
        }

        public virtual void HandleModuleInitializationError(ModuleInfo moduleInfo, string assemblyName, Exception exception)
        {
            Contract.Requires(moduleInfo != null);
            Contract.Requires(exception != null);

            Exception moduleException;

            if (exception is ModuleInitializeException)
            {
                moduleException = exception;
            }
            else
            {
                if (!string.IsNullOrEmpty(assemblyName))
                {
                    moduleException = new ModuleInitializeException(moduleInfo.ModuleName, assemblyName, exception.Message, exception);
                }
                else
                {
                    moduleException = new ModuleInitializeException(moduleInfo.ModuleName, exception.Message, exception);
                }
            }

            this.logger.Log(moduleException.ToString(), Category.Exception, Priority.High);

            throw moduleException;
        }

        protected virtual IModule CreateModule(ModuleInfo moduleInfo)
        {
            Contract.Requires(moduleInfo != null);
            Contract.Ensures(Contract.Result<IModule>() != null);

            return this.CreateModule(moduleInfo.ModuleType);
        }

        protected virtual IModule CreateModule(string typeName)
        {
            Contract.Requires(!string.IsNullOrEmpty(typeName));
            Contract.Ensures(Contract.Result<IModule>() != null);

            Type moduleType = Type.GetType(typeName, false);

            if (moduleType == null)
            {
                string msg = string.Format(
                    CultureInfo.CurrentCulture, 
                    "Unable to retrieve the module type {0} from the loaded assemblies.  You may need to specify a more fully-qualified type name.", 
                    typeName);

                throw new ModuleInitializeException(msg);
            }

            return (IModule)this.container.Resolve(moduleType);
        }
    }
}
