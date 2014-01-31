namespace Infrastructure.Theming
{
    using System;

    public class ThemeChanged
    {
        private readonly Uri themeUri;

        public ThemeChanged(Uri themeUri)
        {
            this.themeUri = themeUri;
        }

        public Uri ThemeUri
        {
            get { return this.themeUri; }
        }
    }
}