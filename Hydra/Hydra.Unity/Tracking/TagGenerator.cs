namespace Hydra.Unity.Tracking
{
    using Microsoft.Practices.Unity;

    public abstract class TagGenerator
    {
        public abstract string GetTag(IUnityContainer container);
    }
}