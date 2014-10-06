namespace Modular.Web
{
    using System;
    using System.Web.Hosting;
    using System.Web.Routing;
    using Microsoft.AspNet.SignalR;
    using Microsoft.AspNet.SignalR.Hubs;
    using Modular.Web.Hosting;
    using Modular.Web.Hubs;

    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            // weberse 2014-02-28 setup Unity to resolve SignalR hubs
            GlobalHost.DependencyResolver.Register(typeof(IHubActivator), () => new UnityHubActivator());

            HubConfiguration configuration = new HubConfiguration
                {
                    EnableCrossDomain = true,
                    EnableDetailedErrors = true,
                    EnableJavaScriptProxies = true
                };

            RouteTable.Routes.MapHubs(configuration);

            // weberse 2014-02-28 The ERVPP has a fallback mechanism that uses the previous VPP when a file cannot be found in the embedded resources.
            // as there is only one image in the resources that fallback will be used in almost all cases.
            // To test if the provider is actually working modify the url in your browser and replace "Modular.aspx" with "images/blue.png".
            // If you see a small blue rectangle you are fine.
            VirtualPathProvider provider = EmbeddedResourceVirtualPathProvider.Create(new VirtualPathUtilityWrapper(), typeof(UnityServiceHostFactory).Assembly);
            HostingEnvironment.RegisterVirtualPathProvider(provider);
        }

        protected void Session_Start(object sender, EventArgs e)
        {
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
        }

        protected void Application_Error(object sender, EventArgs e)
        {
        }

        protected void Session_End(object sender, EventArgs e)
        {
        }

        protected void Application_End(object sender, EventArgs e)
        {
        }
    }
}