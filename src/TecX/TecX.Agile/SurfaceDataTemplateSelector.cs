using System;
using System.Windows;
using System.Windows.Controls;

using TecX.Agile.ViewModels;
using TecX.Common;

namespace TecX.Agile
{
    public class SurfaceDataTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            Guard.AssertNotNull(item, "item");
            Guard.AssertNotNull(container, "container");

            Type itemType = item.GetType();

            FrameworkElement element = container as FrameworkElement;

            if (element == null) return null;

            if (itemType == typeof(StoryCardViewModel))
            {
                object template = element.TryFindResource(Constants.Templates.StoryCard);

                return template as DataTemplate;
            }

            if(itemType == typeof(IterationViewModel))
            {
                object template = element.TryFindResource(Constants.Templates.Iteration);

                return template as DataTemplate;
            }

            return base.SelectTemplate(item, container);
        }
    }
}
