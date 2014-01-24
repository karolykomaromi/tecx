namespace Infrastructure.I18n
{
    public class EchoResourceManager : IResourceManager
    {
        public string this[ResxKey key]
        {
            get { return key.ToString(); }
        }
    }
}