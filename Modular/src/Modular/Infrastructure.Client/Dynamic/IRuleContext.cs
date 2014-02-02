namespace Infrastructure.Dynamic
{
    using Infrastructure.Options;

    public interface IRuleContext
    {
        IOptions Options { get; }

        IViewRegistry Registry { get; }
    }
}