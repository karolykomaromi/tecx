using System;

using TecX.Agile.Infrastructure.Events;
using TecX.Common;
using TecX.Common.Extensions.Error;

namespace TecX.Agile.ViewModel
{
    [Serializable]
    public class Iteration : StoryCardCollection
    {
        public void Postpone(StoryCard storyCard)
        {
            Guard.AssertNotNull(storyCard, "storyCard");
            Guard.AssertNotNull(Project, "Project", "Iteration must be part of a Project to be able to postpone work on a storycard " +
                                                    "and put it to the project's Backlog");
            Guard.AssertNotNull(Project.Backlog, "Project.Backlog", "A Backlog must be assigned to the Project to be able to postpone work on a storycard");

            if (Contains(storyCard))
            {
                Artefacts.Remove(storyCard.Id);
                RemoveCore(storyCard);

                Project.Backlog.Artefacts.Add(storyCard.Id, storyCard);
                Project.Backlog.AddCore(storyCard);

                EventAggregator.Publish(new StoryCardPostponed(storyCard.Id, Id));
            }
            else
            {
                throw new InvalidOperationException("Storycard is not part of this iteration and thus cannot be postponed")
                    .WithAdditionalInfo("storyCard", storyCard);
            }
        }
    }
}