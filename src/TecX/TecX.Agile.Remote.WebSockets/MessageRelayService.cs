using System;
using System.ServiceModel;
using System.Threading;

using Microsoft.ServiceModel.WebSockets;

namespace TecX.Agile.Remote.WebSockets
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class MessageRelayService : WebSocketsService
    {
        private static readonly MessageRelaySessions Sessions = new MessageRelaySessions();
        private static int _globalId;

        private readonly int _id;

        public MessageRelayService()
        {
            _id = Interlocked.Increment(ref MessageRelayService._globalId);
            if (!MessageRelayService.Sessions.TryAdd(this))
            {
                throw new InvalidOperationException("Can't add session.");
            }
        }

        public override void OnMessage(string value)
        {
            MessageRelayService.Sessions.RelayMessage(this, value);
        }

        /// <summary>
        /// Returns the hash code for this instance
        /// </summary>
        /// <returns>A 32-bit signed integer hash code.</returns>
        public override int GetHashCode()
        {
            return _id;
        }

        protected override void OnClose(object sender, EventArgs e)
        {
            MessageRelayService.Sessions.Remove(this);
        }
    }
}
