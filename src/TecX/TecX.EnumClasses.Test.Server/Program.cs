namespace TecX.EnumClasses.Test.Server
{
    using System;
    using System.Runtime.Serialization;
    using System.ServiceModel;
    using System.ServiceModel.Description;

    using TecX.EnumClasses.Test.TestObjects;

    public class Program
    {
        public static void Main(string[] args)
        {
            using (ServiceHost host = new ServiceHost(typeof(SortingService), new Uri("http://localhost:12345/svc")))
            {
                ServiceMetadataBehavior smb = new ServiceMetadataBehavior();
                smb.HttpGetEnabled = true;
                smb.MetadataExporter.PolicyVersion = PolicyVersion.Policy15;

                ////WsdlExporter exporter = smb.MetadataExporter as WsdlExporter;

                ////if (exporter != null)
                ////{
                ////    object dataContractExporter;
                ////    XsdDataContractExporter xsdInventoryExporter;
                ////    if (!exporter.State.TryGetValue(typeof(XsdDataContractExporter), out dataContractExporter))
                ////    {
                ////        xsdInventoryExporter = new XsdDataContractExporter(exporter.GeneratedXmlSchemas);
                ////    }
                ////    else
                ////    {
                ////        xsdInventoryExporter = (XsdDataContractExporter)dataContractExporter;
                ////    }

                ////    exporter.State.Add(typeof(XsdDataContractExporter), xsdInventoryExporter);

                ////    if (xsdInventoryExporter.Options == null)
                ////    {
                ////        xsdInventoryExporter.Options = new ExportOptions();
                ////    }

                ////    xsdInventoryExporter.Options.DataContractSurrogate = new EnumerationClassesSurrogate();
                ////}

                host.Description.Behaviors.Add(smb);

                host.AddServiceEndpoint(ServiceMetadataBehavior.MexContractName, MetadataExchangeBindings.CreateMexHttpBinding(), "mex");

                host.AddServiceEndpoint(typeof(ISortingService), new BasicHttpBinding(), string.Empty);

                host.Open();

                Console.WriteLine("Press ENTER to exit");
                Console.ReadLine();
            }
        }
    }
}
