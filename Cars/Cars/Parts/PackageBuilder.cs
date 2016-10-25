namespace Cars.Parts
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using Cars.I18n;

    public class PackageBuilder : Builder<Package>
    {
        private readonly HashSet<PartNumber> replaces;
        private readonly HashSet<PartNumber> excludes;
        private readonly HashSet<PartBuilder> parts;
        private PartNumber partNumber;
        private PolyglotStringBuilder description;
        private PolyglotStringBuilder @abstract;

        public PackageBuilder()
        {
            this.partNumber = Cars.Parts.PartNumber.Empty;
            this.description = new PolyglotStringBuilder();
            this.@abstract = new PolyglotStringBuilder();
            this.excludes = new HashSet<PartNumber>();
            this.replaces = new HashSet<PartNumber>();
            this.parts = new HashSet<PartBuilder>();
        }

        public PackageBuilder PartNumber(PartNumber partNumber)
        {
            Contract.Requires(partNumber != null);
            Contract.Ensures(Contract.Result<PartBuilder>() != null);

            this.partNumber = partNumber;

            return this;
        }

        public PackageBuilder Description(Action<PolyglotStringBuilder> action)
        {
            Contract.Requires(action != null);
            Contract.Ensures(Contract.Result<PackageBuilder>() != null);
            
            var builder = new PolyglotStringBuilder();

            action(builder);

            this.description = builder;

            return this;
        }

        public PackageBuilder Abstract(Action<PolyglotStringBuilder> action)
        {
            Contract.Requires(action != null);
            Contract.Ensures(Contract.Result<PackageBuilder>() != null);
            
            var builder = new PolyglotStringBuilder();

            action(builder);

            this.@abstract = builder;

            return this;
        }

        public PackageBuilder Excludes(PartNumber partNumber)
        {
            Contract.Requires(partNumber != null);
            Contract.Ensures(Contract.Result<PartBuilder>() != null);

            this.excludes.Add(partNumber);

            return this;
        }

        public PackageBuilder Replaces(PartNumber partNumber)
        {
            Contract.Requires(partNumber != null);
            Contract.Ensures(Contract.Result<PartBuilder>() != null);

            this.replaces.Add(partNumber);

            return this;
        }

        public PackageBuilder Part(Action<PartBuilder> action)
        {
            Contract.Requires(action != null);
            Contract.Ensures(Contract.Result<PackageBuilder>() != null);

            PartBuilder builder = new PartBuilder();

            action(builder);

            this.parts.Add(builder);

            return this;
        }

        public override Package Build()
        {
            var package = new Package(this.partNumber, this.replaces.ToArray(), this.excludes.ToArray())
            {
                Description = this.description,
                Abstract = this.@abstract
            };

            foreach (PartBuilder part in this.parts)
            {
                package.Add(part);
            }

            return package;
        }
    }
}