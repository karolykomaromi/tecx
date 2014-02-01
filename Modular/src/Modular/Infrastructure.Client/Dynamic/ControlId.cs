namespace Infrastructure.Dynamic
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;

    [DebuggerDisplay("{id}")]
    public struct ControlId
    {
        public static readonly ControlId Empty = new ControlId();

        private readonly string id;

        public ControlId(string id)
        {
            Contract.Requires(!string.IsNullOrEmpty(id));

            this.id = string.Intern(id);
        }

        public ReadOnlyCollection<ControlId> Path
        {
            get
            {
                Contract.Ensures(Contract.Result<ReadOnlyCollection<ControlId>>() != null);
                Contract.Ensures(Contract.Result<ReadOnlyCollection<ControlId>>().Count > 0);

                return new ReadOnlyCollection<ControlId>(new List<ControlId> { ControlId.Empty });
            }
        }
    }
}