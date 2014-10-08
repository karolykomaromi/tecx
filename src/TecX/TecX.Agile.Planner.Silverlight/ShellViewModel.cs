using System;

using Microsoft.Practices.Prism.Commands;

using TecX.Agile.ChangeTracking;
using TecX.Agile.Infrastructure;
using TecX.Agile.Infrastructure.Events;
using TecX.Agile.Infrastructure.Services;
using TecX.Agile.Remote;
using TecX.Agile.ViewModel;
using TecX.Common;

namespace TecX.Agile.Planner
{
    public class ShellViewModel
    {
        #region Fields

        private IRemoteUI _remoteUI;
        private readonly Func<IRemoteUI> _remoteUIFactory;
        private readonly IChangeTracker _changeTracker;
        private readonly IShowThings _showThingsService;

        private readonly DelegateCommand<StoryCardAdded> _addStoryCardCommand;

        private Project _currentProject;
        private StoryCard _card;

        #endregion Fields

        #region Properties

        public StoryCard Card
        {
            get { return _card; }
            set
            {
                if (_card != null)
                    _changeTracker.Unsubscribe(_card);

                _card = value;

                _changeTracker.Subscribe(_card);
            }
        }

        public Project CurrentProject
        {
            get { return _currentProject; }
            set
            {
                Guard.AssertNotNull(value, "value");

                if (_currentProject == value)
                    return;

                if (_currentProject != null)
                    _changeTracker.Unsubscribe(_currentProject);

                _currentProject = value;

                _changeTracker.Subscribe(_currentProject);
            }
        }

        #endregion Properties

        #region c'tor

        public ShellViewModel(
            Func<IRemoteUI> remoteUIFactory,
            IChangeTracker changeTracker,
            IShowThings showThingsService,
            EventAggregatorAccessor eventAggregatorAccessor)
        {
            Guard.AssertNotNull(remoteUIFactory, "remoteUIFactory");
            Guard.AssertNotNull(changeTracker, "changeTracker");
            Guard.AssertNotNull(showThingsService, "showThingsService");

            _remoteUIFactory = remoteUIFactory;
            _changeTracker = changeTracker;
            _showThingsService = showThingsService;

            //TODO weberse initialization of current project must be moved somewhere else
            CurrentProject = new Project { Id = Guid.NewGuid() };

            _addStoryCardCommand = new DelegateCommand<StoryCardAdded>(OnAddStoryCard);

            Commands.AddStoryCard.RegisterCommand(_addStoryCardCommand);
        }

        #endregion c'tor

        #region CommandHandling

        private void OnAddStoryCard(StoryCardAdded args)
        {
            Guard.AssertNotNull(args, "args");

            StoryCard card = new StoryCard { Id = args.StoryCardId, X = args.X, Y = args.Y, Angle = args.Angle };

            StoryCardCollection parent = CurrentProject.Find<StoryCardCollection>(args.To) ?? CurrentProject.Backlog;

            parent.Add(card);

            _showThingsService.Show(card);
        }

        #endregion CommandHandling

        public void InitializeConnection()
        {
            _remoteUI = _remoteUIFactory();
        }
    }
}
