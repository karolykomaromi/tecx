namespace Hydra.Infrastructure
{
    public interface IFreezable<out T>
    {
        bool IsMutable { get; }

        T Freeze();
    }
}