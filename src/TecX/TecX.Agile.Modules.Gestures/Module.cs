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

        public override string Description
        {
            get
            {
                return "Gesture Recognition";
            }
        }

        public Module(GestureViewModel gestureViewModel)
        {
            Guard.AssertNotNull(gestureViewModel, "gestureViewModel");

            _gestureViewModel = gestureViewModel;
        }
    }
}
