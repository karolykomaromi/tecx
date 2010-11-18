using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TecX.Agile.ViewModel.Remote;
using TecX.Common;

namespace TecX.Agile.ViewModel
{
    public class ShellViewModel
    {
        private readonly IRemoteUI _remoteUI;

        public ShellViewModel(IRemoteUI remoteUI)
        {
            Guard.AssertNotNull(remoteUI, "remoteUI");

            _remoteUI = remoteUI;
        }

        //TODO weberse i want to hook up some info object that gives you current application version, deployment method (click once etc.),
        //product name, versions of all assemblies in current appdomain and the like
    }
}
