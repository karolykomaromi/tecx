using System.Windows;

using TecX.Common;

namespace TecX.Agile.View.Behavior
{
    /// <summary>
    /// Base for per instance attached behavior that cannot be achieved
    /// with static classes
    /// </summary>
    public abstract class BehaviorHandler : IBehaviorHandler
    {
        #region Properties

        /// <summary>
        /// Gets or sets the element the handler is attached to
        /// </summary>
        /// <value>The item.</value>
        public FrameworkElement Element { get; private set; }

        #endregion Properties

        #region Implementation of IBehaviorHandler

        /// <summary>
        /// Attaches the handler to a given element
        /// </summary>
        /// <param name="element">The element to attach to</param>
        public void Attach(FrameworkElement element)
        {
            Guard.AssertNotNull(element, "element");

            Element = element;

            DoAttach(element);
        }

        /// <summary>
        /// Performs the attachment of this handler
        /// </summary>
        /// <param name="element">The element to attach to</param>
        protected abstract void DoAttach(FrameworkElement element);

        /// <summary>
        /// Detaches this handler from the element
        /// </summary>
        public void Detach()
        {
            DoDetach();

            Element = null;
        }

        /// <summary>
        /// Performs the detachment of this handler
        /// </summary>
        protected abstract void DoDetach();

        #endregion Implementation of IBehaviorHandler
    }
}