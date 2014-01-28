namespace Infrastructure.Triggers
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Interactivity;

    public class AutoCompleteToCommand : TriggerAction<AutoCompleteBox>
    {
        public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register(
            "CommandParameter",
            typeof(object),
            typeof(AutoCompleteToCommand),
            new PropertyMetadata(null, OnCommandParameterChanged));

        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(
            "Command",
            typeof(ICommand),
            typeof(AutoCompleteToCommand),
            new PropertyMetadata(
                null,
                (s, e) => OnCommandChanged(s as AutoCompleteToCommand, e)));

        public static readonly DependencyProperty MustToggleIsEnabledProperty = DependencyProperty.Register(
            "MustToggleIsEnabled",
            typeof(bool),
            typeof(AutoCompleteToCommand),
            new PropertyMetadata(
                false,
                OnMustToggleIsEnabledChanged));

        private object commandParameterValue;

        private bool? mustToggleValue;

        public ICommand Command
        {
            get { return (ICommand)this.GetValue(CommandProperty); }

            set { this.SetValue(CommandProperty, value); }
        }

        public object CommandParameter
        {
            get { return this.GetValue(CommandParameterProperty); }

            set { this.SetValue(CommandParameterProperty, value); }
        }

        public object CommandParameterValue
        {
            get
            {
                return this.commandParameterValue ?? this.CommandParameter;
            }

            set
            {
                this.commandParameterValue = value;
                this.EnableDisableElement();
            }
        }

        public bool MustToggleIsEnabled
        {
            get { return (bool)this.GetValue(MustToggleIsEnabledProperty); }

            set { this.SetValue(MustToggleIsEnabledProperty, value); }
        }

        public bool MustToggleIsEnabledValue
        {
            get
            {
                return this.mustToggleValue == null ? this.MustToggleIsEnabled : this.mustToggleValue.Value;
            }

            set
            {
                this.mustToggleValue = value;
                this.EnableDisableElement();
            }
        }

        protected override void Invoke(object parameter)
        {
            if (this.AssociatedElementIsDisabled())
            {
                return;
            }

            ICommand command = this.Command;
            var commandParameter = this.CommandParameterValue;

            PopulatingCompletedToken token = new PopulatingCompletedToken(this.AssociatedObject, commandParameter, (PopulatingEventArgs)parameter);

            if (command != null && command.CanExecute(token))
            {
                command.Execute(token);
            }
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            this.EnableDisableElement();
        }

        private static void OnCommandParameterChanged(DependencyObject s, DependencyPropertyChangedEventArgs e)
        {
            var sender = s as AutoCompleteToCommand;
            if (sender == null)
            {
                return;
            }

            if (sender.AssociatedObject == null)
            {
                return;
            }

            sender.EnableDisableElement();
        }

        private static void OnCommandChanged(AutoCompleteToCommand element, DependencyPropertyChangedEventArgs e)
        {
            if (element == null)
            {
                return;
            }

            if (e.OldValue != null)
            {
                ((ICommand)e.OldValue).CanExecuteChanged -= element.OnCommandCanExecuteChanged;
            }

            var command = (ICommand)e.NewValue;

            if (command != null)
            {
                command.CanExecuteChanged += element.OnCommandCanExecuteChanged;
            }

            element.EnableDisableElement();
        }

        private static void OnMustToggleIsEnabledChanged(DependencyObject s, DependencyPropertyChangedEventArgs e)
        {
            var sender = s as AutoCompleteToCommand;
            if (sender == null)
            {
                return;
            }

            if (sender.AssociatedObject == null)
            {
                return;
            }

            sender.EnableDisableElement();
        }

        private bool AssociatedElementIsDisabled()
        {
            var element = this.AssociatedObject;

            return this.AssociatedObject == null || (element != null && !element.IsEnabled);
        }

        private void OnCommandCanExecuteChanged(object sender, EventArgs e)
        {
            this.EnableDisableElement();
        }

        private void EnableDisableElement()
        {
            var element = this.AssociatedObject;

            if (element == null)
            {
                return;
            }

            var command = this.Command;

            if (this.MustToggleIsEnabledValue && command != null)
            {
                element.IsEnabled = command.CanExecute(new PopulatingCompletedToken(this.AssociatedObject, this.CommandParameterValue, null));
            }
        }
    }
}
