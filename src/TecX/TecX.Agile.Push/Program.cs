using System;

namespace TecX.Agile.Push
{
    public class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                if (args[0] == @"/server")
                {
                    SocketServer server = null;

                    try
                    {
                        server = new SocketServer();

                        server.Start();

                        Console.WriteLine("Press any key to exit");
                        Console.WriteLine("------------------------------");
                        Console.Read();
                    }
                    finally
                    {
                        if (server != null)
                            server.Stop();
                    }

                }
            }
            else
            {
                PolicySocketServer policyServer = new PolicySocketServer();

                policyServer.Start();

                Console.WriteLine("Press any key to exit");
                Console.WriteLine("------------------------------");
                Console.Read();
            }
        }
    }
}
