namespace Infrastructure.Options
{
    public interface IOptions
    {
        object this[Option option] { get; set; }

        bool KnowsAbout(Option option);
    }
}