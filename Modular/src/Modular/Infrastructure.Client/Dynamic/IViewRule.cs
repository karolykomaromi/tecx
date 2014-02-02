namespace Infrastructure.Dynamic
{
    public interface IViewRule
    {
        void Apply(IRuleContext context);
    }
}