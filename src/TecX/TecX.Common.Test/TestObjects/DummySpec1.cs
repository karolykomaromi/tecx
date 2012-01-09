using System.Collections.Generic;
using TecX.Common.Specifications;

namespace TecX.Common.Test.TestObjects
{
    public class DummySpec1 : Specification<SearchTestEntity>
    {
        public override string Description
        {
            get { return "DummySpec1"; }
        }

        protected override bool IsMatchCore(SearchTestEntity candidate, ICollection<ISpecification<SearchTestEntity>> matchedSpecifications)
        {
            return true;
        }
    }
}