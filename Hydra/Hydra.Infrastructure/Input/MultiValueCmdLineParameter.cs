namespace Hydra.Infrastructure.Input
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics.Contracts;

    public class MultiValueCmdLineParameter : CmdLineParameter
    {
        private readonly IReadOnlyCollection<string> values;

        public MultiValueCmdLineParameter(string name, params string[] values)
            : base(name, string.Join(" ", values ?? new string[0]))
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(name));

            this.values = new ReadOnlyCollection<string>(values ?? new string[0]);
        }

        public IReadOnlyCollection<string> Values
        {
            get
            {
                Contract.Ensures(Contract.Result<IReadOnlyCollection<string>>() != null);

                return this.values;
            }
        }

        public override void Accept(ICmdLineParameterVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}