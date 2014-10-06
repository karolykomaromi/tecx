namespace Hydra.Unity.Tracking
{
    using System.Globalization;
    using Microsoft.Practices.Unity;

    public class HierachicalTagGenerator : TagGenerator
    {
        public override string GetTag(IUnityContainer container)
        {
            int level = 0;

            IUnityContainer parent = container.Parent;
            while (parent != null)
            {
                level++;
                parent = parent.Parent;
            }

            return "level" + level.ToString(CultureInfo.InvariantCulture);
        }
    }
}