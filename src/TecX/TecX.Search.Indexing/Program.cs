namespace TecX.Search.Indexing
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Topshelf;

    public class Program
    {
        public static void Main(string[] args)
        {
            HostFactory.Run(x =>
                {
                    x.RunAsLocalSystem();
                });
        }
    }
}
