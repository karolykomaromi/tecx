namespace Hydra
{
    using System.Web.Mvc;

    /// <summary>
    /// <seealso cref="http://timgthomas.com/2013/10/feature-folders-in-asp-net-mvc/"/>
    /// </summary>
    public class FeatureViewLocationRazorViewEngine : RazorViewEngine
    {
        public FeatureViewLocationRazorViewEngine()
        {
            var featureFolderViewLocationFormats = new[]
                {
                    "~/Features/{1}/{0}.cshtml",
                    "~/Features/{1}/{0}.vbhtml",
                    "~/Features/Shared/{0}.cshtml",
                    "~/Features/Shared/{0}.vbhtml",
                };

            this.ViewLocationFormats = featureFolderViewLocationFormats;
            this.MasterLocationFormats = featureFolderViewLocationFormats;
            this.PartialViewLocationFormats = featureFolderViewLocationFormats;
        }
    }
}