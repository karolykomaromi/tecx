using System.Web.Routing;
using Microsoft.AspNet.SignalR;

namespace Modular.Web
{
    using System;

    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            HubConfiguration configuration = new HubConfiguration
                {
                    EnableCrossDomain = true,
                    EnableDetailedErrors = true,
                    EnableJavaScriptProxies = true
                };

            RouteTable.Routes.MapHubs(configuration);
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