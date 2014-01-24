namespace Infrastructure.I18n
{
    public class WebSvcResourceManager : IResourceManager
    {
        public string this[ResxKey key]
        {
            get { return key.ToString(); }
        }
    }
}