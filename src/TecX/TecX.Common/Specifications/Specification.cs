using System;

namespace TecX.Common.Specifications
{
    /// <summary>
    /// Abstract base class for all specifications.
    /// Part of the <c>Specification Pattern</c>
    /// </summary>
    /// <remarks>
    /// See http://en.wikipedia.org/wiki/Specification_pattern for further information on the pattern
    /// </remarks>
    /// <typeparam name="TCandidate">The type of the candidate objects the specification should
    /// work on</typeparam>
    public abstract class Specification<TCandidate> : ISpecification<TCandidate>
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
        public abstract bool IsSatisfiedBy(TCandidate candidate);

        #endregion Evaluation Method
        
        #region And

        /// <summary>
        /// Links two specifications using logical AND
        /// </summary>
        /// <param name="other">The other specification</param>
        /// <returns>The linked specifications</returns>
        public ISpecification<TCandidate> And(ISpecification<TCandidate> other)
        {
            Guard.AssertNotNull(other, "other");

            return new AndSpecification<TCandidate>(this, other);
        }

        /// <summary>
        /// Links two specifications using locical AND
        /// </summary>
        /// <typeparam name="TSpecification">The type of the specification.</typeparam>
        /// <param name="lowerBound">The lower bound.</param>
        /// <param name="upperBound">The upper bound.</param>
        /// <returns>The linked specifications</returns>
        public ISpecification<TCandidate> And<TSpecification>(IComparable lowerBound, IComparable upperBound)
            where TSpecification : RangeSpecification<TCandidate>, new()
        {
            Guard.AssertNotNull(lowerBound, "lowerBound");
            Guard.AssertNotNull(upperBound, "upperBound");

            var specification = new TSpecification
                                    {
                                        LowerBound = lowerBound,
                                        UpperBound = upperBound
                                    };

            return new AndSpecification<TCandidate>(this, specification);
        }

        /// <summary>
        /// Links two specifications using locical AND
        /// </summary>
        /// <typeparam name="TSpecification">The type of the specification.</typeparam>
        /// <param name="value">The value to compare to.</param>
        /// <returns>The linked specifications</returns>
        public ISpecification<TCandidate> And<TSpecification>(char value)
            where TSpecification : CompareToValueSpecification<TCandidate, char>, new()
        {
            var specification = new TSpecification
                                    {
                                        Value = value
                                    };

            return new AndSpecification<TCandidate>(this, specification);
        }


        /// <summary>
        /// Links two specifications using locical AND
        /// </summary>
        /// <typeparam name="TSpecification">The type of the specification.</typeparam>
        /// <param name="value">The value to compare to.</param>
        /// <returns>The linked specifications</returns>
        public ISpecification<TCandidate> And<TSpecification>(string value)
            where TSpecification : CompareToValueSpecification<TCandidate, string>, new()
        {
            var specification = new TSpecification
                                    {
                                        Value = value
                                    };

            return new AndSpecification<TCandidate>(this, specification);
        }

        /// <summary>
        /// Links two specifications using locical AND
        /// </summary>
        /// <typeparam name="TSpecification">The type of the specification.</typeparam>
        /// <param name="value">The value to compare to.</param>
        /// <returns>The linked specifications</returns>
        public ISpecification<TCandidate> And<TSpecification>(bool value)
            where TSpecification : CompareToValueSpecification<TCandidate, bool>, new()
        {
            var specification = new TSpecification
                                    {
                                        Value = value
                                    };

            return new AndSpecification<TCandidate>(this, specification);
        }

        /// <summary>
        /// Links two specifications using locical AND
        /// </summary>
        /// <typeparam name="TSpecification">The type of the specification.</typeparam>
        /// <param name="value">The value to compare to.</param>
        /// <returns>The linked specifications</returns>
        public ISpecification<TCandidate> And<TSpecification>(uint value)
            where TSpecification : CompareToValueSpecification<TCandidate, uint>, new()
        {
            var specification = new TSpecification
                                    {
                                        Value = value
                                    };

            return new AndSpecification<TCandidate>(this, specification);
        }

        /// <summary>
        /// Links two specifications using locical AND
        /// </summary>
        /// <typeparam name="TSpecification">The type of the specification.</typeparam>
        /// <param name="value">The value to compare to.</param>
        /// <returns>The linked specifications</returns>
        public ISpecification<TCandidate> And<TSpecification>(int value)
            where TSpecification : CompareToValueSpecification<TCandidate, int>, new()
        {
            var specification = new TSpecification
                                    {
                                        Value = value
                                    };

            return new AndSpecification<TCandidate>(this, specification);
        }

