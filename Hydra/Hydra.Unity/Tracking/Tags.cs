namespace Hydra.Unity.Tracking
{
    using System;
    using System.Globalization;
    using System.Runtime.CompilerServices;
    using Hydra.Infrastructure;
    using Microsoft.Practices.Unity;

    public abstract class Tags : Enumeration<Tags>
    {
        public static readonly Tags Random = new RandomTag();

        public static readonly Tags Hierarchy = new HierarchyTag();

        private Tags(string name, int key)
            : base(name, key)
        {
        }

        public abstract string GetTag(IUnityContainer container);

        private class RandomTag : Tags
        {
            public RandomTag([CallerMemberName] string name = "", [CallerLineNumber] int key = -1)
                : base(name, key)
            {
            }

            public override string GetTag(IUnityContainer container)
            {
                return Guid.NewGuid().ToString();
            }
        }

        private class HierarchyTag : Tags
        {
            public HierarchyTag([CallerMemberName] string name = "", [CallerLineNumber] int key = -1)
                : base(name, key)
            {
            }

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
}