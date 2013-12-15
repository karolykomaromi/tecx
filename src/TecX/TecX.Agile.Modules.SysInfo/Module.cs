using System;
using System.Linq;
using System.Text;

using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Modularity;

using TecX.Agile.Infrastructure;
using TecX.Agile.Infrastructure.Services;
using TecX.Common;

namespace TecX.Agile.Modules.SysInfo
{
    public class Module : IModule
    {
        #region Constants

        private static class Constants
        {
            /// <summary>SystemInfo</summary>
            public const string ModuleName = "SystemInfo";
            
            /// <summary>10</summary>
            public const int MaxNumAssemblies = 10;
        }

        #endregion Constants

        #region Fields

        private string _systemInfo;
        private readonly IShowText _showTextService;
        private readonly DelegateCommand _showSystemInfoCommand;

        #endregion Fields

        #region Properties

        public static string ModuleName
        {
            get { return Constants.ModuleName; }
        }

        #endregion Properties

        #region c'tor

        public Module(IShowText showTextService)
        {
            Guard.AssertNotNull(showTextService, "showTextService");

            _showTextService = showTextService;

            _showSystemInfoCommand = new DelegateCommand(OnShowSystemInfo);

            Commands.ShowSystemInfo.RegisterCommand(_showSystemInfoCommand);
        }

        #endregion c'tor

        public void Initialize()
        {

            StringBuilder sb = new StringBuilder(1024);

            sb.AppendLine("WPF").AppendLine();

            var assemblies = AppDomain.CurrentDomain.GetAssemblies().OrderBy(a => a.FullName).ToArray();

            for (int i = 0; i < Constants.MaxNumAssemblies; i++)
            {
                sb.AppendLine(assemblies[i].FullName);
            }

            if (assemblies.Length > Constants.MaxNumAssemblies)
                sb.AppendLine("...");

                _systemInfo = sb.ToString();
        }

        private void OnShowSystemInfo()
        {
            _showTextService.Show(_systemInfo);
        }
    }
}
