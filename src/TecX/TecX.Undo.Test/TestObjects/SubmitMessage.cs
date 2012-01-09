namespace TecX.Undo.Test.TestObjects
{
    public class SubmitMessage : CountsUndoRedo
    {
        public string Message { get; set; }

        protected override void ExecuteCore()
        {
            base.ExecuteCore();

            this.Message = "1";
        }

        protected override void UnexecuteCore()
        {
            base.UnexecuteCore();

            this.Message = "0";
        }
    }
}