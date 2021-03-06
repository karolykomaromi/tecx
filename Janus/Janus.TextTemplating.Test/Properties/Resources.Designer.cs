namespace Janus.TextTemplating.Test.Properties
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
                    IResourceManager temp = new ResourceManagerAdapter(new ResourceManager("Janus.TextTemplating.Test.Properties.Resources", typeof(Resources).Assembly));
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

        public static byte[] MyJsonFile
        {
            get
            {
                object obj = ResourceManager.GetObject("MyJsonFile", resourceCulture);
                return (byte[])obj;
            }
        }

        public static string MyString
        {
            get
            {
                return ResourceManager.GetString("MyString", resourceCulture);
            }
        }

        public static string MyTextFile
        {
            get
            {
                return ResourceManager.GetString("MyTextFile", resourceCulture);
            }
        }
    }
}