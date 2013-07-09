namespace TecX.Playground.QueryAbstractionLayer.Simulation
{
    using System;
    using System.Collections.Generic;

    using TecX.Playground.QueryAbstractionLayer.PD;

    public class FooBuilder
    {
        private int nextID = 1;

        private long id;

        private long principalId;

        private string bar;

        private bool isDeleted;

        public IEnumerable<Foo> Build(int numberOfFoosToCreate)
        {
            List<Foo> foos = new List<Foo>();

            for (int i = 0; i < numberOfFoosToCreate; i++)
            {
                Foo foo = this.WithId(nextID++).Build();

                foos.Add(foo);
            }

            return foos;
        }

        public Foo Build()
        {
            return new Foo { PDO_ID = this.id, Principal = new PDPrincipal { PDO_ID = GetPrincipalID() }, Bar = GetBar(), PDO_DELETED = GetDeleted() };
        }

        public FooBuilder WithId(long id)
        {
            this.id = id;

            return this;
        }

        public FooBuilder WithIsDeleted(bool isDeleted)
        {
            this.isDeleted = isDeleted;

            return this;
        }

        public FooBuilder WithBar(string bar)
        {
            this.bar = bar;

            return this;
        }

        private DateTime? GetDeleted()
        {
            if (!this.isDeleted)
            {
                return null;
            }

            return DateTime.Now;
        }

        private long GetPrincipalID()
        {
            if (this.principalId != 0)
            {
                return this.principalId;
            }

            if (this.principalId % 2 == 0)
            {
                return 42;
            }

            return 1337;
        }

        private string GetBar()
        {
            if (string.IsNullOrEmpty(this.bar))
            {
                return "MyID is " + this.id;
            }

            return this.bar;
        }
    }
}