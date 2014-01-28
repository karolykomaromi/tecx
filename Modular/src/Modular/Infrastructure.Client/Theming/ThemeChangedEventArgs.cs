namespace Infrastructure.Theming
{
    using System;

    public class ThemeChangedEventArgs : EventArgs
    {
        private readonly Uri themeUri;

        public ThemeChangedEventArgs(Uri themeUri)
        {
            this.themeUri = themeUri;
        }

        public Uri ThemeUri
        {
            get { return this.themeUri; }
        }
    }
}