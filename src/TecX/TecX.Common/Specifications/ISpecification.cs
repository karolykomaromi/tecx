using System;

namespace TecX.Common.Specifications
{
    /// <summary>
    /// Part of the <c>Specification Pattern</c>
    /// </summary>
    /// <remarks>
    /// See http://en.wikipedia.org/wiki/Specification_pattern for further information on the pattern
    /// </remarks>
    /// <typeparam name="TCandidate">The type of the candidate objects the specification should
    /// work on</typeparam>
    public interface ISpecification<TCandidate>
    {
        #region Evaluation Method

        /// <summary>
        /// Determines whether a candidate object satifies the specification
        /// </summary>
        /// <param name="candidate">The candidate.</param>
        /// <returns>
        /// 	<c>true</c> if the specification is satisfied by the 
        /// candidate object; otherwise, <c>false</c>.
        /// </returns>
        bool IsSatisfiedBy(TCandidate candidate);

        #endregion Evaluation Method

        #region And

        /// <summary>
        /// Links two specifications using locical AND
        /// </summary>
        /// <typeparam name="TSpecification">The type of the specification.</typeparam>
        /// <param name="value">The value to compare to.</param>
        /// <returns>The linked specifications</returns>
        ISpecification<TCandidate> And<TSpecification>(byte value)
            where TSpecification : CompareToValueSpecification<TCandidate, byte>, new();

        /// <summary>
        /// Links two specifications using locical AND
        /// </summary>
        /// <typeparam name="TSpecification">The type of the specification.</typeparam>
        /// <param name="value">The value to compare to.</param>
        /// <returns>The linked specifications</returns>
        ISpecification<TCandidate> And<TSpecification>(sbyte value)
            where TSpecification : CompareToValueSpecification<TCandidate, sbyte>, new();

        /// <summary>
        /// Links two specifications using locical AND
        /// </summary>
        /// <typeparam name="TSpecification">The type of the specification.</typeparam>
        /// <param name="value">The value to compare to.</param>
        /// <returns>The linked specifications</returns>
        ISpecification<TCandidate> And<TSpecification>(int value)
            where TSpecification : CompareToValueSpecification<TCandidate, int>, new();

        /// <summary>
        /// Links two specifications using locical AND
        /// </summary>
        /// <typeparam name="TSpecification">The type of the specification.</typeparam>
        /// <param name="value">The value to compare to.</param>
        /// <returns>The linked specifications</returns>
        ISpecification<TCandidate> And<TSpecification>(uint value)
            where TSpecification : CompareToValueSpecification<TCandidate, uint>, new();

        /// <summary>
        /// Links two specifications using locical AND
        /// </summary>
        /// <typeparam name="TSpecification">The type of the specification.</typeparam>
        /// <param name="value">The value to compare to.</param>
        /// <returns>The linked specifications</returns>
        ISpecification<TCandidate> And<TSpecification>(short value)
            where TSpecification : CompareToValueSpecification<TCandidate, short>, new();

        /// <summary>
        /// Links two specifications using locical AND
        /// </summary>
        /// <typeparam name="TSpecification">The type of the specification.</typeparam>
        /// <param name="value">The value to compare to.</param>
        /// <returns>The linked specifications</returns>
        ISpecification<TCandidate> And<TSpecification>(ushort value)
            where TSpecification : CompareToValueSpecification<TCandidate, ushort>, new();

        /// <summary>
        /// Links two specifications using locical AND
        /// </summary>
        /// <typeparam name="TSpecification">The type of the specification.</typeparam>
        /// <param name="value">The value to compare to.</param>
        /// <returns>The linked specifications</returns>
        ISpecification<TCandidate> And<TSpecification>(long value)
            where TSpecification : CompareToValueSpecification<TCandidate, long>, new();

        /// <summary>
        /// Links two specifications using locical AND
        /// </summary>
        /// <typeparam name="TSpecification">The type of the specification.</typeparam>
        /// <param name="value">The value to compare to.</param>
        /// <returns>The linked specifications</returns>
        ISpecification<TCandidate> And<TSpecification>(ulong value)
            where TSpecification : CompareToValueSpecification<TCandidate, ulong>, new();

        /// <summary>
        /// Links two specifications using locical AND
        /// </summary>
        /// <typeparam name="TSpecification">The type of the specification.</typeparam>
        /// <param name="value">The value to compare to.</param>
        /// <returns>The linked specifications</returns>
        ISpecification<TCandidate> And<TSpecification>(float value)
            where TSpecification : CompareToValueSpecification<TCandidate, float>, new();

