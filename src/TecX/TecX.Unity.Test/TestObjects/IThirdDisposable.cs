using TecX.Unity.BuildTracking;

using System;

namespace TecX.Unity.Test.TestObjects
{
    /// <summary>
    /// The <see cref="IThirdDisposable"/>
    ///   interface is used to test the <see cref="DisposableStrategyExtension"/> class.
    /// </summary>
    public interface IThirdDisposable : IDisposable
    {
        /// <summary>
        /// Gets or sets the fourth.
        /// </summary>
        /// <value>
        /// The fourth.
        /// </value>
        IFourthDisposable Fourth
        {
            get;
            set;
        }

        /// <summary>
        /// Gets a value indicating whether this instance is disposed.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is disposed; otherwise, <c>false</c>.
        /// </value>
        bool IsDisposed
        {
            get;
        }
    }
}