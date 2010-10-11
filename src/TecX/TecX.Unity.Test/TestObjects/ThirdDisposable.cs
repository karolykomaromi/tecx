using TecX.Unity.BuildTracking;

using Microsoft.Practices.Unity;

namespace TecX.Unity.Test.TestObjects
{
    /// <summary>
    /// The <see cref="ThirdDisposable"/>
    ///   class is used to test the <see cref="DisposableStrategyExtension"/> class.
    /// </summary>
    public class ThirdDisposable : DisposableTester, IThirdDisposable
    {
        /// <summary>
        /// Gets or sets the fourth.
        /// </summary>
        /// <value>
        /// The fourth.
        /// </value>
        [Dependency]
        public IFourthDisposable Fourth
        {
            get;
            set;
        }
    }
}