namespace TecX.Unity.Groups
{
    using System.Collections.Generic;

    public class SemanticGroupPolicy : ISemanticGroupPolicy
    {
        private readonly List<ScopedMapping> scopedMappings;

        public SemanticGroupPolicy()
        {
            this.scopedMappings = new List<ScopedMapping>();
        }

        public ICollection<ScopedMapping> ScopedMappings
        {
            get
            {
                return this.scopedMappings;
            }
        }
    }
}