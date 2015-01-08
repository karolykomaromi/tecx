namespace Hydra.Infrastructure.I18n
{
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.Linq;
    using NHibernate;
    using NHibernate.Linq;

    public class NhResourceManager : ResourceManagerBase
    {
        private readonly ISession session;

        public NhResourceManager(string baseName, ISession session)
            : base(baseName)
        {
            Contract.Requires(session != null);

            this.session = session;
        }

        public override string GetString(string name, CultureInfo culture)
        {
            ResourceItem resourceItem = this.session.Query<ResourceItem>()
                .FirstOrDefault(
                    ri => ri.Name == (this.BaseName + "." + name).ToUpperInvariant() && 
                    ri.TwoLetterISOLanguageName == culture.TwoLetterISOLanguageName.ToUpperInvariant());

            if (resourceItem != null)
            {
                return resourceItem.Value;
            }

            return this.GetKey(name, culture);
        }

        public IEnumerable<ResourceItem> GetAll(CultureInfo culture)
        {
            Contract.Requires(culture != null);

            IEnumerable<ResourceItem> resourceItems = this.session.Query<ResourceItem>()
                .Where(
                    ri => ri.Name.StartsWith(this.BaseName.ToUpperInvariant()) && 
                    ri.TwoLetterISOLanguageName == culture.TwoLetterISOLanguageName.ToUpperInvariant());

            return resourceItems;
        }
    }
}
