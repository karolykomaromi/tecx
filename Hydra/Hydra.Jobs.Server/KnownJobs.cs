﻿namespace Hydra.Jobs.Server
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics.Contracts;
    using System.Runtime.CompilerServices;
    using Hydra.Infrastructure;
    using Hydra.Jobs.Server.Jobs;
    using Quartz;

    [Serializable]
    [TypeConverter(typeof(EnumerationTypeConverter<KnownJobs>))]
    public class KnownJobs : Enumeration<KnownJobs>
    {
        public static readonly KnownJobs Nop = new KnownJobs(typeof(Nop));

        public static readonly KnownJobs Heartbeat = new KnownJobs(typeof(Heartbeat));

        public static readonly KnownJobs BatchMail = new KnownJobs(typeof(BatchMail));

        private readonly Type jobType;

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