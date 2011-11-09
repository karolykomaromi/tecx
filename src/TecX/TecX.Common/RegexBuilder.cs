namespace TecX.Common
{
    using System.Text;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Builder for very simple regular expressions (developed for recognizing Guids)
    /// </summary>
    public class RegexBuilder
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RegexBuilder"/> class
        /// </summary>
        public RegexBuilder()
        {
            this.Pattern = new StringBuilder(256);
        }

        /// <summary>
        /// Gets the pattern used by the regular expression
        /// </summary>
        /// <value>The pattern.</value>
        public StringBuilder Pattern { get; private set; }

        /// <summary>
        /// Performs an implicit conversion from <see cref="System.Text.RegularExpressions.Regex"/> to <see cref="Regex"/>.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator Regex(RegexBuilder builder)
        {
            Guard.AssertNotNull(builder, "builder");

            return builder.Build();
        }

        /// <summary>
        /// Builds a regular expression using the constructed <see cref="Pattern"/>
        /// </summary>
        /// <returns>A <see cref="System.Text.RegularExpressions.Regex"/></returns>
        public Regex Build()
        {
            return new Regex(this.Pattern.ToString());
        }

        /// <summary>
        /// Appends an end of string ($) marker to the pattern
        /// </summary>
        /// <returns>Self. Fluent interface</returns>
        public RegexBuilder ToEndOfString()
        {
            this.Pattern.Append("$");

            return this;
        }

        /// <summary>
        /// Appends a start of string (^) marker to the pattern
        /// </summary>
        /// <returns>Self. Fluent interface</returns>
        public RegexBuilder StartingFromBeginning()
        {
            this.Pattern.Append("^");

            return this;
        }

        /// <summary>
        /// Appends an any single char marker (.) to the pattern
        /// </summary>
        /// <returns>Self. Fluent interface</returns>
        public RegexBuilder AnyChar()
        {
            this.Pattern.Append(".");

            return this;
        }

        /// <summary>
        /// Appends a marker for a hexadecimal char ([a-fA-F0-9]) to the pattern
        /// </summary>
        /// <returns>Self. Fluent interface</returns>
        public RegexBuilder AHexDigit()
        {
            this.Pattern.Append("[a-fA-F0-9]");

            return this;
        }

        /// <summary>
        /// Appends a marker for a single specific char to the pattern
        /// </summary>
        /// <param name="specificChar">The specific char.</param>
        /// <returns>Self. Fluent interface</returns>
        public RegexBuilder ASpecificChar(char specificChar)
        {
            Guard.AssertNotNull(specificChar, "specificChar");

            this.Pattern.Append(specificChar);

            return this;
        }

        /// <summary>
        /// Appends a marker for a range of occurences ({atLeast,atMost}) to the pattern
        /// </summary>
        /// <param name="atLeast">At least.</param>
        /// <param name="atMost">At most.</param>
        /// <returns>Self. Fluent interface</returns>
        public RegexBuilder OccursForRangeOfNumberOfTimes(int atLeast, int atMost)
        {
            Guard.AssertIsInRange(atLeast, "atLeast", 0, int.MaxValue);
            Guard.AssertIsInRange(atMost, "atMost", atLeast, int.MaxValue);

            this.Pattern.Append("{")
                .Append(atLeast)
                .Append(",")
                .Append(atMost)
                .Append("}");

            return this;
        }

        /// <summary>
        /// Appends a marker for a specific number of occurences ({numberOfOccurences}) to the pattern
        /// </summary>
        /// <param name="numberOfOccurences">The number of occurences.</param>
        /// <returns>Self. Fluent interface</returns>
        public RegexBuilder OccursForSpecificNumberOfTimes(int numberOfOccurences)
        {
            this.Pattern.Append("{")
                .Append(numberOfOccurences)
                .Append("}");

            return this;
        }

        /// <summary>
        /// Appends a marker for zero or more occurences (*) to the pattern
        /// </summary>
        /// <returns>Self. Fluent interface</returns>
        public RegexBuilder OccursZeroOrMoreTimes()
        {
            this.Pattern.Append("*");

            return this;
        }

        /// <summary>
        /// Appends a marker for at least one occurence (+) to the pattern
        /// </summary>
        /// <returns>Self. Fluent interface</returns>
        public RegexBuilder OccursAtLeastOnce()
        {
            this.Pattern.Append("+");

            return this;
        }

        /// <summary>
        /// Appends a marker for at most a certain number of occurences ({0,atMost}) to the pattern
        /// </summary>
        /// <param name="atMost">At most.</param>
        /// <returns>Self. Fluent interface</returns>
        public RegexBuilder OccursAtMost(int atMost)
        {
            Guard.AssertIsInRange(atMost, "atMost", 0, int.MaxValue);

            this.Pattern.Append("{0,")
                .Append(atMost)
                .Append("}");

            return this;
        }
    }
}