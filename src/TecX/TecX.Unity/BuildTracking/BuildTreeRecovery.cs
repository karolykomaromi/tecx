using System;
using System.Diagnostics;

using Microsoft.Practices.ObjectBuilder2;

using TecX.Common;

namespace TecX.Unity.BuildTracking
{
    /// <summary>
    /// The <see cref="BuildTreeRecovery"/>
    ///   class is used to repair the build tree when a build failure occurs.
    /// </summary>
    internal class BuildTreeRecovery : IRequiresRecovery
    {
        private readonly IBuilderContext _context;

        private readonly BuildTreeItemNode _failedNode;

        private readonly Action<BuildTreeItemNode> _failureAction;

        /// <summary>
        /// Initializes a new instance of the <see cref="BuildTreeRecovery"/> class.
        /// </summary>
        /// <param name="context">
        /// The context.
        /// </param>
        /// <param name="failedNode">
        /// The current node.
        /// </param>
        /// <param name="failureAction">
        /// The failure action.
        /// </param>
        public BuildTreeRecovery(IBuilderContext context, BuildTreeItemNode failedNode,
                                 Action<BuildTreeItemNode> failureAction)
        {
            Guard.AssertNotNull(context, "context");
            Guard.AssertNotNull(failedNode, "failedNode");

            _context = context;
            _failedNode = failedNode;
            _failureAction = failureAction;
        }

        /// <summary>
        /// A method that does whatever is needed to clean up
        ///   as part of cleaning up after an exception.
        /// </summary>
        /// <remarks>
        /// Don't do anything that could throw in this method,
        ///   it will cause later recovery operations to get skipped
        ///   and play real havoc with the stack trace.
        /// </remarks>
        public void Recover()
        {
            try
            {
                if (_failureAction != null)
                {
                    _failureAction(_failedNode);
                }

                BuildTreeItemNode parentNode = _failedNode.Parent;

                BuildTreeDisposer.DisposeTree(_context, _failedNode);

                if (parentNode != null)
                {
                    parentNode.Children.Remove(_failedNode);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Recovery failed: " + ex);
            }
        }
    }
}