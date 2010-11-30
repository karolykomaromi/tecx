using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;

using Microsoft.Practices.Prism.Regions;

namespace TecX.Prism.Regions
{
    public class GridRegionAdapter : RegionAdapterBase<Grid>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RibbonRegionAdapter"/> class
        /// </summary>
        public GridRegionAdapter(IRegionBehaviorFactory regionBehaviorFactory)
            : base(regionBehaviorFactory)
        {
        }

        protected override void Adapt(IRegion region, Grid grid)
        {
            region.ActiveViews.CollectionChanged += (sender, args) =>
                                                        {
                                                            switch (args.Action)
                                                            {
                                                                case NotifyCollectionChangedAction.Add:
                                                                    {
                                                                        foreach (UIElement element in args.NewItems)
                                                                        {
                                                                            grid.Children.Add(element);
                                                                        }
                                                                        break;
                                                                    }
                                                                case NotifyCollectionChangedAction.Remove:
                                                                    {
                                                                        foreach (UIElement element in args.NewItems)
                                                                        {
                                                                            grid.Children.Remove(element);
                                                                        }
                                                                        break;
                                                                    }
                                                            }
                                                        };
        }

        protected override IRegion CreateRegion()
        {
            return new AllActiveRegion();
        }
    }
}
