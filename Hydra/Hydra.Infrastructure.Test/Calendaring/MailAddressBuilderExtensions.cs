namespace Hydra.Infrastructure.Test.Calendaring
{
    using System.Diagnostics.Contracts;

    public static class MailAddressBuilderExtensions
    {
        public static MailAddressBuilder JohnWayne(this MailAddressBuilder builder)
        {
            Contract.Requires(builder != null);

            return builder.WithFirstName("John").WithLastName("Wayne");
        }

        public static MailAddressBuilder ClintEastwood(this MailAddressBuilder builder)
        {
            Contract.Requires(builder != null);

            return builder.WithFirstName("Clint").WithLastName("Eastwood");
        }

        public static MailAddressBuilder LorneGreene(this MailAddressBuilder builder)
        {
            Contract.Requires(builder != null);

            return builder.WithFirstName("Lorne").WithLastName("Greene");
        }

        public static MailAddressBuilder PernellRoberts(this MailAddressBuilder builder)
        {
            Contract.Requires(builder != null);

            return builder.WithFirstName("Pernell").WithLastName("Roberts");
        }

        public static MailAddressBuilder DanBlocker(this MailAddressBuilder builder)
        {
            Contract.Requires(builder != null);

            return builder.WithFirstName("Dan").WithLastName("Blocker");
        }

        public static MailAddressBuilder MichaelLandon(this MailAddressBuilder builder)
        {
            Contract.Requires(builder != null);

            return builder.WithFirstName("Michael").WithLastName("Landon");
        }

        public static MailAddressBuilder VictorSenYung(this MailAddressBuilder builder)
        {
            Contract.Requires(builder != null);

            return builder.WithFirstName("Victor").WithLastName("Sen Yung");
        }

        public static MailAddressBuilder HenryFonda(this MailAddressBuilder builder)
        {
            Contract.Requires(builder != null);

            return builder.WithFirstName("Henry").WithLastName("Fonda");
        }

        public static MailAddressBuilder CharlesBronson(this MailAddressBuilder builder)
        {
            Contract.Requires(builder != null);

            return builder.WithFirstName("Charles").WithLastName("Bronson");
        }

        public static MailAddressBuilder JasonRobards(this MailAddressBuilder builder)
        {
            Contract.Requires(builder != null);

            return builder.WithFirstName("Jason").WithLastName("Robards");
        }
    }
}