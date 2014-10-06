namespace TecX.Search.Parse
{
    using TecX.Common;
    using TecX.Search.Split;

    public class ParameterParseContext
    {
        private readonly StringSplitParameter stringToParse;

        public ParameterParseContext(StringSplitParameter stringToParse)
        {
            Guard.AssertNotNull(stringToParse, "StringToParse");

            this.stringToParse = stringToParse;
        }

        public StringSplitParameter StringToParse
        {
            get
            {
                return this.stringToParse;
            }
        }

        public bool ParseComplete { get; set; }

        public SearchParameter Parameter { get; set; }
    }
}