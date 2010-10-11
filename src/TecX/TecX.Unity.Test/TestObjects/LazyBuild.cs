using System;

using Microsoft.Practices.Unity;

namespace TecX.Unity.Test.TestObjects
{
    /// <summary>
    /// The <see cref="LazyBuild"/>
    ///   class is used to test build tree tracking for lazy unity injection.
    /// </summary>
    public class LazyBuild
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LazyBuild"/> class.
        /// </summary>
        /// <param name="third">
        /// The third.
        /// </param>
        public LazyBuild(Func<IThirdDisposable> third)
        {
            Third = third;
        }

        /// <summary>
        /// Gets or sets the forth.
        /// </summary>
        /// <value>
        /// The forth.
        /// </value>
        [Dependency]
        public Func<IFourthDisposable> Forth
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the third.
        /// </summary>
        /// <value>
        /// The third.
        /// </value>
        public Func<IThirdDisposable> Third
        {
            get;
            private set;
        }
    }
}