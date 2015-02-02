namespace Infrastructure.Triggers
{
    using System.Diagnostics.Contracts;
    using System.Windows.Controls;

    public class PopulatingCompletedToken : Token
    {
        private readonly AutoCompleteBox autoCompleteBox;
        private readonly PopulatingEventArgs eventArgs;

        public PopulatingCompletedToken(AutoCompleteBox autoCompleteBox, object commandParameter, PopulatingEventArgs eventArgs)
            : base(autoCompleteBox.PopulateComplete, commandParameter)
        {
            Contract.Requires(autoCompleteBox != null);
            Contract.Requires(commandParameter != null);

            this.eventArgs = eventArgs;
            this.autoCompleteBox = autoCompleteBox;
        }

        public AutoCompleteBox AutoCompleteBox
        {
            get { return this.autoCompleteBox; }
        }

        public PopulatingEventArgs EventArgs
        {
            get { return this.eventArgs; }
        }

        public override string ToString()
        {
            return "Dispose me to signal to the AutoCompleteBox that the populating cycle is finished.";
        }
    }
}