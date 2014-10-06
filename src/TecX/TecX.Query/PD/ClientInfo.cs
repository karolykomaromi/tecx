namespace TecX.Query.PD
{
    using System.Threading;

    /// <summary>
    /// Dummy implementation for demonstration purposes
    /// </summary>
    public class ClientInfo : IClientInfo
    {
        private static readonly ThreadLocal<IClientInfo> thread_ci = new ThreadLocal<IClientInfo>(() => new ClientInfo());

        public ClientInfo()
        {
            this.Principal = new PDPrincipal();
        }

        public PDPrincipal Principal { get; set; }

        public static IClientInfo GetClientInfo()
        {
            return thread_ci.Value;
        }
    }
}
