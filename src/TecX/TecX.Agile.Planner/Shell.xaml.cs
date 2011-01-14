using System.Windows;

using TecX.Agile.Infrastructure;
using TecX.Agile.ViewModel;
using TecX.Common;

namespace TecX.Agile.Planner
{
    /// <summary>
    /// Interaction logic for Shell.xaml
    /// </summary>
    public partial class Shell : Window
    {
        private readonly ShellViewModel _shellViewModel;

        public Shell()
        {
            InitializeComponent();

            // Insert code required on object creation below this point.
        }

        public Shell(ShellViewModel shellViewModel)
            : this()
        {
            Guard.AssertNotNull(shellViewModel, "shellViewModel");

            _shellViewModel = shellViewModel;

            DataContext = shellViewModel;

            shellViewModel.Card = storyCard.DataContext as StoryCard;
        }
    }
}
