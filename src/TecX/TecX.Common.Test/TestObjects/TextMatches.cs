using TecX.Common.Specifications;

namespace TecX.Common.Test.TestObjects
{
    internal class TextMatches : CompareToValueSpecification<SearchTestEntity, string>
    {
        public TextMatches()
        {
        }

        public TextMatches(string text)
        {
            this.Value = text;
        }

        public override bool IsSatisfiedBy(SearchTestEntity candidate)
        {
            return candidate.Text == this.Value;
        }
    }
}