using System.Collections.Specialized;

using Microsoft.Practices.Prism.Regions;
using Microsoft.Windows.Controls.Ribbon;

namespace TecX.Prism.Regions
{
    public class RibbonRegionAdapter : RegionAdapterBase<Ribbon>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RibbonRegionAdapter"/> class
        /// </summary>
        public RibbonRegionAdapter(IRegionBehaviorFactory regionBehaviorFactory)
            : base(regionBehaviorFactory)
        {
        }

        protected override void Adapt(IRegion region, Ribbon ribbon)
        {
            region.ActiveViews.CollectionChanged += (sender, args) =>
                                                        {
                                                            switch (args.Action)
                                                            {
                                                                case NotifyCollectionChangedAction.Add:
                                                                    {
                                                                        foreach (RibbonTab ribbonTab in args.NewItems)
                                                                        {
                                                                            ribbon.Items.Add(ribbonTab);
                                                                        }
                                                                        break;
                                                                    }
                                                                case NotifyCollectionChangedAction.Remove:
                                                                    {
                                                                        foreach (RibbonTab ribbonTab in args.NewItems)
                                                                        {
                                                                            ribbon.Items.Remove(ribbonTab);
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