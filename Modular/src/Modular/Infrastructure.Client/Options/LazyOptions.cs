namespace Infrastructure.Options
{
    using System;
    using System.Diagnostics.Contracts;

    public class LazyOptions : IOptions
    {
        private readonly Lazy<IOptions> lazy;

        public LazyOptions(Func<IOptions> factory)
        {
            Contract.Requires(factory != null);

            this.lazy = new Lazy<IOptions>(factory);
        }

        public object this[Option option]
        {
            get
            {
                return this.lazy.Value[option];
            }

            set
            {
                this.lazy.Value[option] = value;
            }
        }

        public bool KnowsAbout(Option option)
        {
            return this.lazy.Value.KnowsAbout(option);
        }
    }
}
