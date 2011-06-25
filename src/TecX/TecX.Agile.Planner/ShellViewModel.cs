using System;

using Microsoft.Practices.Prism.Commands;

using TecX.Agile.Infrastructure;
using TecX.Agile.Infrastructure.Events;
using TecX.Agile.Infrastructure.Services;
using TecX.Agile.Remote;
using TecX.Agile.ViewModel;
using TecX.Common;

namespace TecX.Agile.Planner
{
    //TODO weberse i want to hook up some info object that gives you current application version, deployment method (click once etc.),
    //product name, versions of all assemblies in current appdomain and the like
    public class ShellViewModel
    {
        #region Fields

        private readonly IMessageRelay messageRelay;
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
                _card = value;
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
                
                _currentProject = value;
            }
        }

        #endregion Properties

        #region c'tor

        public ShellViewModel(
            IMessageRelay messageRelay, 
            IShowThings showThingsService, 
            EventAggregatorAccessor eventAggregatorAccessor)
        {
            Guard.AssertNotNull(messageRelay, "messageRelay");
            Guard.AssertNotNull(showThingsService, "showThingsService");

            messageRelay = messageRelay;
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

            StoryCard card = new StoryCard(new NullEventAggregator()) { Id = args.StoryCardId, X = args.X, Y = args.Y, Angle = args.Angle };

            StoryCardCollection parent = CurrentProject.Find<StoryCardCollection>(args.To) ?? CurrentProject.Backlog;

            parent.Add(card);

            _showThingsService.Show(card);
        }

        #endregion CommandHandling
    }
}
