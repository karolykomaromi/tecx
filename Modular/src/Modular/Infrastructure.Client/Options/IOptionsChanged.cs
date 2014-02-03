namespace Infrastructure.Options
{
    public interface IOptionsChanged<out TOptions>
        where TOptions : IOptions
    {
        TOptions Options { get; }

        Option OptionName { get; }
    }
}