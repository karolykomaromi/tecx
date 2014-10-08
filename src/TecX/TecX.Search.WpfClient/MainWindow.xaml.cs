namespace TecX.Search.WpfClient
{
    using System.Windows;

    using TecX.Common;
    using TecX.Search.WpfClient.ViewModel;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public MainWindow(MainWindowViewModel viewModel)
        {
            Guard.AssertNotNull(viewModel, "viewModel");

            this.DataContext = viewModel;

            this.InitializeComponent();
        }
    }
}
