﻿using TecX.Unity.BuildTracking;

using System;

namespace TecX.Unity.Test.TestObjects
{
    /// <summary>
    /// The <see cref="ISecondDisposable"/>
    ///   interface is used to test the <see cref="DisposableStrategyExtension"/> class.
    /// </summary>
    public interface ISecondDisposable : IDisposable
    {
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

        /// <summary>
        /// Gets or sets the third.
        /// </summary>
        /// <value>
        /// The third.
        /// </value>
        IThirdDisposable Third
        {
            get;
            set;
        }
    }
}