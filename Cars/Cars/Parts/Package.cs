using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using Cars.Financial;

namespace Cars.Parts
{
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
            return GetEnumerator();
        }
    }
}