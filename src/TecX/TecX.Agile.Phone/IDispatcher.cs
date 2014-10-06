namespace TecX.Agile.Phone
{
    using System;

    public interface IDispatcher
    {
        void BeginInvoke(Action action);

        void BeginInvoke(Delegate d, params object[] args);
    }
}