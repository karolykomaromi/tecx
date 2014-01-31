namespace Modular.Web
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Web;

    public class XapDownloadHandler : IHttpHandler
    {
        // The default percentage of the file size per chunk transmitted is 10%
        private const double DefaultTransmitChunkPercent = 0.1;

        /// <summary>
        /// Gets a value indicating whether another request can use the <see cref="T:System.Web.IHttpHandler"/> instance.
        /// </summary>
        /// <value></value>
        /// <returns>true if the <see cref="T:System.Web.IHttpHandler"/> instance is reusable; otherwise, false.</returns>
        public bool IsReusable
        {
            get { return true; }
        }

        /// <summary>
        /// Enables processing of HTTP Web requests by a custom HttpHandler that implements the <see cref="T:System.Web.IHttpHandler"/> interface.
        /// </summary>
        /// <param name="context">An <see cref="T:System.Web.HttpContext"/> object that provides references to the intrinsic server objects (for example, Request, Response, Session, and Server) used to service HTTP requests.</param>
        public void ProcessRequest(HttpContext context)
        {
            this.ProcessRequest(new HttpContextWrapper(context));
        }

        public void ProcessRequest(HttpContextBase context)
        {
            string physicalPath = context.Server.MapPath(context.Request.Path);

            this.TransmitFile(context, physicalPath);
        }

        // This method provides the actual slow-down and incremental transmittion.
        private void TransmitFile(HttpContextBase context, string path)
        {
            // The output must be unbufferred to allow for the client to receive chunks separately.
            context.Response.BufferOutput = false;
            context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            context.Response.Cache.SetNoStore();
            context.Response.Cache.SetExpires(DateTime.MinValue);

            if (!File.Exists(path))
            {
                context.Response.StatusCode = 404;
                context.Response.End();
                return;
            }

            // So that the client can display progress as chunks are downloaded, we output the Content-length header.
            long fileSize = -1L;

            FileInfo fileInfo = new FileInfo(path);
            fileSize = fileInfo.Length;

            context.Response.AppendHeader("Content-Length", fileSize.ToString(CultureInfo.InvariantCulture));

            // Read the file and calculate the number of even chunks + the leftover chunk.
            byte[] moduleBuffer = File.ReadAllBytes(path);
            byte[] chunkBufer;
            int chunkSize = (int)((double)moduleBuffer.Length * DefaultTransmitChunkPercent);
            int chunkCount = moduleBuffer.Length / chunkSize;
            int leftOverSize = moduleBuffer.Length % chunkSize;
            int i = 0;
            
            if (chunkCount > 0)
            {
                chunkBufer = new byte[chunkSize];
                while (i < chunkCount * chunkSize)
                {
                    Array.Copy(moduleBuffer, i, chunkBufer, 0, chunkSize);
                    context.Response.BinaryWrite(chunkBufer);
                    i += chunkSize;
                }
            }

            if (leftOverSize != 0)
            {
                chunkBufer = new byte[leftOverSize];
                Array.Copy(moduleBuffer, i, chunkBufer, 0, leftOverSize);
                context.Response.BinaryWrite(chunkBufer);
            }
        }

    }
}