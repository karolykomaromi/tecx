namespace Hydra.Infrastructure.I18n
{
    using System.Diagnostics.Contracts;

    /// <summary>
    /// For implementation there could be versions backed by <seealso cref="http://msdn.microsoft.com/en-us/library/system.data.entity.design.pluralizationservices.pluralizationservice%28v=vs.110%29.aspx"/>
    /// and another one for Castle Project Active Record Inflector (which is also included in RavenDB btw.)
    /// </summary>
    [ContractClass(typeof(InflectorContract))]
    public interface IInflector
    {
        string Pluralize(string word);

        string Singularize(string word);
    }

    [ContractClassFor(typeof(IInflector))]
    internal abstract class InflectorContract : IInflector
    {
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
