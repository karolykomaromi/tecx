namespace Cars.Parts
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using Cars.Financial;

    public class Package : Part, IEnumerable<Part>
    {
        private readonly HashSet<Part> parts;

        public Package(PartNumber partNumber)
            : this(partNumber, PartNumber.None, PartNumber.None)
        {
        }

        public Package(PartNumber partNumber, PartNumber[] replacesTheseParts, PartNumber[] cantBeCombinedWithTheseParts) 
            : base(partNumber, replacesTheseParts, cantBeCombinedWithTheseParts)
        {
            this.parts = new HashSet<Part>();
        }

        public override IReadOnlyCollection<PartNumber> CantBeCombinedWithTheseParts
        {
            get
            {
                return this.parts
                    .SelectMany(p => p.CantBeCombinedWithTheseParts)
                    .Concat(base.CantBeCombinedWithTheseParts)
                    .Distinct()
                    .ToArray();
            }
        }

        public override IReadOnlyCollection<PartNumber> ReplacesTheseParts
        {
            get
            {
                return this.parts
                    .SelectMany(p => p.ReplacesTheseParts)
                    .Concat(base.ReplacesTheseParts)
                    .Distinct()
                    .ToArray();
            }
        }

        public CurrencyAmount SumPriceOfIndividualParts
        {
            get
            {
                return this.parts
                    .Select(p => p.Price)
                    .Sum();
            }
        }

        public bool Add(Part part)
        {
            Contract.Requires(part != null);

            return this.parts.Add(part);
        }

        public IEnumerator<Part> GetEnumerator()
        {
            return this.parts.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder(200);

            CultureInfo culture = CultureInfo.CurrentUICulture;

            sb.AppendLine(this.Abstract.ToString(culture));

            foreach (Part part in this.parts)
            {
                sb.Append("\t- ").AppendLine(part.ToString());
            }

            return sb.ToString();
        }
    }
}