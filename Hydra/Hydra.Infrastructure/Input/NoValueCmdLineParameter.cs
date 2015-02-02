namespace Hydra.Infrastructure.Input
{
    using System.Diagnostics.Contracts;

    public class NoValueCmdLineParameter : CmdLineParameter
    {
        public NoValueCmdLineParameter(string name)
            : base(name, string.Empty)
        {
        }

        public override void Accept(ICmdLineParameterVisitor visitor)
        {
            Contract.Requires(visitor != null);

            visitor.Visit(this);
        }
    }
}