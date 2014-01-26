namespace Infrastructure.Options
{
    public interface IOptions
    {
        object this[OptionName optionName] { get; }
    }
}