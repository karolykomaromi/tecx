using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TecX.Common;

namespace TecX.Agile.ViewModel.Messages
{
    public class StoryCardAdded : IDomainEvent
    {
        private readonly StoryCard _storyCard;
        private readonly StoryCardCollection _to;

        public StoryCardCollection To
        {
            get { return _to; }
        }

        public StoryCard StoryCard
        {
            get { return _storyCard; }
        }

        public StoryCardAdded(StoryCard storyCard, StoryCardCollection to)
        {
            Guard.AssertNotNull(storyCard, "storyCard");
            Guard.AssertNotNull(to, "to");

            _storyCard = storyCard;
            _to = to;
        }
    }
}
