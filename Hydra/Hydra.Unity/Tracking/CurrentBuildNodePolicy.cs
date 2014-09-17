using System.Collections.Generic;

namespace Hydra.Unity.Tracking
{
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