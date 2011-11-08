namespace TecX.Unity.Groups
{
    using System.Collections.Generic;

    public class SemanticGroupPolicy : ISemanticGroupPolicy
    {
        private readonly List<Using> usings;

        public SemanticGroupPolicy()
        {
            usings = new List<Using>();
        }

        public ICollection<Using> Usings
        {
            get
            {
                return usings;
            }
        }
    }
}