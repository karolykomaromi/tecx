namespace Hydra.Unity.Tracking
{
    using System;
    using System.Diagnostics;

    public class BuildTreeDisposer : ITreeItemVisitor
    {
        public void Visit(DisposableItemNode node)
        {
            // clean up the current object, then proceed to its children
            this.DisposeTree(node);

            foreach (BuildTreeItemNode child in node.Children)
            {
                child.Accept(this);
            }
        }

        public void Visit(NonDisposableItemNode node)
        {
            // can't dispose the current object but maybe we need to deal
            // with its children
            foreach (BuildTreeItemNode child in node.Children)
            {
                child.Accept(this);
            }
        }

        public void Visit(SomeoneElsesProblem node)
        {
            // noop
        }

        private void DisposeTree(BuildTreeItemNode treeNode)
        {
            try
            {
                treeNode.Dispose();
            }
            catch (ObjectDisposedException)
            {
                Debug.WriteLine("Object was already disposed");
            }
        }
    }
}