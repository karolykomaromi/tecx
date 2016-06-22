namespace Cars.I18n
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Globalization;

    public sealed class TemporarilyChangeCulture : IDisposable
    {
        private readonly CultureInfo temporaryCulture;
        private readonly CultureInfo temporaryUICulture;

        private readonly CultureInfo currentCulture;
        private readonly CultureInfo currentUICulture;

        public TemporarilyChangeCulture(CultureInfo temporaryCulture)
            : this(temporaryCulture, temporaryCulture)
        {
        }

        private TemporarilyChangeCulture(CultureInfo temporaryCulture, CultureInfo temporaryUICulture)
        {
            Contract.Requires(temporaryCulture != null);
            Contract.Requires(temporaryUICulture != null);

            this.temporaryCulture = temporaryCulture;
            this.temporaryUICulture = temporaryUICulture;

            this.currentCulture = CultureInfo.CurrentCulture;
            this.currentUICulture = CultureInfo.CurrentUICulture;

            CultureInfo.CurrentCulture = this.temporaryCulture;
            CultureInfo.CurrentUICulture = this.temporaryUICulture;
        }

        public void Dispose()
        {
            CultureInfo.CurrentCulture = this.currentCulture;
            CultureInfo.CurrentUICulture = this.currentUICulture;
        }
    }
}