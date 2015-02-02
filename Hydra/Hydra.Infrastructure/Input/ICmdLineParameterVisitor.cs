namespace Hydra.Infrastructure.Input
{
    public interface ICmdLineParameterVisitor
    {
        void Visit(CmdLineParameter parameter);

        void Visit(NoValueCmdLineParameter parameter);

        void Visit(MultiValueCmdLineParameter parameter);
    }
}