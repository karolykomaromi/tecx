namespace TecX.Search.Split
{
    using TecX.Common;

    public class StringSplitContext
    {
        private readonly StringSplitParameterCollection splitResults;

        private string stringToSplit;

        public StringSplitContext()
        {
            this.splitResults = new StringSplitParameterCollection();
        }

        public StringSplitParameterCollection SplitResults
        {
            get
            {
                return this.splitResults;
            }
        }

        public string StringToSplit
        {
            get
            {
                return this.stringToSplit;
            }

            set
            {
                Guard.AssertNotNull(value, "StringToSplit");

                this.stringToSplit = value;
            }
        }
    }
}
