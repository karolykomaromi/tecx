using System;
using System.Windows;
using System.Windows.Controls;

using TecX.Agile.ViewModel;
using TecX.Agile.ViewModel.Messages;
using TecX.Common;
using TecX.Common.Event;

namespace TecX.Agile.View.Behavior
{
    public class HighlightTextBoxHandler : BehaviorHandler,
                                           ISubscribeTo<IncomingRequestToHighlightField>
    {
        #region Fields

        private Guid _id;
        private string _fieldName;
        //private IDisposable _subscription;
        private readonly IEventAggregator _eventAggregator;

        #endregion Fields

        #region c'tor

        public HighlightTextBoxHandler(IEventAggregator eventAggregator)
        {
            Guard.AssertNotNull(eventAggregator, "eventAggregator");

            _eventAggregator = eventAggregator;
        }

        #endregion c'tor

        #region Overrides of BehaviorHandler

        protected override void DoAttach(FrameworkElement element)
        {
            UserControl ctrl;
            string fieldName;
            if (UIHelper.TryFindAncestor(element, out ctrl) &&
                UIHelper.TryGetName(ctrl, element, out fieldName))
            {
                //TODO weberse if ctrl.DataContext == null should hook up a handler to ctrl.DataContextChanged and
                //act a soon as the context is available. hook up handlers and remove listener to DataContextChanged
                PlanningArtefact artefact = ctrl.DataContext as PlanningArtefact;

                if (artefact != null)
                {
                    _fieldName = fieldName;
                    _id = artefact.Id;

                    Element.GotFocus += OnGotFocus;

                    ////whenever a request comes in to highlight a textbox identified by the Id of the underlying PlanningArtefact from the
                    ////DataContext and the name of the field
                    //var highlight = from evt in Observable.FromEvent<RemoteHighlightEventArgs>(
                    //    handler => RemoteHighlight.IncomingRequestToHighlightField += handler,
                    //    handler => RemoteHighlight.IncomingRequestToHighlightField -= handler)
                    //                where evt.EventArgs.ArtefactId == _id &&
                    //                      evt.EventArgs.FieldName == _fieldName
                    //                select evt;

                    ////...we just set the focus to that control
                    //_subscription = highlight.Subscribe(x => Element.Focus());

                    _eventAggregator.Subscribe(this);
                }
            }
        }

        //TODO weberse this would be a perfect little hook to add textbox focus
        //notification at the top level. just hook up the behavior to a story card and that
        //would attach handlers to all textboxes inside that story card
        //private void OnLoaded(object sender, RoutedEventArgs e)
        //{
        //    UserControl ctrl = sender as UserControl;

        //    var textBoxes = TreeHelper.FindChildren<TextBox>(ctrl);

        //    int count = textBoxes.Count();
        //}

        private void OnGotFocus(object sender, RoutedEventArgs e)
        {
            _eventAggregator.Publish(new OutgoingNotificationToHighlightField(_id, _fieldName));

            //whenever the textbox receives focus we signal that via an event to the outside world
            //RemoteHighlight.RaiseOutgoingNotificationThatFieldWasHighlighted(Element, new RemoteHighlightEventArgs(_id, _fieldName));
        }

        protected override void DoDetach()
        {
            Element.GotFocus -= OnGotFocus;

            //_subscription.Dispose();
            //_subscription = null;

            _fieldName = null;
            _id = Guid.Empty;

            _eventAggregator.Unsubscribe(this);
        }

        #endregion

        #region Implementation of ISubscribeTo<in IncomingRequestToHighlightField>

        public void Handle(IncomingRequestToHighlightField message)
        {
            Guard.AssertNotNull(message, "message");
            Guard.AssertNotEmpty(message.FieldName, "message.FieldName");

            if (message.ArtefactId == _id && message.FieldName == _fieldName)
            {
                Element.Focus();
            }
        }

        #endregion
    }
}