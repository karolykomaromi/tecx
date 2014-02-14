namespace Infrastructure.I18n
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Reflection;
    using System.Threading;
    using Microsoft.Practices.Prism.Logging;

    public class ResxFilesResourceManager : IResourceManager
    {
        private readonly Type resxType;
        private readonly ILoggerFacade logger;

        public ResxFilesResourceManager(Type resxType, ILoggerFacade logger)
        {
            Contract.Requires(resxType != null);
            Contract.Requires(logger != null);

            this.resxType = resxType;
            this.logger = logger;
        }

        public string this[ResxKey key]
        {
            get
            {
                string rsk = key.ToString();

                int lastIndexOfDot = Math.Max(0, rsk.LastIndexOf('.'));

                if (!this.resxType.FullName.StartsWith(rsk.Substring(0, lastIndexOfDot), StringComparison.OrdinalIgnoreCase))
                {
                    string msg = string.Format("Could not find resource item. Key=\"{0}\" Culture=\"{1}\" Resx=\"{2}\"", key.ToString(), Thread.CurrentThread.CurrentUICulture.Name, this.resxType.FullName);
                    this.logger.Log(msg, Category.Debug, Priority.Low);
                    return rsk;
                }

                PropertyInfo property = this.resxType.GetProperty(rsk.Substring(lastIndexOfDot + 1), BindingFlags.Static | BindingFlags.Public | BindingFlags.IgnoreCase);

                if (property == null)
                {
                    string msg = string.Format("Could not find resource item. Key=\"{0}\" Culture=\"{1}\" Resx=\"{2}\"", key.ToString(), Thread.CurrentThread.CurrentUICulture.Name, this.resxType.FullName);
                    this.logger.Log(msg, Category.Debug, Priority.Low);
                    return rsk;
                }

                MethodInfo getter = property.GetGetMethod();

                if (getter == null)
                {
                    string msg = string.Format("Could not find resource item. Key=\"{0}\" Culture=\"{1}\" Resx=\"{2}\"", key.ToString(), Thread.CurrentThread.CurrentUICulture.Name, this.resxType.FullName);
                    this.logger.Log(msg, Category.Debug, Priority.Low);
                    return rsk;
                }

                string value = getter.Invoke(null, null) as string;

                if (value != null)
                {
                    string msg = string.Format("Found resource item. Value=\"{0}\" Key=\"{1}\" Culture=\"{2}\" Resx=\"{3}\"", value, key.ToString(), Thread.CurrentThread.CurrentUICulture.Name, this.resxType.FullName);
                    this.logger.Log(msg, Category.Debug, Priority.Low);
                    return value;
                }

                return rsk;
            }
        }
    }
}