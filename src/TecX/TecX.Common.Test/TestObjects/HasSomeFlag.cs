using System.Collections.Generic;
using TecX.Common.Specifications;

namespace TecX.Common.Test.TestObjects
{
    internal class HasSomeFlag : CompareToValueSpecification<SearchTestEntity, bool>
    {
        public override string Description
        {
            get { return "HasSomeFlag"; }
        }
        protected override bool IsMatchCore(SearchTestEntity candidate, ICollection<ISpecification<SearchTestEntity>> matchedSpecifications)
        {
            return candidate.HasSomeFlag;
        }
    }
}