namespace TecX.Search.Data.EF
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using TecX.Common;

    public class EFCustomerRepository : ICustomerRepository
    {
        private readonly ISearchEntities context;

        public EFCustomerRepository(ISearchEntities context)
        {
            Guard.AssertNotNull(context, "context");

            this.context = context;
        }

        public IQueryable<Customer> Customers
        {
            get
            {
                return this.context.Customers;
            }
        }
    }
}
