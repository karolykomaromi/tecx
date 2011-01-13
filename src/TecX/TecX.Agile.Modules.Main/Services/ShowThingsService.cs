using System;

using TecX.Agile.Infrastructure.Services;
using TecX.Agile.View;

namespace TecX.Agile.Modules.Main.Services
{
    public class ShowThingsService : IShowThings
    {
        public void Show(object thing)
        {
            ViewModel.StoryCard storyCard = thing as ViewModel.StoryCard;

            if (storyCard == null)
                throw new NotSupportedException("Right now 'thing' means 'ViewModel.StoryCard'. " +
                                                "Everything else will follow");

            StoryCard view = new StoryCard(storyCard);

            Tabletop.Surface.Children.Add(view);
        }
    }
}