        /// <summary>
        /// Links two specifications using locical AND
        /// </summary>
        /// <typeparam name="TSpecification">The type of the specification.</typeparam>
        /// <param name="value">The value to compare to.</param>
        /// <returns>The linked specifications</returns>
        ISpecification<TCandidate> And<TSpecification>(double value)
            where TSpecification : CompareToValueSpecification<TCandidate, double>, new();

        /// <summary>
        /// Links two specifications using locical AND
        /// </summary>
        /// <typeparam name="TSpecification">The type of the specification.</typeparam>
        /// <param name="value">The value to compare to.</param>
        /// <returns>The linked specifications</returns>
        ISpecification<TCandidate> And<TSpecification>(char value)
            where TSpecification : CompareToValueSpecification<TCandidate, char>, new();

        /// <summary>
        /// Links two specifications using locical AND
        /// </summary>
        /// <typeparam name="TSpecification">The type of the specification.</typeparam>
        /// <param name="value">The value to compare to.</param>
        /// <returns>The linked specifications</returns>
        ISpecification<TCandidate> And<TSpecification>(bool value)
            where TSpecification : CompareToValueSpecification<TCandidate, bool>, new();

        /// <summary>
        /// Links two specifications using locical AND
        /// </summary>
        /// <typeparam name="TSpecification">The type of the specification.</typeparam>
        /// <param name="value">The value to compare to.</param>
        /// <returns>The linked specifications</returns>
        ISpecification<TCandidate> And<TSpecification>(string value)
            where TSpecification : CompareToValueSpecification<TCandidate, string>, new();

        /// <summary>
        /// Links two specifications using locical AND
        /// </summary>
        /// <typeparam name="TSpecification">The type of the specification.</typeparam>
        /// <param name="value">The value to compare to.</param>
        /// <returns>The linked specifications</returns>
        ISpecification<TCandidate> And<TSpecification>(decimal value)
            where TSpecification : CompareToValueSpecification<TCandidate, decimal>, new();

        /// <summary>
        /// Links two specifications using logical AND
        /// </summary>
        /// <param name="other">The other specification</param>
        /// <returns>The linked specifications</returns>
        ISpecification<TCandidate> And(ISpecification<TCandidate> other);

        /// <summary>
        /// Links two specifications using locical AND
        /// </summary>
        /// <typeparam name="TSpecification">The type of the specification.</typeparam>
        /// <param name="lowerBound">The lower bound.</param>
        /// <param name="upperBound">The upper bound.</param>
        /// <returns>The linked specifications</returns>
        ISpecification<TCandidate> And<TSpecification>(IComparable lowerBound, IComparable upperBound)
            where TSpecification : InRangeSpecification<TCandidate>, new();

        #endregion And

        #region Or

        /// <summary>
        /// Links two specifications using logical OR
        /// </summary>
        /// <param name="other">The other specification</param>
        /// <returns>The linked specifications</returns>
        ISpecification<TCandidate> Or(ISpecification<TCandidate> other);

        /// <summary>
        /// Links two specifications using locical OR
        /// </summary>
        /// <typeparam name="TSpecification">The type of the specification.</typeparam>
        /// <param name="value">The value to compare to.</param>
        /// <returns>The linked specifications</returns>
        ISpecification<TCandidate> Or<TSpecification>(bool value)
            where TSpecification : CompareToValueSpecification<TCandidate, bool>, new();


        /// <summary>
        /// Links two specifications using locical OR
        /// </summary>
        /// <typeparam name="TSpecification">The type of the specification.</typeparam>
        /// <param name="value">The value to compare to.</param>
        /// <returns>The linked specifications</returns>
        ISpecification<TCandidate> Or<TSpecification>(uint value)
            where TSpecification : CompareToValueSpecification<TCandidate, uint>, new();


        /// <summary>
        /// Links two specifications using locical OR
        /// </summary>
        /// <typeparam name="TSpecification">The type of the specification.</typeparam>
        /// <param name="value">The value to compare to.</param>
        /// <returns>The linked specifications</returns>
        ISpecification<TCandidate> Or<TSpecification>(int value)
            where TSpecification : CompareToValueSpecification<TCandidate, int>, new();


        /// <summary>
        /// Links two specifications using locical OR
        /// </summary>
        /// <typeparam name="TSpecification">The type of the specification.</typeparam>
        /// <param name="value">The value to compare to.</param>
        /// <returns>The linked specifications</returns>
        ISpecification<TCandidate> Or<TSpecification>(ulong value)
            where TSpecification : CompareToValueSpecification<TCandidate, ulong>, new();


        /// <summary>
        /// Links two specifications using locical OR
        /// </summary>
        /// <typeparam name="TSpecification">The type of the specification.</typeparam>
        /// <param name="value">The value to compare to.</param>
        /// <returns>The linked specifications</returns>
        ISpecification<TCandidate> Or<TSpecification>(long value)
            where TSpecification : CompareToValueSpecification<TCandidate, long>, new();


