namespace TecX.Undo.Test.TestObjects
{
    public class SubmitMessage : Command
    {
        public string Message { get; set; }

        protected override void ExecuteCore()
        {
            this.Message = "1";
        }

        protected override void UnexecuteCore()
        {
            this.Message = "0";
        }
    }
}