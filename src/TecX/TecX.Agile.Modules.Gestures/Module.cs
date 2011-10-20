using System.Diagnostics;

using TecX.Agile.Infrastructure;
using TecX.Agile.Modules.Gestures.ViewModels;
using TecX.Common;

namespace TecX.Agile.Modules.Gestures
{
    [DebuggerDisplay("{Description}")]
    public class Module : ModuleBase
    {
        private readonly GestureViewModel _gestureViewModel;

        private readonly IShell _shell;

        public override string Description
        {
            get
            {
                return "Gesture Recognition";
            }
        }

        public Module(GestureViewModel gestureViewModel, IShell shell)
        {
            Guard.AssertNotNull(gestureViewModel, "gestureViewModel");
            Guard.AssertNotNull(shell, "shell");

            _gestureViewModel = gestureViewModel;
            _shell = shell;
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();

            _shell.AddOverlay(_gestureViewModel);
        }
    }
}
