using System.Windows;

namespace TecX.Agile.Views
{
    using TecX.Agile.ViewModels;

    /// <summary>
    /// Interaction logic for ShellView.xaml
    /// </summary>
    public partial class ShellView : Window
    {
        public ShellView()
        {
            InitializeComponent();
        }

        private int counter = 0;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (counter % 2 == 0)
            {
                ((ShellViewModel)DataContext).Items.Add(new StoryCardViewModel());
            }
            else
            {
                ((ShellViewModel)DataContext).Items.Add(new IterationViewModel());
            }

            counter++;
        }
    }
}
