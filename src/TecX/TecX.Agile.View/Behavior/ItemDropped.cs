using System.Windows;

using TecX.Common;

namespace TecX.Agile.View.Behavior
{
    internal class ItemDropped
    {
        private readonly FrameworkElement _element;
        private readonly Point _dropPoint;

        public Point DropPoint
        {
            get { return _dropPoint; }
        }

        public FrameworkElement Element
        {
            get { return _element; }
        }

        public ItemDropped(FrameworkElement element, Point dropPoint)
        {
            Guard.AssertNotNull(element, "element");

            _element = element;
            _dropPoint = dropPoint;
        }
    }
}