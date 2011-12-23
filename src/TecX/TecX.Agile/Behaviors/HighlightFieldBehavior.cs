namespace TecX.Agile.Behaviors
{
    using System;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Interactivity;

    using Microsoft.Practices.ServiceLocation;

    using TecX.Agile.Infrastructure.Commands;
    using TecX.Agile.Infrastructure.Events;
    using TecX.Agile.Utilities;
    using TecX.Agile.ViewModels;
    using TecX.Event;

    public class HighlightFieldBehavior : Behavior<TextBox>, ISubscribeTo<HighlightField>, ISubscribeTo<MoveCaret>
    {
        private IEventAggregator eventAggregator;
        private Guid id;
        private string fieldName;
        
        public void Handle(HighlightField message)
        {
            if (message.ArtefactId == this.id &&
                message.FieldName == this.fieldName)
            {
                AssociatedObject.Focus();
            }
        }

        public void Handle(MoveCaret message)
        {
            if (message.ArtefactId == this.id &&
                message.FieldName == this.fieldName)
            {
                AssociatedObject.CaretIndex = message.CaretIndex;
            }
        }

        protected override void OnAttached()
        {
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
            {
                return;
            }

            // can't inject dependencies to behavior :(
            this.eventAggregator = ServiceLocator.Current.GetInstance<IEventAggregator>();

            UserControl ctrl;
            string fn;
            if (TreeHelper.TryFindAncestor(AssociatedObject, out ctrl) &&
                TreeHelper.TryGetName(ctrl, AssociatedObject, out fn))
            {
                StoryCardViewModel card = ctrl.DataContext as StoryCardViewModel;

                if (card != null)
                {
                    this.id = card.Id;
                }

                ctrl.DataContextChanged += (s, e) =>
                {
                    card = ctrl.DataContext as StoryCardViewModel;

                    if (card != null)
                    {
                        this.id = card.Id;
                    }
                };

                this.fieldName = fn;

                this.AssociatedObject.GotFocus += this.OnGotFocus;
                this.AssociatedObject.KeyUp += this.OnKeyUp;
            }

            this.eventAggregator.Subscribe(this);
        }

        protected override void OnDetaching()
        {
            this.AssociatedObject.GotFocus -= this.OnGotFocus;

            this.fieldName = null;
            this.id = Guid.Empty;

            this.eventAggregator.Unsubscribe(this);
        }

        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            if (AssociatedObject.IsFocused)
            {
                this.eventAggregator.Publish(new CaretMoved(this.id, this.fieldName, AssociatedObject.CaretIndex));
            }
        }

        // TODO weberse this would be a perfect little hook to add textbox focus
        // notification at the top level. just hook up the behavior to a story card and that
        // would attach handlers to all textboxes inside that story card
        ////private void OnLoaded(object sender, RoutedEventArgs e)
        ////{
        ////    UserControl ctrl = sender as UserControl;

        ////    var textBoxes = TreeHelper.FindChildren<TextBox>(ctrl);

        ////    int count = textBoxes.Count();
        ////}

        private void OnGotFocus(object sender, RoutedEventArgs e)
        {
            this.eventAggregator.Publish(new FieldHighlighted(this.id, this.fieldName));
        }
    }
}
