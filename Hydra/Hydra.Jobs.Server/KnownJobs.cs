namespace Hydra.Jobs.Server
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics.Contracts;
    using System.Runtime.CompilerServices;
    using Hydra.Infrastructure;
    using Quartz;

    [Serializable]
    [TypeConverter(typeof(EnumerationTypeConverter<KnownJobs>))]
    public class KnownJobs : Enumeration<KnownJobs>
    {
        private readonly Type jobType;

        public static readonly KnownJobs Noop = new KnownJobs(typeof(NoOp));

        private KnownJobs(Type jobType, [CallerMemberName] string name = "", [CallerLineNumber] int key = -1)
            : base(name, key)
        {
            Contract.Requires(jobType != null);
            Contract.Requires(typeof(IJob).IsAssignableFrom(jobType));

            this.jobType = jobType;
        }

        public Type JobType
        {
            get { return this.jobType; }
        }
    }
}