//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34003
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Hydra.Properties
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
                    IResourceManager temp = new ResourceManagerWrapper(new ResourceManager("Hydra.Properties.Resources", typeof(Resources).Assembly));
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

        public static string ApplicationName
        {
            get { return Resources.ResourceManager.GetString("ApplicationName", resourceCulture ?? CultureInfo.CurrentUICulture); }
        }

        public static string Books_Enter_Title
        {
            get { return Resources.ResourceManager.GetString("Books_Enter_Title", resourceCulture ?? CultureInfo.CurrentUICulture); }
        }

        public static string Books_IndexAsync_Title
        {
            get { return Resources.ResourceManager.GetString("Books_IndexAsync_Title", resourceCulture ?? CultureInfo.CurrentUICulture); }
        }

        public static string Books_Index_Title
        {
            get { return Resources.ResourceManager.GetString("Books_Index_Title", resourceCulture ?? CultureInfo.CurrentUICulture); }
        }

        public static string BookViewModel_Title
        {
            get { return Resources.ResourceManager.GetString("BookViewModel_Title", resourceCulture ?? CultureInfo.CurrentUICulture); }
        }

        public static string Create
        {
            get { return Resources.ResourceManager.GetString("Create", resourceCulture ?? CultureInfo.CurrentUICulture); }
        }

        public static string Delete
        {
            get { return Resources.ResourceManager.GetString("Delete", resourceCulture ?? CultureInfo.CurrentUICulture); }
        }

        public static string Edit
        {
            get { return Resources.ResourceManager.GetString("Edit", resourceCulture ?? CultureInfo.CurrentUICulture); }
        }

        public static string Home_Index_Title
        {
            get { return Resources.ResourceManager.GetString("Home_Index_Title", resourceCulture ?? CultureInfo.CurrentUICulture); }
        }

        public static string IJobDetail_Description
        {
            get { return Resources.ResourceManager.GetString("IJobDetail_Description", resourceCulture ?? CultureInfo.CurrentUICulture); }
        }

        public static string IJobDetail_Name
        {
            get { return Resources.ResourceManager.GetString("IJobDetail_Name", resourceCulture ?? CultureInfo.CurrentUICulture); }
        }

        public static string Jobs_Index_Title
        {
            get { return Resources.ResourceManager.GetString("Jobs_Index_Title", resourceCulture ?? CultureInfo.CurrentUICulture); }
        }

        public static string Jobs_Schedule_Title
        {
            get { return Resources.ResourceManager.GetString("Jobs_Schedule_Title", resourceCulture ?? CultureInfo.CurrentUICulture); }
        }

        public static string Registration_Register_Title
        {
            get { return Resources.ResourceManager.GetString("Registration_Register_Title", resourceCulture ?? CultureInfo.CurrentUICulture); }
        }

        public static string Schedule
        {
            get { return Resources.ResourceManager.GetString("Schedule", resourceCulture ?? CultureInfo.CurrentUICulture); }
        }

        public static string Enter
        {
            get { return Resources.ResourceManager.GetString("Enter", resourceCulture ?? CultureInfo.CurrentUICulture); }
        }

        public static string Register
        {
            get { return Resources.ResourceManager.GetString("Register", resourceCulture ?? CultureInfo.CurrentUICulture); }
        }

        public static string BookViewModel_ASIN
        {
            get { return Resources.ResourceManager.GetString("BookViewModel_ASIN", resourceCulture ?? CultureInfo.CurrentUICulture); }
        }

        public static string Settings_Index_Title
        {
            get { return Resources.ResourceManager.GetString("Settings_Index_Title", resourceCulture ?? CultureInfo.CurrentUICulture); }
        }

        public static string Settings_Edit_Title
        {
            get { return Resources.ResourceManager.GetString("Settings_Edit_Title", resourceCulture ?? CultureInfo.CurrentUICulture); }
        }

        public static string Setting_Name
        {
            get { return Resources.ResourceManager.GetString("Setting_Name", resourceCulture ?? CultureInfo.CurrentUICulture); }
        }

        public static string Setting_Value
        {
            get { return Resources.ResourceManager.GetString("Setting_Value", resourceCulture ?? CultureInfo.CurrentUICulture); }
        }
    }
}