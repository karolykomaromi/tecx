namespace Hydra.Infrastructure.I18n
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics.Contracts;
    using System.Globalization;

    /// <summary>
    /// For implementation there could be versions backed by <seealso cref="http://msdn.microsoft.com/en-us/library/system.data.entity.design.pluralizationservices.pluralizationservice%28v=vs.110%29.aspx"/>
    /// and another one for Castle Project Active Record Inflector (which is also included in RavenDB btw.)
    /// </summary>
    [ContractClass(typeof(InflectorContract))]
    public interface IInflector
    {
        IReadOnlyCollection<CultureInfo> SupportedCultures { get; }

        string Pluralize(string word);

        string Singularize(string word);
    }

    [ContractClassFor(typeof(IInflector))]
    internal abstract class InflectorContract : IInflector
    {
        public IReadOnlyCollection<CultureInfo> SupportedCultures
        {
            get
            {
                Contract.Ensures(Contract.Result<IReadOnlyCollection<CultureInfo>>() != null);

                return new ReadOnlyCollection<CultureInfo>(new CultureInfo[0]);
            }
        }

        public string Pluralize(string word)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(word));
            Contract.Ensures(Contract.Result<string>() != null);

            return string.Empty;
        }

        public string Singularize(string word)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(word));
            Contract.Ensures(Contract.Result<string>() != null);

            return string.Empty;
        }
    }
}
