<#@ template debug="false" hostspecific="true" language="C#" #>
<#@ output extension=".Designer.cs" #>
<#@ assembly name="System.Xml" #>
<#@ assembly name="System.Xml.Linq" #>
<#@ assembly name="$(t4LibDir)\Janus.TextTemplating.dll" #>
<#@ import namespace="System.IO" #>
<#@ import namespace="System.Xml.Linq" #>
<#@ import namespace="Janus.TextTemplating" #>
namespace $rootnamespace$.Properties
{
    using System.ComponentModel;
    using System.Globalization;
    using System.Resources;
    using Janus.I18n.Resources;

    public static class Resources
    {
        private static IResourceManager resourceManager;

        private static CultureInfo resourceCulture;

        [EditorBrowsableAttribute(EditorBrowsableState.Advanced)]
        public static IResourceManager ResourceManager
        {
            get
            {
                if(resourceManager == null)
                {
                    IResourceManager temp = new ResourceManagerAdapter(new ResourceManager("$rootnamespace$.Properties.Resources", typeof(Resources).Assembly));
                    resourceManager = temp;
                }

                return resourceManager;
            }

            set
            {
                resourceManager = value;
            }
        }

        [EditorBrowsableAttribute(EditorBrowsableState.Advanced)]
        public static CultureInfo Culture
        {
            get
            {
                return resourceCulture;
            }

            set
            {
                resourceCulture = value;
            }
        }
<# 
    string resxFileName = this.Host.TemplateFile.Replace(".tt", ".resx");
    XDocument doc = XDocument.Load(resxFileName);

    ResourcesTemplate template = ResourcesTemplate.FromNode(doc);

    Write(template.Properties());
 #>
    }
}