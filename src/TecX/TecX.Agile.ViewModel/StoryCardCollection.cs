using System;
using System.Collections.Generic;

using TecX.Agile.Infrastructure.Events;
using TecX.Common;
using TecX.Common.Extensions.Error;

namespace TecX.Agile.ViewModel
{
    public class StoryCardCollection : PlanningArtefactCollection<StoryCard>
    {
        #region Overrides of PlanningArtefactCollection<StoryCard>

        protected internal override void AddCore(StoryCard item)
        {
            item.Parent = this;
        }

        protected override void RemoveCore(StoryCard item)
        {
            item.Parent = null;
        }

        #endregion Overrides of PlanningArtefactCollection<StoryCard>

        #region Methods

        public void Reschedule(StoryCard storyCard, StoryCardCollection otherCollection)
        {
            Guard.AssertNotNull(storyCard, "storyCard");
            Guard.AssertNotNull(otherCollection, "otherCollection");

            Backlog backlog = otherCollection as Backlog;
            if (backlog != null)
            {
                throw new InvalidOperationException("If you want to move the storycard to the Project Backlog use Postpone() instead!")
                    .WithAdditionalInfos(
                        new Dictionary<object, object>
                            {
                                {"storyCard", storyCard}, 
                                {"otherCollection", otherCollection}
                            });
            }

            if (!Contains(storyCard))
            {
                throw new InvalidOperationException("StoryCard is not inside this collection")
                    .WithAdditionalInfos(
                        new Dictionary<object, object>
                            {
                                {"storyCard", storyCard},
                                {"otherCollection", otherCollection},
                                {"this", this}
                            }
                    );
            }

            Artefacts.Remove(storyCard.Id);
            RemoveCore(storyCard);

            otherCollection.Artefacts.Add(storyCard.Id, storyCard);
            otherCollection.AddCore(storyCard);

            EventAggregator.Publish(new StoryCardRescheduled(storyCard.Id, Id, otherCollection.Id));
        }

        #endregion Methods
    }
}