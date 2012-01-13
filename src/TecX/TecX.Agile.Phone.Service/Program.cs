namespace TecX.Agile.Phone.Service
{
    using System;
    using System.ServiceModel;
    using System.ServiceModel.Description;

    class Program
    {
        static void Main(string[] args)
        {
            ServiceHost host = null;

            try
            {
                host = new ServiceHost(typeof(ProjectService), new Uri("http://localhost/phone/project"));

                host.AddServiceEndpoint(
                    typeof(IProjectService), new BasicHttpBinding(BasicHttpSecurityMode.None), string.Empty);

                ServiceMetadataBehavior behavior = new ServiceMetadataBehavior();

                host.Description.Behaviors.Add(behavior);

                host.AddServiceEndpoint(
                    typeof(IMetadataExchange),
                    MetadataExchangeBindings.CreateMexHttpBinding(),
                    new Uri("http://localhost/phone/project/mex", UriKind.Absolute));

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
