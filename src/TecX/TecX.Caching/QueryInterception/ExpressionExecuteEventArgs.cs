namespace TecX.Caching.QueryInterception
{
    using System;
    using System.Linq.Expressions;

    public class ExpressionExecuteEventArgs : EventArgs
    {
        public bool Handled { get; set; }

        public object Result { get; set; }

        public Expression Expression { get; set; }
    }
}