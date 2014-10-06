namespace TecX.Common.Test.TestObjects
{
    using TecX.Common.Specifications;

    internal class TextMatches : Specification<SearchTestEntity>
    {
        private readonly string text;

        public override string Description
        {
            get { return "TextMatches"; }
        }

        public TextMatches(string text)
        {
            this.text = text;
        }

        public override bool IsSatisfiedBy(SearchTestEntity candidate)
        {
            return candidate.Text == this.text;
        }
    }
}