namespace Hydra.Infrastructure
{
    using System.Diagnostics;
    using System.Diagnostics.Contracts;
    using System.Runtime.CompilerServices;

    [DebuggerDisplay("{StatusCode} {Status}")]
    public class HttpStatusCode
    {
        private readonly int statusCode;
        private readonly string status;
        private readonly string description;

        public HttpStatusCode(int statusCode, string description = "", [CallerMemberName]string status = "")
        {
            Contract.Requires(statusCode > 99);
            Contract.Requires(statusCode < 600);
            Contract.Requires(!string.IsNullOrWhiteSpace(status));

            this.statusCode = statusCode;
            this.description = description;
            this.status = StringHelper.SplitCamelCase(status);
        }

        public int StatusCode
        {
            get { return this.statusCode; }
        }

        public string Status
        {
            get { return this.status; }
        }

        public string Description
        {
            get { return this.description; }
        }

        public static implicit operator int(HttpStatusCode status)
        {
            Contract.Requires(status != null);

            return status.StatusCode;
        }
    }
}