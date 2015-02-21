namespace Hydra.Infrastructure.Mail
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using MimeKit;

    public class MailboxAddressBuilder : Builder<MailboxAddress>
    {
        private string address;

        private string firstName;

        private string lastName;

        private string displayName;

        private string domain;

        public MailboxAddressBuilder()
        {
            this.address = string.Empty;
            this.firstName = string.Empty;
            this.lastName = string.Empty;
            this.displayName = string.Empty;
            this.domain = "mail.invalid";
        }

        public override MailboxAddress Build()
        {
            string dn;
            if (string.IsNullOrWhiteSpace(this.displayName))
            {
                dn = this.firstName + " " + this.lastName;
            }
            else
            {
                dn = this.displayName;
            }

            string adr;
            if (string.IsNullOrWhiteSpace(this.address))
            {
                string f = this.firstName.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).First().ToLowerInvariant();

                string n = this.lastName.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Last().ToLowerInvariant();

                adr = f + "." + n + "@" + this.domain;
            }
            else
            {
                adr = this.address;
            }

            return new MailboxAddress(dn, adr);
        }

        public MailboxAddressBuilder WithAddress(string address)
        {
            Contract.Requires(!string.IsNullOrEmpty(address));
            Contract.Ensures(Contract.Result<MailboxAddressBuilder>() != null);

            this.address = address;

            return this;
        }

        public MailboxAddressBuilder WithFirstName(string firstName)
        {
            Contract.Requires(!string.IsNullOrEmpty(firstName));
            Contract.Ensures(Contract.Result<MailboxAddressBuilder>() != null);

            this.firstName = StringHelper.CapitalizeFirstLetter(firstName);

            return this;
        }

        public MailboxAddressBuilder WithLastName(string lastName)
        {
            Contract.Requires(!string.IsNullOrEmpty(lastName));
            Contract.Ensures(Contract.Result<MailboxAddressBuilder>() != null);

            this.lastName = StringHelper.CapitalizeFirstLetter(lastName);

            return this;
        }

        public MailboxAddressBuilder WithDisplayName(string displayName)
        {
            Contract.Requires(!string.IsNullOrEmpty(displayName));
            Contract.Ensures(Contract.Result<MailboxAddressBuilder>() != null);

            this.displayName = displayName;

            return this;
        }

        public MailboxAddressBuilder WithDomain(string domain)
        {
            Contract.Requires(!string.IsNullOrEmpty(domain));
            Contract.Ensures(Contract.Result<MailboxAddressBuilder>() != null);

            this.domain = domain;

            return this;
        }
    }
}