namespace TecX.Expressions.Test.TestObjects
{
    using System.Collections.Generic;

    public class CustomerComparer : EqualityComparer<Customer>
    {
        public override bool Equals(Customer x, Customer y)
        {
            return x.Id == y.Id;
        }

        public override int GetHashCode(Customer obj)
        {
            return obj.Id;
        }
    }
}