namespace Hydra.TestTools
{
    using System.Diagnostics.Contracts;
    using Hydra.Infrastructure.Mail;

    public static class MailboxAddressBuilderExtensions
    {
        public static MailboxAddressBuilder JohnWayne(this MailboxAddressBuilder builder)
        {
            Contract.Requires(builder != null);
            Contract.Ensures(Contract.Result<MailboxAddressBuilder>() != null);

            return builder.WithFirstName("John").WithLastName("Wayne");
        }

        public static MailboxAddressBuilder ClintEastwood(this MailboxAddressBuilder builder)
        {
            Contract.Requires(builder != null);
            Contract.Ensures(Contract.Result<MailboxAddressBuilder>() != null);

            return builder.WithFirstName("Clint").WithLastName("Eastwood");
        }

        public static MailboxAddressBuilder LorneGreene(this MailboxAddressBuilder builder)
        {
            Contract.Requires(builder != null);
            Contract.Ensures(Contract.Result<MailboxAddressBuilder>() != null);

            return builder.WithFirstName("Lorne").WithLastName("Greene");
        }

        public static MailboxAddressBuilder PernellRoberts(this MailboxAddressBuilder builder)
        {
            Contract.Requires(builder != null);
            Contract.Ensures(Contract.Result<MailboxAddressBuilder>() != null);

            return builder.WithFirstName("Pernell").WithLastName("Roberts");
        }

        public static MailboxAddressBuilder DanBlocker(this MailboxAddressBuilder builder)
        {
            Contract.Requires(builder != null);
            Contract.Ensures(Contract.Result<MailboxAddressBuilder>() != null);

            return builder.WithFirstName("Dan").WithLastName("Blocker");
        }

        public static MailboxAddressBuilder MichaelLandon(this MailboxAddressBuilder builder)
        {
            Contract.Requires(builder != null);
            Contract.Ensures(Contract.Result<MailboxAddressBuilder>() != null);

            return builder.WithFirstName("Michael").WithLastName("Landon");
        }

        public static MailboxAddressBuilder VictorSenYung(this MailboxAddressBuilder builder)
        {
            Contract.Requires(builder != null);
            Contract.Ensures(Contract.Result<MailboxAddressBuilder>() != null);

            return builder.WithFirstName("Victor").WithLastName("Sen Yung");
        }

        public static MailboxAddressBuilder HenryFonda(this MailboxAddressBuilder builder)
        {
            Contract.Requires(builder != null);
            Contract.Ensures(Contract.Result<MailboxAddressBuilder>() != null);

            return builder.WithFirstName("Henry").WithLastName("Fonda");
        }

        public static MailboxAddressBuilder CharlesBronson(this MailboxAddressBuilder builder)
        {
            Contract.Requires(builder != null);
            Contract.Ensures(Contract.Result<MailboxAddressBuilder>() != null);

            return builder.WithFirstName("Charles").WithLastName("Bronson");
        }

        public static MailboxAddressBuilder JasonRobards(this MailboxAddressBuilder builder)
        {
            Contract.Requires(builder != null);
            Contract.Ensures(Contract.Result<MailboxAddressBuilder>() != null);

            return builder.WithFirstName("Jason").WithLastName("Robards");
        }

        public static MailboxAddressBuilder DoNotReply(this MailboxAddressBuilder builder)
        {
            Contract.Requires(builder != null);
            Contract.Ensures(Contract.Result<MailboxAddressBuilder>() != null);

            return builder.WithAddress("donotreply@mail.invalid").WithDisplayName("Do Not Reply");
        }
    }
}