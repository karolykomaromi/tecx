namespace TecX.Search.Data.EF
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Text;

    public class SearchEntities : DbContext, ISearchEntities
    {
        public SearchEntities()
        {
            // TODO weberse 2011-12-02 initialization of sets must be done here
        }

        public IDbSet<Customer> Customers { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new CustomerConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
