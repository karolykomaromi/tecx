namespace Infrastructure
{
    using System;
    using System.Diagnostics.Contracts;

    public class Token : IDisposable
    {
        private readonly object parameter;
        private readonly Action onDispose;

        public Token(Action onDispose, object parameter)
        {
            Contract.Requires(onDispose != null);
            Contract.Requires(parameter != null);

            this.onDispose = onDispose;
            this.parameter = parameter;
        }

        public Token(Action onDispose)
            : this(onDispose, null)
        {
        }

        public object Parameter
        {
            get { return this.parameter; }
        }

        public void Dispose()
        {
            this.onDispose();
        }
    }
}
