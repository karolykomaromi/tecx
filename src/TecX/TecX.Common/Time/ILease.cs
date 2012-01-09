namespace TecX.Common.Time
{
    public interface ILease
    {
        bool IsExpired { get; }

        void Renew();
    }
}
