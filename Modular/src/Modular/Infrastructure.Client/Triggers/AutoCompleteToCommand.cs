namespace Infrastructure.Triggers
{
    using System;
    using System.Windows.Controls;
    using GalaSoft.MvvmLight.Command;

    public class AutoCompleteToCommand : EventToCommand
    {
        protected override void OnAttached()
        {
            if (this.AssociatedObject as AutoCompleteBox == null)
            {
                throw new InvalidOperationException("Sorry, I derived from EventToCommand because I was too lazy. You cannot attach to anything but AutoCompleteBox.");
            }

            base.OnAttached();
        }

        protected override void Invoke(object parameter)
        {
            if (this.AssociatedElementIsDisabled())
            {
                return;
            }

            var command = this.Command;
            var commandParameter = this.CommandParameterValue;

            PopulatingEventArgs e = (PopulatingEventArgs)parameter;
            AutoCompleteBox box = (AutoCompleteBox)this.AssociatedObject;

            var cp = new AutoCompleteParameter
                {
                    CancelPopulating = () => e.Cancel = true,
                    PopulateComplete = box.PopulateComplete,
                    CommandParameter = commandParameter
                };

            if (command != null && command.CanExecute(cp))
            {
                command.Execute(cp);
            }
        }

        private bool AssociatedElementIsDisabled()
        {
            Control element = this.AssociatedObject as Control;

            return this.AssociatedObject == null || (element != null && !element.IsEnabled);
        }
    }
}
