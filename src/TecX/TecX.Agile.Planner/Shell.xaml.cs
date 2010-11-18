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
using System.Windows.Shapes;

using Microsoft.Windows.Controls.Ribbon;

using TecX.Agile.ViewModel;
using TecX.Common;

namespace TecX.Agile.Planner
{
    /// <summary>
    /// Interaction logic for Shell.xaml
    /// </summary>
    public partial class Shell : RibbonWindow
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
        }
    }
}