        /// <summary>
        /// Links two specifications using locical AND
        /// </summary>
        /// <typeparam name="TSpecification">The type of the specification.</typeparam>
        /// <param name="value">The value to compare to.</param>
        /// <returns>The linked specifications</returns>
        public ISpecification<TCandidate> And<TSpecification>(float value)
            where TSpecification : CompareToValueSpecification<TCandidate, float>, new()
        {
            var specification = new TSpecification
                                    {
                                        Value = value
                                    };

            return new AndSpecification<TCandidate>(this, specification);
        }

        /// <summary>
        /// Links two specifications using locical AND
        /// </summary>
        /// <typeparam name="TSpecification">The type of the specification.</typeparam>
        /// <param name="value">The value to compare to.</param>
        /// <returns>The linked specifications</returns>
        public ISpecification<TCandidate> And<TSpecification>(double value)
            where TSpecification : CompareToValueSpecification<TCandidate, double>, new()
        {
            var specification = new TSpecification
                                    {
                                        Value = value
                                    };

            return new AndSpecification<TCandidate>(this, specification);
        }

        /// <summary>
        /// Links two specifications using locical AND
        /// </summary>
        /// <typeparam name="TSpecification">The type of the specification.</typeparam>
        /// <param name="value">The value to compare to.</param>
        /// <returns>The linked specifications</returns>
        public ISpecification<TCandidate> And<TSpecification>(decimal value)
            where TSpecification : CompareToValueSpecification<TCandidate, decimal>, new()
        {
            var specification = new TSpecification
                                    {
                                        Value = value
                                    };

            return new AndSpecification<TCandidate>(this, specification);
        }

        /// <summary>
        /// Links two specifications using locical AND
        /// </summary>
        /// <typeparam name="TSpecification">The type of the specification.</typeparam>
        /// <param name="value">The value to compare to.</param>
        /// <returns>The linked specifications</returns>
        public ISpecification<TCandidate> And<TSpecification>(ulong value)
            where TSpecification : CompareToValueSpecification<TCandidate, ulong>, new()
        {
            var specification = new TSpecification
                                    {
                                        Value = value
                                    };

            return new AndSpecification<TCandidate>(this, specification);
        }

        /// <summary>
        /// Links two specifications using locical AND
        /// </summary>
        /// <typeparam name="TSpecification">The type of the specification.</typeparam>
        /// <param name="value">The value to compare to.</param>
        /// <returns>The linked specifications</returns>
        public ISpecification<TCandidate> And<TSpecification>(long value)
            where TSpecification : CompareToValueSpecification<TCandidate, long>, new()
        {
            var specification = new TSpecification
                                    {
                                        Value = value
                                    };

            return new AndSpecification<TCandidate>(this, specification);
        }

        /// <summary>
        /// Links two specifications using locical AND
        /// </summary>
        /// <typeparam name="TSpecification">The type of the specification.</typeparam>
        /// <param name="value">The value to compare to.</param>
        /// <returns>The linked specifications</returns>
        public ISpecification<TCandidate> And<TSpecification>(short value)
            where TSpecification : CompareToValueSpecification<TCandidate, short>, new()
        {
            var specification = new TSpecification
                                    {
                                        Value = value
                                    };

            return new AndSpecification<TCandidate>(this, specification);
        }

        /// <summary>
        /// Links two specifications using locical AND
        /// </summary>
        /// <typeparam name="TSpecification">The type of the specification.</typeparam>
        /// <param name="value">The value to compare to.</param>
        /// <returns>The linked specifications</returns>
        public ISpecification<TCandidate> And<TSpecification>(ushort value)
            where TSpecification : CompareToValueSpecification<TCandidate, ushort>, new()
        {
            var specification = new TSpecification
                                    {
                                        Value = value
                                    };

            return new AndSpecification<TCandidate>(this, specification);
        }

        /// <summary>
        /// Links two specifications using locical AND
        /// </summary>
        /// <typeparam name="TSpecification">The type of the specification.</typeparam>
        /// <param name="value">The value to compare to.</param>
        /// <returns>The linked specifications</returns>
        public ISpecification<TCandidate> And<TSpecification>(byte value)
            where TSpecification : CompareToValueSpecification<TCandidate, byte>, new()
        {
            var specification = new TSpecification
                                    {
                                        Value = value
                                    };

            return new AndSpecification<TCandidate>(this, specification);
        }

        /// <summary>
        /// Links two specifications using locical AND
        /// </summary>
        /// <typeparam name="TSpecification">The type of the specification.</typeparam>
        /// <param name="value">The value to compare to.</param>
        /// <returns>The linked specifications</returns>
        public ISpecification<TCandidate> And<TSpecification>(sbyte value)
            where TSpecification : CompareToValueSpecification<TCandidate, sbyte>, new()
        {
            var specification = new TSpecification
                                    {
                                        Value = value
                                    };

            return new AndSpecification<TCandidate>(this, specification);
        }

