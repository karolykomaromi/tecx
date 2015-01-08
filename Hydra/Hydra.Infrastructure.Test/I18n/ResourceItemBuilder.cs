namespace Hydra.Infrastructure.Test.I18n
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.Linq.Expressions;
    using System.Reflection;
    using Hydra.Infrastructure.I18n;
    using Hydra.Infrastructure.Reflection;

    public class ResourceItemBuilder : Builder<ResourceItem>
    {
        private Type type;

        private string propertyName;

        private CultureInfo culture;

        private string value;

        public ResourceItemBuilder()
        {
            this.type = typeof(Missing);
            this.propertyName = string.Empty;
            this.value = string.Empty;
            this.culture = CultureInfo.InvariantCulture;
        }

        public override ResourceItem Build()
        {
            Contract.Requires(this.type != typeof(Missing));
            Contract.Requires(!string.IsNullOrWhiteSpace(this.propertyName));
            Contract.Requires(!object.Equals(this.culture, CultureInfo.InvariantCulture));
            Contract.Requires(!string.IsNullOrWhiteSpace(this.value));

            ResourceItem ri = new ResourceItem
                {
                    Name = (this.type.FullName + "." + this.propertyName).ToUpperInvariant(),
                    TwoLetterISOLanguageName = this.culture.TwoLetterISOLanguageName.ToUpperInvariant(),
                    Value = this.value
                };

            return ri;
        }

        public ResourceItemBuilder FromType(Type type)
        {
            Contract.Requires(type != null);

            this.type = type;

            return this;
        }

        public ResourceItemBuilder ForProperty(Expression<Func<string>> propertySelector)
        {
            Contract.Requires(propertySelector != null);

            this.propertyName = TypeHelper.GetPropertyName(propertySelector);

            return this;
        }

        public ResourceItemBuilder UseCulture(CultureInfo culture)
        {
            Contract.Requires(culture != null);

            this.culture = culture;

            return this;
        }

        public ResourceItemBuilder WithValue(string value)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(value));

            this.value = value;

            return this;
        }
    }
}