using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

using TecX.Common;

namespace TecX.Agile.Planner
{
    public partial class Shell : UserControl
    {
        #region Fields

        private readonly ShellViewModel _viewModel;

        #endregion Fields

        #region c'tor

        public Shell()
        {
            InitializeComponent();

            Loaded += OnLoaded;
        }

        //public Shell(ShellViewModel viewModel)
        //    : this()
        //{
        //    Guard.AssertNotNull(viewModel, "viewModel");

        //    DataContext = _viewModel = viewModel;
        //}

        #endregion c'tor

        #region EventHandling

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (DesignerProperties.GetIsInDesignMode(this))
                return;

            //_viewModel.InitializeSocketConnection();
        }

        #endregion EventHandling
    }
}
