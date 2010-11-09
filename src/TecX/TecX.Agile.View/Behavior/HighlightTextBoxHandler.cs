using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

using TecX.Agile.ViewModel;

namespace TecX.Agile.View.Behavior
{
    public class HighlightTextBoxHandler : BehaviorHandler
    {
        private Guid _id;
        private string _fieldName;
        private IDisposable _subscription;

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

                    //whenever a request comes in to highlight a textbox identified by the Id of the underlying PlanningArtefact from the
                    //DataContext and the name of the field
                    var highlight = from evt in Observable.FromEvent<HighlightEventArgs>(
                        handler => HighlightSource.HighlightField += handler,
                        handler => HighlightSource.HighlightField -= handler)
                                    where evt.EventArgs.Id == _id &&
                                          evt.EventArgs.FieldName == _fieldName
                                    select evt;

                    //...we just set the focus to that control
                    _subscription = highlight.Subscribe(x => Element.Focus());
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
            //whenever the textbox receives focus we signal that via an event to the outside world
            HighlightSource.RaiseFieldHighlighted(Element, new HighlightEventArgs(_id, _fieldName));
        }

        protected override void DoDetach()
        {
            Element.GotFocus -= OnGotFocus;

            _subscription.Dispose();

            _subscription = null;
            _fieldName = null;
            _id = Guid.Empty;
        }

        #endregion
    }
}