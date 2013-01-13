namespace TecX.Agile.Utilities
{
    using System;
    using System.Windows;
    using System.Windows.Controls;

    using TecX.Agile.Infrastructure;
    using TecX.Agile.ViewModels;
    using TecX.Common;

    public class SurfaceDataTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            Guard.AssertNotNull(item, "item");
            Guard.AssertNotNull(container, "container");

            Type itemType = item.GetType();

            FrameworkElement element = container as FrameworkElement;

            if (element == null)
            {
                return null;
            }

            if (itemType == typeof(StoryCardViewModel))
            {
                object template = element.TryFindResource(Constants.DataTemplates.StoryCard);

                return template as DataTemplate;
            }

            if (itemType == typeof(IterationViewModel))
            {
                object template = element.TryFindResource(Constants.DataTemplates.Iteration);

                return template as DataTemplate;
            }

            return base.SelectTemplate(item, container);
        }
    }
}
