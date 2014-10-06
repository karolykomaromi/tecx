namespace Hydra.Unity.Tracking
{
    using System.Collections.Generic;

    public class CurrentBuildNodePolicy : ICurrentBuildNodePolicy
    {
        private readonly Stack<string> tags;

        public CurrentBuildNodePolicy()
        {
            this.tags = new Stack<string>();
        }

        public Stack<string> Tags
        {
            get { return this.tags; }
        }
    }
}