namespace Hydra.Jobs.Input
{
    using System.Diagnostics.Contracts;

    public class NoValueCmdLineParameter : CmdLineParameter
    {
        public NoValueCmdLineParameter(string name)
            : base(name, string.Empty)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(name));
        }

        public override void Accept(ICmdLineParameterVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}