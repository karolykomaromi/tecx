﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34003
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Hydra.Import.Properties
{
    using System;
    using System.ComponentModel;
    using System.Globalization;
    using System.Resources;
    using System.Threading;
    using Hydra.Infrastructure.I18n;

    public class Resources
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
                    IResourceManager temp = new ResourceManagerWrapper(new ResourceManager("Hydra.Import.Properties.Resources", typeof(Resources).Assembly));
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
    }
}