        #endregion And

        #region Or

        /// <summary>
        /// Links two specifications using logical OR
        /// </summary>
        /// <param name="other">The other specification</param>
        /// <returns>The linked specifications</returns>
        public ISpecification<TCandidate> Or(ISpecification<TCandidate> other)
        {
            Guard.AssertNotNull(other, "other");

            return new OrSpecification<TCandidate>(this, other);
        }

        /// <summary>
        /// Links two specifications using locical Or
        /// </summary>
        /// <typeparam name="TSpecification">The type of the specification.</typeparam>
        /// <param name="value">The value to compare to.</param>
        /// <returns>The linked specifications</returns>
        public ISpecification<TCandidate> Or<TSpecification>(float value)
            where TSpecification : CompareToValueSpecification<TCandidate, float>, new()
        {
            var specification = new TSpecification
                                    {
                                        Value = value
                                    };

            return new OrSpecification<TCandidate>(this, specification);
        }

        /// <summary>
        /// Links two specifications using locical Or
        /// </summary>
        /// <typeparam name="TSpecification">The type of the specification.</typeparam>
        /// <param name="value">The value to compare to.</param>
        /// <returns>The linked specifications</returns>
        public ISpecification<TCandidate> Or<TSpecification>(bool value)
            where TSpecification : CompareToValueSpecification<TCandidate, bool>, new()
        {
            var specification = new TSpecification
                                    {
                                        Value = value
                                    };

            return new OrSpecification<TCandidate>(this, specification);
        }

        /// <summary>
        /// Links two specifications using locical Or
        /// </summary>
        /// <typeparam name="TSpecification">The type of the specification.</typeparam>
        /// <param name="value">The value to compare to.</param>
        /// <returns>The linked specifications</returns>
        public ISpecification<TCandidate> Or<TSpecification>(double value)
            where TSpecification : CompareToValueSpecification<TCandidate, double>, new()
        {
            var specification = new TSpecification
                                    {
                                        Value = value
                                    };

            return new OrSpecification<TCandidate>(this, specification);
        }

        /// <summary>
        /// Links two specifications using locical Or
        /// </summary>
        /// <typeparam name="TSpecification">The type of the specification.</typeparam>
        /// <param name="value">The value to compare to.</param>
        /// <returns>The linked specifications</returns>
        public ISpecification<TCandidate> Or<TSpecification>(uint value)
            where TSpecification : CompareToValueSpecification<TCandidate, uint>, new()
        {
            var specification = new TSpecification
                                    {
                                        Value = value
                                    };

            return new OrSpecification<TCandidate>(this, specification);
        }

        /// <summary>
        /// Links two specifications using locical Or
        /// </summary>
        /// <typeparam name="TSpecification">The type of the specification.</typeparam>
        /// <param name="value">The value to compare to.</param>
        /// <returns>The linked specifications</returns>
        public ISpecification<TCandidate> Or<TSpecification>(int value)
            where TSpecification : CompareToValueSpecification<TCandidate, int>, new()
        {
            var specification = new TSpecification
                                    {
                                        Value = value
                                    };

            return new OrSpecification<TCandidate>(this, specification);
        }

        /// <summary>
        /// Links two specifications using locical Or
        /// </summary>
        /// <typeparam name="TSpecification">The type of the specification.</typeparam>
        /// <param name="value">The value to compare to.</param>
        /// <returns>The linked specifications</returns>
        public ISpecification<TCandidate> Or<TSpecification>(ulong value)
            where TSpecification : CompareToValueSpecification<TCandidate, ulong>, new()
        {
            var specification = new TSpecification
                                    {
                                        Value = value
                                    };

            return new OrSpecification<TCandidate>(this, specification);
        }

        /// <summary>
        /// Links two specifications using locical Or
        /// </summary>
        /// <typeparam name="TSpecification">The type of the specification.</typeparam>
        /// <param name="value">The value to compare to.</param>
        /// <returns>The linked specifications</returns>
        public ISpecification<TCandidate> Or<TSpecification>(long value)
            where TSpecification : CompareToValueSpecification<TCandidate, long>, new()
        {
            var specification = new TSpecification
                                    {
                                        Value = value
                                    };

            return new OrSpecification<TCandidate>(this, specification);
        }

        /// <summary>
        /// Links two specifications using locical Or
        /// </summary>
        /// <typeparam name="TSpecification">The type of the specification.</typeparam>
        /// <param name="value">The value to compare to.</param>
        /// <returns>The linked specifications</returns>
        public ISpecification<TCandidate> Or<TSpecification>(ushort value)
            where TSpecification : CompareToValueSpecification<TCandidate, ushort>, new()
        {
            var specification = new TSpecification
                                    {
                                        Value = value
                                    };

            return new OrSpecification<TCandidate>(this, specification);
        }

