namespace Infrastructure
{
    using System.Collections.Specialized;
    using System.Windows;
    using System.Windows.Controls;
    using Microsoft.Practices.Prism.Regions;

    public class StackPanelRegionAdapter : RegionAdapterBase<StackPanel>
    {
        public StackPanelRegionAdapter(IRegionBehaviorFactory regionBehaviorFactory)
            : base(regionBehaviorFactory)
        {
        }

        protected override void Adapt(IRegion region, StackPanel regionTarget)
        {
            region.Views.CollectionChanged += (s, e) =>
            {
                switch (e.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                        foreach (FrameworkElement element in e.NewItems)
                        {
                            regionTarget.Children.Add(element);
                        }

                        break;
                    case NotifyCollectionChangedAction.Remove:
                        foreach (FrameworkElement element in e.OldItems)
                        {
                            regionTarget.Children.Remove(element);
                        }

                        break;
                }
            };
        }

        protected override IRegion CreateRegion()
        {
            return new AllActiveRegion();
        }
    }
}
