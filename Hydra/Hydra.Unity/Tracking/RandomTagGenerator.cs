namespace Hydra.Unity.Tracking
{
    using System;
    using Microsoft.Practices.Unity;

    public class RandomTagGenerator : TagGenerator
    {
        public override string GetTag(IUnityContainer container)
        {
            return Guid.NewGuid().ToString();
        }
    }
}