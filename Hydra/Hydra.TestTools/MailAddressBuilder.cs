namespace Hydra.TestTools
{
    using System.Diagnostics.Contracts;
    using System.Net.Mail;
    using Hydra.Infrastructure;

    public class MailAddressBuilder : Builder<MailAddress>
    {
        private string first;

        private string last;

        private string domain;

        public MailAddressBuilder()
        {
            this.first = string.Empty;

            this.last = string.Empty;

            this.domain = "mail.invalid";
        }

        public override MailAddress Build()
        {
            return new MailAddress(this.first.ToLowerInvariant() + "." + this.last.ToLowerInvariant() + "@" + this.domain, this.first + " " + this.last);
        }

        public MailAddressBuilder WithFirstName(string firstName)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(firstName));

            this.first = this.CapitalizeFirstLetter(firstName);

            return this;
        }

        public MailAddressBuilder WithLastName(string lastName)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(lastName));

            this.last = this.CapitalizeFirstLetter(lastName);

            return this;
        }

        public MailAddressBuilder WithDomain(string domain)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(domain));

            this.domain = domain.ToLowerInvariant().Trim();

            return this;
        }

        private string CapitalizeFirstLetter(string s)
        {
            return (s.Substring(0, 1).ToUpperInvariant() + s.Substring(1)).Trim();
        }
    }
}
