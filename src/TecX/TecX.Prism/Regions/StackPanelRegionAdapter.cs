using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;

using Microsoft.Practices.Prism.Regions;

namespace TecX.Prism.Regions
{
    public class StackPanelRegionAdapter : RegionAdapterBase<StackPanel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StackPanelRegionAdapter"/> class
        /// </summary>
        public StackPanelRegionAdapter(IRegionBehaviorFactory regionBehaviorFactory)
            : base(regionBehaviorFactory)
        {
        }

        protected override void Adapt(IRegion region, StackPanel stackPanel)
        {
            region.ActiveViews.CollectionChanged += (sender, args) =>
                                                        {
                                                            switch (args.Action)
                                                            {
                                                                case NotifyCollectionChangedAction.Add:
                                                                    {
                                                                        foreach (UIElement element in args.NewItems)
                                                                        {
                                                                            stackPanel.Children.Add(element);
                                                                        }
                                                                        break;
                                                                    }
                                                                case NotifyCollectionChangedAction.Remove:
                                                                    {
                                                                        foreach (UIElement element in args.NewItems)
                                                                        {
                                                                            stackPanel.Children.Remove(element);
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