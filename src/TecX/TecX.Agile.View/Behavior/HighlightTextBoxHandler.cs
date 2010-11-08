using System.Windows;
using System.Windows.Controls;

using TecX.Agile.ViewModel;
using TecX.Common;

namespace TecX.Agile.View.Behavior
{
    public class HighlightTextBoxHandler : BehaviorHandler
    {
        private IHighlightable _highlightable;
        private string _name;

        #region Overrides of BehaviorHandler

        protected override void DoAttach(FrameworkElement element)
        {
            UserControl ctrl;
            string name;
            if (UIHelper.TryFindAncestor(element, out ctrl) &&
                UIHelper.TryGetName(ctrl, element, out name))
            {
                _highlightable = ctrl.DataContext as IHighlightable;

                if (_highlightable != null)
                {
                    _name = name;
                    _highlightable.Highlight += OnHighlight;

                    element.GotFocus += OnGotFocus;
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
            if (_highlightable != null &&
                !string.IsNullOrEmpty(_name))
            {
                _highlightable.NotifyFieldHighlighted(_name);
            }
        }

        protected override void DoDetach()
        {
            if (_highlightable != null)
            {
                _highlightable.Highlight += OnHighlight;
                Element.GotFocus -= OnGotFocus;
                _highlightable = null;
            }
        }

        #endregion

        private void OnHighlight(object sender, HighlightEventArgs e)
        {
            Guard.AssertNotNull(e, "e");
            Guard.AssertNotEmpty(e.FieldName, "e.ControlName");

            //if the name of the control that should be focused matches that of the control
            //we are currently hooked up to we put the focus on that control
            if (_name == e.FieldName)
            {
                Element.Focus();
            }
        }
    }
}