﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34003
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Cars.Properties
{
    using System;
    using System.ComponentModel;
    using System.Globalization;
    using System.Resources;
    using System.Threading;
    using Cars.I18n;

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
                    IResourceManager temp = new ResourceManagerWrapper(new ResourceManager("Cars.Properties.Resources", typeof(Resources).Assembly));
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

        public static string CurrencyMismatch
        {
            get { return Resources.ResourceManager.GetString("CurrencyMismatch", resourceCulture ?? CultureInfo.CurrentUICulture); }
        }

        public static string InvalidCurrencyFormatString
        {
            get { return Resources.ResourceManager.GetString("InvalidCurrencyFormatString", resourceCulture ?? CultureInfo.CurrentUICulture); }
        }

        public static string TranslationNotFound
        {
            get { return Resources.ResourceManager.GetString("TranslationNotFound", resourceCulture ?? CultureInfo.CurrentUICulture); }
        }
    }
}