        /// <summary>
        /// Links two specifications using locical OR
        /// </summary>
        /// <typeparam name="TSpecification">The type of the specification.</typeparam>
        /// <param name="value">The value to compare to.</param>
        /// <returns>The linked specifications</returns>
        ISpecification<TCandidate> Or<TSpecification>(byte value)
            where TSpecification : CompareToValueSpecification<TCandidate, byte>, new();


        /// <summary>
        /// Links two specifications using locical OR
        /// </summary>
        /// <typeparam name="TSpecification">The type of the specification.</typeparam>
        /// <param name="value">The value to compare to.</param>
        /// <returns>The linked specifications</returns>
        ISpecification<TCandidate> Or<TSpecification>(sbyte value)
            where TSpecification : CompareToValueSpecification<TCandidate, sbyte>, new();


        /// <summary>
        /// Links two specifications using locical OR
        /// </summary>
        /// <typeparam name="TSpecification">The type of the specification.</typeparam>
        /// <param name="value">The value to compare to.</param>
        /// <returns>The linked specifications</returns>
        ISpecification<TCandidate> Or<TSpecification>(short value)
            where TSpecification : CompareToValueSpecification<TCandidate, short>, new();


        /// <summary>
        /// Links two specifications using locical OR
        /// </summary>
        /// <typeparam name="TSpecification">The type of the specification.</typeparam>
        /// <param name="value">The value to compare to.</param>
        /// <returns>The linked specifications</returns>
        ISpecification<TCandidate> Or<TSpecification>(ushort value)
            where TSpecification : CompareToValueSpecification<TCandidate, ushort>, new();


        /// <summary>
        /// Links two specifications using locical OR
        /// </summary>
        /// <typeparam name="TSpecification">The type of the specification.</typeparam>
        /// <param name="value">The value to compare to.</param>
        /// <returns>The linked specifications</returns>
        ISpecification<TCandidate> Or<TSpecification>(float value)
            where TSpecification : CompareToValueSpecification<TCandidate, float>, new();

        /// <summary>
        /// Links two specifications using locical OR
        /// </summary>
        /// <typeparam name="TSpecification">The type of the specification.</typeparam>
        /// <param name="value">The value to compare to.</param>
        /// <returns>The linked specifications</returns>
        ISpecification<TCandidate> Or<TSpecification>(double value)
            where TSpecification : CompareToValueSpecification<TCandidate, double>, new();

        /// <summary>
        /// Links two specifications using locical OR
        /// </summary>
        /// <typeparam name="TSpecification">The type of the specification.</typeparam>
        /// <param name="value">The value to compare to.</param>
        /// <returns>The linked specifications</returns>
        ISpecification<TCandidate> Or<TSpecification>(decimal value)
            where TSpecification : CompareToValueSpecification<TCandidate, decimal>, new();

        /// <summary>
        /// Links two specifications using locical OR
        /// </summary>
        /// <typeparam name="TSpecification">The type of the specification.</typeparam>
        /// <param name="value">The value to compare to.</param>
        /// <returns>The linked specifications</returns>
        ISpecification<TCandidate> Or<TSpecification>(char value)
            where TSpecification : CompareToValueSpecification<TCandidate, char>, new();

        /// <summary>
        /// Links two specifications using locical OR
        /// </summary>
        /// <typeparam name="TSpecification">The type of the specification.</typeparam>
        /// <param name="value">The value to compare to.</param>
        /// <returns>The linked specifications</returns>
        ISpecification<TCandidate> Or<TSpecification>(string value)
            where TSpecification : CompareToValueSpecification<TCandidate, string>, new();

        /// <summary>
        /// Links two specifications using locical OR
        /// </summary>
        /// <typeparam name="TSpecification">The type of the specification.</typeparam>
        /// <param name="lowerBound">The lower bound.</param>
        /// <param name="upperBound">The upper bound.</param>
        /// <returns>The linked specifications</returns>
        ISpecification<TCandidate> Or<TSpecification>(IComparable lowerBound, IComparable upperBound)
            where TSpecification : InRangeSpecification<TCandidate>, new();

        #endregion Or

        #region Not

        /// <summary>
        /// Negates the result of the current specification
        /// </summary>
        /// <returns>The negated specification</returns>
        ISpecification<TCandidate> Not();

        /// <summary>
        /// Links two specifications using logical NOT AND (NAND)
        /// </summary>
        /// <param name="other">The other specification</param>
        /// <returns>The linked specifications</returns>
        ISpecification<TCandidate> AndNot(ISpecification<TCandidate> other);

        #endregion Not
    }
}