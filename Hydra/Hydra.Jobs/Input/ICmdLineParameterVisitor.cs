namespace Hydra.Jobs.Input
{
    public interface ICmdLineParameterVisitor
    {
        void Visit(CmdLineParameter parameter);

        void Visit(NoValueCmdLineParameter parameter);

        void Visit(MultiValueCmdLineParameter parameter);
    }
}