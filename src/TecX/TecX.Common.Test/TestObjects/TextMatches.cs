using System.Collections.Generic;
using TecX.Common.Specifications;

namespace TecX.Common.Test.TestObjects
{
    internal class TextMatches : CompareToValueSpecification<SearchTestEntity, string>
    {
        public override string Description
        {
            get { return "TextMatches"; }
        }

        public TextMatches(string text)
        {
            this.Value = text;
        }

        protected override bool IsMatchCore(SearchTestEntity candidate, ICollection<ISpecification<SearchTestEntity>> matchedSpecifications)
        {
            return candidate.Text == this.Value;
        }
    }
}