namespace Cars.I18n
{
    using System.Diagnostics.Contracts;

    public static class PolyglotStringBuilderExtensions
    {
        public static PolyglotStringBuilder GermanGermany(this PolyglotStringBuilder builder, string translation)
        {
            Contract.Requires(builder != null);
            Contract.Requires(!string.IsNullOrWhiteSpace(translation));
            Contract.Ensures(Contract.Result<PolyglotStringBuilder>() != null);

            builder.WithTranslation(Cultures.GermanGermany, translation);

            return builder;
        }

        public static PolyglotStringBuilder EnglishUnitedKingdom(this PolyglotStringBuilder builder, string translation)
        {
            Contract.Requires(builder != null);
            Contract.Requires(!string.IsNullOrWhiteSpace(translation));
            Contract.Ensures(Contract.Result<PolyglotStringBuilder>() != null);

            builder.WithTranslation(Cultures.EnglishUnitedKingdom, translation);

            return builder;
        }

        public static PolyglotStringBuilder EnglishUnitedStates(this PolyglotStringBuilder builder, string translation)
        {
            Contract.Requires(builder != null);
            Contract.Requires(!string.IsNullOrWhiteSpace(translation));
            Contract.Ensures(Contract.Result<PolyglotStringBuilder>() != null);

            builder.WithTranslation(Cultures.EnglishUnitedStates, translation);

            return builder;
        }

        public static PolyglotStringBuilder FrenchFrance(this PolyglotStringBuilder builder, string translation)
        {
            Contract.Requires(builder != null);
            Contract.Requires(!string.IsNullOrWhiteSpace(translation));
            Contract.Ensures(Contract.Result<PolyglotStringBuilder>() != null);

            builder.WithTranslation(Cultures.FrenchFrance, translation);

            return builder;
        }
    }
}