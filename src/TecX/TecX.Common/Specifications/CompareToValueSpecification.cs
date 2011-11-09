namespace TecX.Common.Specifications
{
    /// <summary>
    /// Abstract base class for specifications that work on primitive types (plus strings which don't really belong
    /// in any category)
    /// </summary>
    /// <typeparam name="TCandidate">The type of the candidate.</typeparam>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    public abstract class CompareToValueSpecification<TCandidate, TValue> : Specification<TCandidate>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CompareToValueSpecification&lt;TCandidate, TValue&gt;"/> class.
        /// </summary>
        protected CompareToValueSpecification()
        {   
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CompareToValueSpecification&lt;TCandidate, TValue&gt;"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        protected CompareToValueSpecification(TValue value)
        {
            Guard.AssertNotNull(value, "value");

            this.Value = value;
        }

        /// <summary>
        /// Gets or sets the value against which a candidate is compared
        /// </summary>
        /// <value>The value.</value>
        public TValue Value { get; set; }
    }
}