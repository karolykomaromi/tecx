namespace TecX.Agile.Push
{
    public interface ISocketServer
    {
        /// <summary>
        /// Starts the socket server.
        /// </summary>
        void Start();

        /// <summary>
        /// Stops the socket server.
        /// </summary>
        void Stop();

        /// <summary>
        /// Sends the serialized data to all clients.
        /// </summary>
        /// <param name="message">The message.</param>
        void Push(byte[] message);
    }
}