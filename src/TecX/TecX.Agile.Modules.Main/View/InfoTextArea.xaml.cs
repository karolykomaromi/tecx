using System.Windows.Controls;

using TecX.Agile.Infrastructure.Services;
using TecX.Common;

namespace TecX.Agile.Modules.Main.View
{
    /// <summary>
    /// Interaction logic for InfoTextArea.xaml
    /// </summary>
    public partial class InfoTextArea : UserControl
    {
        private readonly IShowText _showTextService;

        public InfoTextArea(IShowText showTextService)
        {
            Guard.AssertNotNull(showTextService, "showTextService");

            _showTextService = showTextService;

            DataContext = showTextService;

            InitializeComponent();
        }
    }
}
