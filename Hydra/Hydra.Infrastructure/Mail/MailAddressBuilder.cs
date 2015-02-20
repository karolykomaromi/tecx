namespace Hydra.Infrastructure.Mail
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Net.Mail;

    public class MailAddressBuilder : Builder<MailAddress>
    {
        private string first;

        private string last;

        private string displayName;

        private string address;

        private string domain;

        public MailAddressBuilder()
        {
            this.first = string.Empty;

            this.last = string.Empty;

            this.displayName = string.Empty;

            this.address = string.Empty;

            this.domain = "mail.invalid";
        }

        public override MailAddress Build()
        {
            string dn;
            if (string.IsNullOrWhiteSpace(this.displayName))
            {
                dn = this.first + " " + this.last;
            }
            else
            {
                dn = this.displayName;
            }

            string adr;
            if (string.IsNullOrWhiteSpace(this.address))
            {
                string f = this.first.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).First().ToLowerInvariant();

                string n = this.last.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Last().ToLowerInvariant();

                adr = f + "." + n + "@" + this.domain;
            }
            else
            {
                adr = this.address;
            }

            return new MailAddress(adr, dn);
        }

        public MailAddressBuilder WithFirstName(string firstName)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(firstName));
            Contract.Ensures(Contract.Result<MailAddressBuilder>() != null);

            this.first = this.CapitalizeFirstLetter(firstName);

            return this;
        }

        public MailAddressBuilder WithLastName(string lastName)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(lastName));
            Contract.Ensures(Contract.Result<MailAddressBuilder>() != null);

            this.last = this.CapitalizeFirstLetter(lastName);

            return this;
        }

        public MailAddressBuilder WithDomain(string domain)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(domain));
            Contract.Ensures(Contract.Result<MailAddressBuilder>() != null);

            this.domain = domain.ToLowerInvariant().Trim();

            return this;
        }

        public MailAddressBuilder WithAddress(string address)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(address));
            Contract.Ensures(Contract.Result<MailAddressBuilder>() != null);

            this.address = address.ToLowerInvariant().Trim();

            return this;
        }

        public MailAddressBuilder WithDisplayName(string displayName)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(displayName));
            Contract.Ensures(Contract.Result<MailAddressBuilder>() != null);

            this.displayName = displayName;

            return this;
        }

        private string CapitalizeFirstLetter(string s)
        {
            return (s.Substring(0, 1).ToUpperInvariant() + s.Substring(1)).Trim();
        }
    }
}