        /// <summary>
        /// Links two specifications using locical Or
        /// </summary>
        /// <typeparam name="TSpecification">The type of the specification.</typeparam>
        /// <param name="value">The value to compare to.</param>
        /// <returns>The linked specifications</returns>
        public ISpecification<TCandidate> Or<TSpecification>(short value)
            where TSpecification : CompareToValueSpecification<TCandidate, short>, new()
        {
            var specification = new TSpecification
                                    {
                                        Value = value
                                    };

            return new OrSpecification<TCandidate>(this, specification);
        }

        /// <summary>
        /// Links two specifications using locical Or
        /// </summary>
        /// <typeparam name="TSpecification">The type of the specification.</typeparam>
        /// <param name="value">The value to compare to.</param>
        /// <returns>The linked specifications</returns>
        public ISpecification<TCandidate> Or<TSpecification>(byte value)
            where TSpecification : CompareToValueSpecification<TCandidate, byte>, new()
        {
            var specification = new TSpecification
                                    {
                                        Value = value
                                    };

            return new OrSpecification<TCandidate>(this, specification);
        }

        /// <summary>
        /// Links two specifications using locical Or
        /// </summary>
        /// <typeparam name="TSpecification">The type of the specification.</typeparam>
        /// <param name="value">The value to compare to.</param>
        /// <returns>The linked specifications</returns>
        public ISpecification<TCandidate> Or<TSpecification>(sbyte value)
            where TSpecification : CompareToValueSpecification<TCandidate, sbyte>, new()
        {
            var specification = new TSpecification
                                    {
                                        Value = value
                                    };

            return new OrSpecification<TCandidate>(this, specification);
        }

        /// <summary>
        /// Links two specifications using locical Or
        /// </summary>
        /// <typeparam name="TSpecification">The type of the specification.</typeparam>
        /// <param name="value">The value to compare to.</param>
        /// <returns>The linked specifications</returns>
        public ISpecification<TCandidate> Or<TSpecification>(string value)
            where TSpecification : CompareToValueSpecification<TCandidate, string>, new()
        {
            var specification = new TSpecification
                                    {
                                        Value = value
                                    };

            return new OrSpecification<TCandidate>(this, specification);
        }

        /// <summary>
        /// Links two specifications using locical Or
        /// </summary>
        /// <typeparam name="TSpecification">The type of the specification.</typeparam>
        /// <param name="value">The value to compare to.</param>
        /// <returns>The linked specifications</returns>
        public ISpecification<TCandidate> Or<TSpecification>(char value)
            where TSpecification : CompareToValueSpecification<TCandidate, char>, new()
        {
            var specification = new TSpecification
                                    {
                                        Value = value
                                    };

            return new OrSpecification<TCandidate>(this, specification);
        }

        /// <summary>
        /// Links two specifications using locical Or
        /// </summary>
        /// <typeparam name="TSpecification">The type of the specification.</typeparam>
        /// <param name="value">The value to compare to.</param>
        /// <returns>The linked specifications</returns>
        public ISpecification<TCandidate> Or<TSpecification>(decimal value)
            where TSpecification : CompareToValueSpecification<TCandidate, decimal>, new()
        {
            var specification = new TSpecification
                                    {
                                        Value = value
                                    };

            return new OrSpecification<TCandidate>(this, specification);
        }

        /// <summary>
        /// Links two specifications using locical OR
        /// </summary>
        /// <typeparam name="TSpecification">The type of the specification.</typeparam>
        /// <param name="lowerBound">The lower bound.</param>
        /// <param name="upperBound">The upper bound.</param>
        /// <returns>The linked specifications</returns>
        public ISpecification<TCandidate> Or<TSpecification>(IComparable lowerBound, IComparable upperBound)
            where TSpecification : RangeSpecification<TCandidate>, new()
        {
            Guard.AssertNotNull(lowerBound, "lowerBound");
            Guard.AssertNotNull(upperBound, "upperBound");

            var specification = new TSpecification
                                    {
                                        LowerBound = lowerBound,
                                        UpperBound = upperBound
                                    };

            return new OrSpecification<TCandidate>(this, specification);
        }

        #endregion Or
        
        #region Not

        /// <summary>
        /// Negates the result of the current specification
        /// </summary>
        /// <returns>The negated specification</returns>
        public ISpecification<TCandidate> Not()
        {
            return new NotSpecification<TCandidate>(this);
        }

        /// <summary>
        /// Links two specifications using logical AND NOT
        /// </summary>
        /// <param name="other">The other specification</param>
        /// <returns>The linked specifications</returns>
        public ISpecification<TCandidate> AndNot(ISpecification<TCandidate> other)
        {
            return new AndSpecification<TCandidate>(this, other.Not());
        }

        #endregion Not
    }
}