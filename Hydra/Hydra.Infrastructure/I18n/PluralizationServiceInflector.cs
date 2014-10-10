namespace Hydra.Infrastructure.I18n
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Data.Entity.Design.PluralizationServices;
    using System.Diagnostics.Contracts;
    using System.Globalization;

    public class PluralizationServiceInflector : IInflector
    {
        private readonly PluralizationService pluralizationService;

        private readonly CultureInfo english;

        public PluralizationServiceInflector()
        {
            this.english = CultureInfo.CreateSpecificCulture("en-US");
            this.pluralizationService = PluralizationService.CreateService(this.english);
        }

        public IReadOnlyCollection<CultureInfo> SupportedCultures
        {
            get
            {
                return new ReadOnlyCollection<CultureInfo>(new[] { this.english });
            }
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