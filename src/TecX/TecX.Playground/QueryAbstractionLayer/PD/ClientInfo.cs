namespace TecX.Playground.QueryAbstractionLayer.PD
{
    using System.Threading;

    /// <summary>
    /// Dummy implementation for demonstration purposes
    /// </summary>
    public class ClientInfo : IClientInfo
    {
        private static readonly ThreadLocal<IClientInfo> thread_clientInfo = new ThreadLocal<IClientInfo>(() => new ClientInfo());

        private static int counter = 0;

        public ClientInfo()
        {
            counter++;

            this.Principal = new PDPrincipal { PDO_ID = GetPrincipalID() };
        }

        public PDPrincipal Principal { get; set; }

        public static IClientInfo GetClientInfo()
        {
            return thread_clientInfo.Value;
        }

        private long GetPrincipalID()
        {
            if (counter % 2 == 0)
            {
                return 42;
            }

            return 1337;
        }
    }
}
