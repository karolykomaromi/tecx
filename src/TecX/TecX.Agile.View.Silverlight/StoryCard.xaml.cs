using System.Windows.Controls;

using TecX.Common;

namespace TecX.Agile.View
{
    public partial class StoryCard : UserControl
    {
        #region c'tor

        public StoryCard()
            : this(new ViewModel.StoryCard())
        {
        }

        public StoryCard(ViewModel.StoryCard storyCard)
        {
            Guard.AssertNotNull(storyCard, "storyCard");

            DataContext = storyCard;

            InitializeComponent();

        }

        #endregion c'tor
    }
}
