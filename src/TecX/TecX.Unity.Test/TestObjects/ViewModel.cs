namespace TecX.Unity.Test.TestObjects
{
    public class ViewModel
    {
        public ViewModel(ICommand loadCommand, ICommand saveCommand)
        {
            this.LoadCommand = loadCommand;
            this.SaveCommand = saveCommand;
        }

        public ICommand LoadCommand { get; set; }

        public ICommand SaveCommand { get; set; }
    }
}