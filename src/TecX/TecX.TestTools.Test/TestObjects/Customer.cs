namespace TecX.TestTools.Test.TestObjects
{
    using System;

    public class Customer
    {
        public Customer(int id)
        {
            this.Id = id;
        }

        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Company { get; set; }

        public string Description { get; set; }

        public int Locations { get; set; }

        public DateTime IncorporatedOn { get; set; }

        public double Revenue { get; set; }

        public Address WorkAddress { get; set; }

        public Address HomeAddress { get; set; }

        public Address[] Addresses { get; set; }

        public string HomePhone { get; set; }

        public int Type { get; set; }
    }
}