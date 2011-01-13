using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using TecX.Agile.ViewModel;
using TecX.Common;

namespace TecX.Agile.View
{
    /// <summary>
    /// Interaction logic for StoryCard.xaml
    /// </summary>
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
