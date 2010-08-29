using System.Windows;

namespace TecX.Agile.View.Behavior
{
    /// <summary>
    /// Interface for per-instance attached behavior that cannot be achieved
    /// with static classes
    /// </summary>
    public interface IBehaviorHandler
    {
        /// <summary>
        /// Attaches the handler to a given element
        /// </summary>
        /// <param name="element">The element to attach to</param>
        void Attach(FrameworkElement element);

        /// <summary>
        /// Detaches this handler from the element
        /// </summary>
        void Detach();
    }
}
