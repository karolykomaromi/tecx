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

        protected override bool IsMatchCore(SearchTestEntity candidate)
        {
            return candidate.Text == this.Value;
        }
    }
}