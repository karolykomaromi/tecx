namespace Infrastructure.I18n
{
    public class WebSvcResourceManager : IResourceManager
    {
        public string this[string key]
        {
            get { return key; }
        }
    }
}