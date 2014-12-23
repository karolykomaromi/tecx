namespace Hydra.Import
{
    public abstract class ImportMessage
    {
        public static readonly ImportMessage Empty = new EmptyMessage();

        private readonly string message;

        protected ImportMessage(string message)
        {
            this.message = message;
        }

        public override string ToString()
        {
            return this.message;
        }

        private class EmptyMessage : ImportMessage
        {
            public EmptyMessage()
                : base(string.Empty)
            {
            }
        }
    }
}