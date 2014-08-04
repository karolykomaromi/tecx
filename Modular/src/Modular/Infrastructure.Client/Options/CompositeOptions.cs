namespace Infrastructure.Options
{
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;

    public class CompositeOptions : IOptions
    {
        private readonly HashSet<IOptions> options;

        public CompositeOptions(params IOptions[] options)
        {
            this.options = new HashSet<IOptions>(options ?? new IOptions[0]);
        }

        public object this[Option option]
        {
            get
            {
                foreach (IOptions o in this.options)
                {
                    if (o.KnowsAbout(option))
                    {
                        return o[option];
                    }
                }

                throw new UnknownOptionException(option, this.GetType());
            }

            set
            {
                foreach (IOptions o in this.options)
                {
                    if (o.KnowsAbout(option))
                    {
                        o[option] = value;
                    }
                }
            }
        }

        public bool KnowsAbout(Option option)
        {
            return this.options.Any(o => o.KnowsAbout(option));
        }

        public void Add(IOptions options)
        {
            Contract.Requires(options != null);

            this.options.Add(options);
        }
    }
}
