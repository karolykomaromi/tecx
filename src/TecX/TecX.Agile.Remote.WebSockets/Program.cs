using System;

using Microsoft.ServiceModel.WebSockets;

namespace TecX.Agile.Remote.WebSockets
{
    class Program
    {
        static void Main(string[] args)
        {
            var sh = new WebSocketsHost<MessageRelayService>(new Uri("ws://" + Environment.MachineName + ":4502/relay"));
            sh.AddWebSocketsEndpoint();
            sh.Open();

            Console.WriteLine("Press ENTER to stop service");
            Console.ReadLine();
        }
    }
}
