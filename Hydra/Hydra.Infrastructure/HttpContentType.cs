namespace Hydra.Infrastructure
{
    using System.Diagnostics.Contracts;
    using System.Runtime.CompilerServices;

    public class HttpContentType
    {
        private readonly string value;
        private readonly string name;

        public HttpContentType(string value, [CallerMemberName] string name = "")
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(value));
            Contract.Requires(!string.IsNullOrWhiteSpace(name));

            this.value = value;
            this.name = name;
        }

        public string Name
        {
            get
            {
                Contract.Ensures(!string.IsNullOrWhiteSpace(Contract.Result<string>()));

                return this.name;
            }
        }

        public string Value
        {
            get
            {
                Contract.Ensures(!string.IsNullOrWhiteSpace(Contract.Result<string>()));

                return this.value;
            }
        }

        public static implicit operator string(HttpContentType contentType)
        {
            Contract.Requires(contentType != null);
            Contract.Ensures(Contract.Result<string>() != null);

            return contentType.Value;
        }

        public override string ToString()
        {
            return this.Value;
        }
    }
}