﻿namespace TecX.Agile.Modules.Gestures
{
    using System.Diagnostics;

    using TecX.Agile.Infrastructure;
    using TecX.Agile.Modules.Gestures.ViewModels;
    using TecX.CaliburnEx.Modularization;
    using TecX.Common;

    [DebuggerDisplay("{Description}")]
    public class GestureModule : Module
    {
        private readonly GestureViewModel gestureViewModel;

        private readonly IShell shell;

        public GestureModule(GestureViewModel gestureViewModel, IShell shell)
        {
            Guard.AssertNotNull(gestureViewModel, "gestureViewModel");
            Guard.AssertNotNull(shell, "shell");

            this.gestureViewModel = gestureViewModel;
            this.shell = shell;
        }

        public override string Description
        {
            get
            {
                return "Gesture Recognition";
            }
        }

        protected override void Initialize()
        {
            this.shell.AddOverlay(this.gestureViewModel);
        }
    }
}
