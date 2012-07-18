using TecX.Agile.Error;

namespace TecX.Agile.Peer
{
    public class PnrpNotAvailableException : AgileBaseException
    {
        public PnrpNotAvailableException()
            : base("PNRP is not available on this machine")
        {
        }
    }
}
