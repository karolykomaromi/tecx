using System.Windows;
using System.Windows.Input;

namespace TecX.Agile.View.Behavior
{
    internal class PreviewFocusHandler : BehaviorHandler
    {
        #region c'tor

        public PreviewFocusHandler()
        {
        }

        #endregion c'tor

        #region Overrides of BehaviorHandler

        /// <summary>
        /// Performs the attachment of this handler
        /// </summary>
        /// <param name="element">The element to attach to</param>
        protected override void DoAttach(FrameworkElement element)
        {
            Element.PreviewMouseLeftButtonDown += OnPreviewMouseLeftButtonDown;
        }

        /// <summary>
        /// Performs the detachment of this handler
        /// </summary>
        protected override void DoDetach()
        {
            Element.PreviewMouseLeftButtonDown -= OnPreviewMouseLeftButtonDown;
        }

        #endregion

        /// <summary>
        /// Called when [preview mouse left button down].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.Input.MouseButtonEventArgs"/> instance containing the event data.</param>
        private static void OnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            UIElement element = sender as UIElement;

            if (element != null)
                element.Focus();
        }
    }
}