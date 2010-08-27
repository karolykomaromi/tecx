namespace TecX.Common.Event
{
    public interface ICancellationToken
    {
        bool Cancel { get; set; }
    }
}