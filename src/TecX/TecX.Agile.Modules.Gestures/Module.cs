using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;

using TecX.Agile.Infrastructure;
using TecX.Agile.Modules.Gestures.View;
using TecX.Common;

namespace TecX.Agile.Modules.Gestures
{
    public class Module : IModule
    {
        #region Constants

        public const string ModuleName = "Gestures";

        #endregion Constants

        #region Fields

        private readonly IRegionManager _regionManager;
        private readonly GestureOverlay _gestureOverlay;

        #endregion Fields

        #region c'tor

        public Module(IRegionManager regionManager, GestureOverlay gestureOverlay)
        {
            Guard.AssertNotNull(regionManager, "regionManager");
            Guard.AssertNotNull(gestureOverlay, "gestureOverlay");

            _regionManager = regionManager;
            _gestureOverlay = gestureOverlay;
        }

        #endregion c'tor

        #region Implementation of IModule

        public void Initialize()
        {
            IRegion main = _regionManager.Regions[Regions.Main];

            main.Add(_gestureOverlay);
        }

        #endregion Implementation of IModule
    }
}
