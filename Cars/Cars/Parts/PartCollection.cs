namespace Cars.Parts
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;

    public class PartCollection : IReadOnlyCollection<Part>
    {
        private readonly HashSet<Part> parts;

        public PartCollection()
        {
            this.parts = new HashSet<Part>();
        }

        public int Count
        {
            get { return this.parts.Count; }
        }

        public IReadOnlyCollection<Part> AddNewAndReplaceExisting(Part newPart)
        {
            Contract.Requires(newPart != null);

            if (this.parts.Contains(newPart))
            {
                return new Part[0];
            }

            var replacedParts = this.parts
                .Where(p => newPart.ReplacesTheseParts.Contains(p.PartNumber))
                .ToArray();

            HashSet<Part> thesePartsWhereReplaced = new HashSet<Part>();
            foreach (Part replacedPart in replacedParts)
            {
                this.parts.Remove(replacedPart);
                thesePartsWhereReplaced.Add(replacedPart);
            }

            this.parts.Add(newPart);

            return thesePartsWhereReplaced;
        }

        public IEnumerator<Part> GetEnumerator()
        {
            return this.parts.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}