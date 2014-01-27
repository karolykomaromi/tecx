namespace Infrastructure.Triggers
{
    using System;

    public class AutoCompleteParameter
    {
        public object CommandParameter { get; set; }

        public Action CancelPopulating { get; set; }

        public Action PopulateComplete { get; set; }
    }
}