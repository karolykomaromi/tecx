using System;
using System.Reflection;
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
        private string _systemInfo;
        private readonly IShowText _showTextService;
        private readonly DelegateCommand _showSystemInfoCommand;

        public Module(IShowText showTextService)
        {
            Guard.AssertNotNull(showTextService, "showTextService");

            _showTextService = showTextService;

            _showSystemInfoCommand = new DelegateCommand(OnShowSystemInfo);

            Commands.ShowSystemInfo.RegisterCommand(_showSystemInfoCommand);
        }

        public void Initialize()
        {
            StringBuilder sb = new StringBuilder(1024);

            sb.AppendLine("         WPF         ")
              .AppendLine("---------------------");

            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            
            foreach(var assembly in assemblies)
            {
                sb.Append(assembly.FullName);
            }

            _systemInfo = sb.ToString();
        }

        private void OnShowSystemInfo()
        {
            _showTextService.Show(_systemInfo);
        }
    }
}
