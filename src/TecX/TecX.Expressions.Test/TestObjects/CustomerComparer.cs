namespace TecX.Expressions.Test.TestObjects
{
    using System.Collections.Generic;

    public class CustomerComparer : Comparer<Customer>
    {
        public override int Compare(Customer x, Customer y)
        {
            return x.Id.CompareTo(y.Id);
        }
    }
}