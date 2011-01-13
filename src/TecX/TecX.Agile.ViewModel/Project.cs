using System;
using System.Collections.Generic;

using Microsoft.Practices.Prism.Commands;

using TecX.Agile.Infrastructure;
using TecX.Agile.Infrastructure.Events;
using TecX.Agile.Infrastructure.Services;
using TecX.Common;

namespace TecX.Agile.ViewModel
{
    public class Project : IterationCollection, IEnumerable<PlanningArtefact>
    {
        private readonly IShowThings _showThingsService;

        #region Fields

        private readonly Backlog _backlog;

        private readonly DelegateCommand<StoryCardAdded> _addStoryCardCommand;

        #endregion Fields

        #region Properties

        public Backlog Backlog
        {
            get { return _backlog; }
        }

        #endregion Properties

        #region c'tor

        /// <summary>
        /// Initializes a new instance of the <see cref="Project"/> class
        /// </summary>
        public Project(IShowThings showThingsService)
        {
            Guard.AssertNotNull(showThingsService, "showThingsService");

            _showThingsService = showThingsService;

            _backlog = new Backlog { Id = Guid.NewGuid() };

            _addStoryCardCommand = new DelegateCommand<StoryCardAdded>(OnAddStoryCard);

            Commands.AddStoryCard.RegisterCommand(_addStoryCardCommand);
        }

        #endregion c'tor

        #region Overrides of PlanningArtefactCollection<Iteration>

        protected internal override void AddCore(Iteration item)
        {
            item.Project = this;
        }

        protected override void RemoveCore(Iteration item)
        {
            item.Project = null;
        }

        #endregion Overrides of PlanningArtefactCollection<Iteration>

        #region Methods

        public TArtefact Find<TArtefact>(Guid id)
            where TArtefact : PlanningArtefact
        {
            if (Id == id)
                return this as TArtefact;

            if (Backlog.Id == id)
                return Backlog as TArtefact;

            foreach (Iteration iteration in this)
            {
                if (iteration.Id == id)
                    return iteration as TArtefact;

                foreach (StoryCard storyCard in iteration)
                {
                    if (storyCard.Id == id)
                        return storyCard as TArtefact;
                }
            }

            return null;
        }

        private void OnAddStoryCard(StoryCardAdded args)
        {
            Guard.AssertNotNull(args, "args");

            StoryCard card = new StoryCard { Id = args.StoryCardId, X = args.X, Y = args.Y, Angle = args.Angle };

            StoryCardCollection parent = Find<StoryCardCollection>(args.To) ?? _backlog;

            parent.Add(card);

            _showThingsService.Show(card);
        }

        #endregion Methods

        #region Implementation of IEnumerable<out PlanningArtefact>

        IEnumerator<PlanningArtefact> IEnumerable<PlanningArtefact>.GetEnumerator()
        {
            yield return this;

            yield return _backlog;

            foreach (StoryCard storyCard in Backlog)
            {
                yield return storyCard;
            }

            foreach (Iteration iteration in Project)
            {
                yield return iteration;

                foreach (StoryCard storyCard in iteration)
                {
                    yield return storyCard;
                }
            }
        }

        #endregion Implementation of IEnumerable<out PlanningArtefact>
    }
}
