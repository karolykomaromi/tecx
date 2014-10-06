namespace TecX.Search.Parse
{
    using System;
    
    using TecX.Common;

    public abstract class ParameterParseStrategy
    {
        private IFormatProvider formatProvider;

        protected ParameterParseStrategy()
            : this(Defaults.Culture)
        {
        }

        protected ParameterParseStrategy(IFormatProvider formatProvider)
        {
            Guard.AssertNotNull(formatProvider, "formatProvider");

            this.formatProvider = formatProvider;
        }

        public IFormatProvider FormatProvider
        {
            get
            {
                return this.formatProvider;
            }

            set
            {
                Guard.AssertNotNull(value, "FormatProvider");

                this.formatProvider = value;
            }
        }

        public void Parse(ParameterParseContext context)
        {
            Guard.AssertNotNull(context, "context");
            Guard.AssertNotNull(context.StringToParse, "context.parameterToParse");

            if (context.ParseComplete)
            {
                throw new ArgumentException("Another strategy flagged parsing as completed.", "context.ParseComplete");
            }

            if (context.Parameter != null)
            {
                throw new ArgumentException("Parsed parameter already present in context.", "context.Parameter");
            }

            this.ParseCore(context);

            if (context.Parameter != null)
            {
                context.ParseComplete = true;
            }
        }

        protected abstract void ParseCore(ParameterParseContext context);
    }
}
