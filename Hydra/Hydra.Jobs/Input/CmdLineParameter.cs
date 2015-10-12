namespace Hydra.Jobs.Input
{
    using System.Diagnostics.Contracts;

    public class CmdLineParameter
    {
        private readonly string name;

        private readonly string value;

        public CmdLineParameter(string name, string value)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(name));
            Contract.Requires(value != null);

            this.name = name;
            this.value = value;
        }

        public string Name
        {
            get { return this.name; }
        }

        public string Value
        {
            get { return this.value; }
        }

        public virtual void Accept(ICmdLineParameterVisitor visitor)
        {
            Contract.Requires(visitor != null);

            visitor.Visit(this);
        }
    }
}