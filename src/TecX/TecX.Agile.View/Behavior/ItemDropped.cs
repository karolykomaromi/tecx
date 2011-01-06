using System.Windows;

using TecX.Common;

namespace TecX.Agile.View.Behavior
{
    //TODO weberse 2011-01-06 must hook up a listener that finds all usercontrols that are beneath the drop point,
    //extracts the planningartefacts from those controls and raises an event that can be handled by the view model
    //sth. like "storycarddroppedontopof"
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