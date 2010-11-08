using System;

using TecX.Common;
using TecX.Common.Extensions.Error;

namespace TecX.Agile.ViewModel
{
    public class Iteration : StoryCardCollection, IHighlightable
    {
        public event EventHandler<StoryCardPostponedEventArgs> StoryCardPostponed;

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

                StoryCardPostponedEventArgs args = new StoryCardPostponedEventArgs(storyCard, this);

                OnStoryCardPostponed(args);
            }
            else
            {
                throw new InvalidOperationException("Storycard is not part of this iteration and thus cannot be postponed")
                    .WithAdditionalInfo("storyCard", storyCard);
            }
        }

        private void OnStoryCardPostponed(StoryCardPostponedEventArgs args)
        {
            Guard.AssertNotNull(args, "args");

            if(StoryCardPostponed != null)
            {
                StoryCardPostponed(this, args);
            }
        }


        #region Implementation of IHighlightable

        public event EventHandler<HighlightEventArgs> Highlight = delegate { };

        public void NotifyFieldHighlighted(string fieldName)
        {
            //TODO weberse this will be the place where we notify remote clients that 
            //some field on the ui was highlighted
            if (Project != null)
            {
                Project.NotifyFieldHighlighted(Id, fieldName);
            }
        }

        public void HighlightField(string fieldName)
        {
            Guard.AssertNotEmpty(fieldName, "controlName");

            Highlight(this, new HighlightEventArgs(fieldName));
        }

        #endregion Implementation of IHighlightable

    }
}