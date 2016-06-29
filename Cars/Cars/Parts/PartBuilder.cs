namespace Cars.Parts
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using Cars.I18n;

    public class PartBuilder : Builder<Part>
    {
        private readonly HashSet<PartNumber> replaces;
        private readonly HashSet<PartNumber> excludes;
        private PartNumber partNumber;
        private PolyglotStringBuilder description;
        private PolyglotStringBuilder @abstract;

        public PartBuilder()
        {
            this.partNumber = Cars.Parts.PartNumber.Empty;
            this.description = new PolyglotStringBuilder();
            this.@abstract = new PolyglotStringBuilder();
            this.excludes = new HashSet<PartNumber>();
            this.replaces = new HashSet<PartNumber>();
        }

        public PartBuilder PartNumber(PartNumber partNumber)
        {
            Contract.Requires(partNumber != null);
            Contract.Ensures(Contract.Result<PartBuilder>() != null);

            this.partNumber = partNumber;

            return this;
        }

        public PartBuilder Description(Action<PolyglotStringBuilder> action)
        {
            Contract.Requires(action != null);
            Contract.Ensures(Contract.Result<PackageBuilder>() != null);

            var builder = new PolyglotStringBuilder();

            action(builder);

            this.description = builder;

            return this;
        }

        public PartBuilder Abstract(Action<PolyglotStringBuilder> action)
        {
            Contract.Requires(action != null);
            Contract.Ensures(Contract.Result<PackageBuilder>() != null);

            var builder = new PolyglotStringBuilder();

            action(builder);

            this.@abstract = builder;

            return this;
        }


        public PartBuilder Excludes(PartNumber partNumber)
        {
            Contract.Requires(partNumber != null);
            Contract.Ensures(Contract.Result<PartBuilder>() != null);

            this.excludes.Add(partNumber);

            return this;
        }

        public PartBuilder Replaces(PartNumber partNumber)
        {
            Contract.Requires(partNumber != null);
            Contract.Ensures(Contract.Result<PartBuilder>() != null);

            this.replaces.Add(partNumber);

            return this;
        }

        public override Part Build()
        {
            return new Part(this.partNumber, this.replaces.ToArray(), this.excludes.ToArray())
            {
                Description = this.description,
                Abstract = this.@abstract
            };
        }
    }
}