namespace Infrastructure.I18n
{
    public class EchoResourceManager : IResourceManager
    {
        public string this[string key]
        {
            get { return key; }
        }
    }
}