using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using Xunit;

namespace Cars.Test
{
    public class YourCarTests
    {
        [Fact]
        public void Selecting_Part_Should_Move_It_To_Selected_Parts_And_Remove_From_Available_Parts()
        {
            Part p = new Part("16_STEEL");

            var sut = new YourCar(p);

            Assert.True(sut.Select(p));
            Assert.Contains(p, sut.SelectedParts);
            Assert.DoesNotContain(p, sut.AvailableParts);
        }

        [Fact]
        public void Parts_From_Ctor_Should_Be_Placed_In_Available_Parts()
        {
            Part p = new Part("16_STEEL");

            var sut = new YourCar(p);

            Assert.Contains(p, sut.AvailableParts);
        }

        [Fact]
        public void Should_Replace_Appropriate_Parts()
        {
            Part alu = new Part("17_ALU", new[] { new PartNumber("16_STEEL") }, PartNumber.None);
            Part steel = new Part("16_STEEL", new[] { new PartNumber("17_ALU") }, PartNumber.None);

            var sut = new YourCar(alu, steel);
            sut.Select(steel);

            Assert.True(sut.Select(alu));
            Assert.Contains(alu, sut.SelectedParts);
            Assert.Contains(steel, sut.AvailableParts);
        }

        [Fact]
        public void Should_Lock_Appropriate_Parts()
        {
            Package basic = new Package("BASIC", PartNumber.None, new[] { new PartNumber("SURPLUS") });
            Package surplus = new Package("SURPLUS", PartNumber.None, new[] { new PartNumber("BASIC") });

            var sut = new YourCar(basic, surplus);

            Assert.True(sut.Select(basic));
            Assert.Contains(basic, sut.SelectedParts);
            Assert.Contains(surplus, sut.PartsThatCantBeUsedInCurrentSelection);
        }
    }

    public class YourCar
    {
        private readonly HashSet<Part> availableParts;
        private readonly HashSet<Part> selectedParts;
        private readonly HashSet<Part> partsThatCantBeUsedInCurrentSelection;

        public YourCar(params Part[] availableParts)
        {
            this.availableParts = new HashSet<Part>(availableParts ?? new Part[0]);
            this.selectedParts = new HashSet<Part>();
            this.partsThatCantBeUsedInCurrentSelection = new HashSet<Part>();
        }

        public CurrencyAmount Total
        {
            get { return this.selectedParts.Select(p => p.Price).Sum(); }
        }

        public IReadOnlyCollection<Part> PartsThatCantBeUsedInCurrentSelection
        {
            get { return this.partsThatCantBeUsedInCurrentSelection; }
        }

        public IReadOnlyCollection<Part> AvailableParts
        {
            get { return this.availableParts; }
        }

        public IReadOnlyCollection<Part> SelectedParts
        {
            get { return selectedParts; }
        }

        public bool Select(Part part)
        {
            Contract.Requires(part != null);

            bool noLongerAvailable = this.availableParts.Remove(part);

            this.RemovePartsThatAreSupersededBySelectedPart(part);

            bool selected = this.selectedParts.Add(part);

            this.LockPartsThatCantBeUsedInCombinationWithSelectedPart();

            return noLongerAvailable && selected;
        }

        private void LockPartsThatCantBeUsedInCombinationWithSelectedPart()
        {
            var partsThatCantBeUsed = this.selectedParts
                .SelectMany(p => p.CantBeCombinedWithTheseParts)
                .Distinct()
                .ToArray();

            foreach (Part availablePart in this.availableParts.ToArray())
            {
                if (partsThatCantBeUsed.Contains(availablePart.PartNumber))
                {
                    Part unuseablePart = availablePart;

                    this.availableParts.Remove(unuseablePart);
                    this.partsThatCantBeUsedInCurrentSelection.Add(unuseablePart);
                }
            }
        }

        private void RemovePartsThatAreSupersededBySelectedPart(Part part)
        {
            var replacedParts = this.selectedParts
                .Where(currentlySelectedPart => part.ReplacesTheseParts.Contains(currentlySelectedPart.PartNumber))
                .ToArray();

            foreach (Part replacedPart in replacedParts)
            {
                this.selectedParts.Remove(replacedPart);
                this.availableParts.Add(replacedPart);
            }
        }
    }
}