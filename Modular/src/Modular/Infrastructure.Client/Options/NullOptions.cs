namespace Infrastructure.Options
{
    public class NullOptions : IOptions
    {
        public object this[Option option]
        {
            get { return null; }
            set { }
        }

        public bool KnowsAbout(Option option)
        {
            return false;
        }
    }
}
