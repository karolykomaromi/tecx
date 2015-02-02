namespace Hydra.Infrastructure
{
    public interface ICloneable<out T>
    {
        T Clone();
    }
}