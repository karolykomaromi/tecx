namespace Hydra.Infrastructure.I18n
{
    using System.Data.Entity.Design.PluralizationServices;
    using System.Diagnostics.Contracts;
    using System.Globalization;

    public class PluralizationServiceInflector : IInflector
    {
        private readonly PluralizationService pluralizationService;

        public PluralizationServiceInflector()
        {
            this.pluralizationService = PluralizationService.CreateService(CultureInfo.CreateSpecificCulture("en-US"));
        }

        public string Pluralize(string word)
        {
            Contract.Requires(this.pluralizationService != null);

            return this.pluralizationService.Pluralize(word);
        }

        public string Singularize(string word)
        {
            Contract.Requires(this.pluralizationService != null);

            return this.pluralizationService.Singularize(word);
        }
    }
}