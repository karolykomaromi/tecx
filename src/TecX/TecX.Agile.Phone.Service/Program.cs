namespace TecX.Agile.Phone.Service
{
    using System;
    using System.ServiceModel;

    class Program
    {
        static void Main(string[] args)
        {
            ServiceHost host = null;

            try
            {
                host = new ServiceHost(typeof(ProjectService));
                
                Console.WriteLine("Starting host...");

                host.Open();

                Console.WriteLine("Host running...");
                Console.WriteLine("Press ENTER to stop service.");

                Console.ReadLine();
            }
            finally
            {
                if (host != null)
                {
                    host.Close();
                }
            }
        }
    }
}
