namespace Hydra.Unity.Tracking
{
    using System;
    using System.Diagnostics;

    public class BuildTreeDisposer : ITreeItemVisitor
    {
        public void Visit(DisposableItemNode node)
        {
            this.DisposeTree(node);

            foreach (BuildTreeItemNode child in node.Children)
            {
                child.Accept(this);
            }
        }

        public void Visit(NonDisposableItemNode node)
        {
            foreach (BuildTreeItemNode child in node.Children)
            {
                child.Accept(this);
            }
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