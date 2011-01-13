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

using TecX.Agile.Infrastructure.Services;
using TecX.Common;

namespace TecX.Agile.Modules.Main.View
{
    /// <summary>
    /// Interaction logic for InfoTextArea.xaml
    /// </summary>
    public partial class InfoTextArea : UserControl
    {
        private readonly IShowText _displayTextService;

        public InfoTextArea(IShowText displayTextService)
        {
            Guard.AssertNotNull(displayTextService, "displayTextService");

            _displayTextService = displayTextService;

            InitializeComponent();
        }
    }
}
