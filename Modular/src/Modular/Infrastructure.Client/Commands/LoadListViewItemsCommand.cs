namespace Infrastructure.Commands
{
    using System;
    using System.Windows.Input;
    using Infrastructure.Events;

    public class LoadListViewItemsCommand : ICommand, ISubscribeTo<CanExecuteChanged>
    {
        public event EventHandler CanExecuteChanged = delegate { };

        public bool CanExecute(object parameter)
        {
            LoadListViewItemsCommandParameter p = parameter as LoadListViewItemsCommandParameter;

            return p != null && 
                p.ViewModel != null && 
                !p.ViewModel.IsCurrentlyLoading &&
                p.RowIndex + p.ViewModel.LoadNextBeforeEndOfItemsThreshold >= p.ViewModel.Items.Count;
        }

        public void Execute(object parameter)
        {
            LoadListViewItemsCommandParameter p = parameter as LoadListViewItemsCommandParameter;

            if (p != null && 
                p.ViewModel != null && 
                !p.ViewModel.IsCurrentlyLoading)
            {
                p.ViewModel.LoadListViewItems();
            }
        }

        public void Handle(CanExecuteChanged message)
        {
            this.CanExecuteChanged(this, EventArgs.Empty);
        }
    }
}