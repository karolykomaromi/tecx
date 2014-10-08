namespace TecX.Unity.Groups
{
    using System;

    using Microsoft.Practices.Unity;

    public class GroupingContainerProxy : UnityContainerProxy
    {
        private readonly IMappingGroupPolicy policy;

        private string groupName;

        private Type groupParentType;

        public GroupingContainerProxy(IUnityContainer container)
            : base(container)
        {
            this.PreRegistering += this.OnPreRegistering;
            this.PreRegisteringInstance += this.OnPreRegisteringInstance;

            this.policy = new MappingGroupPolicy();

            this.groupName = null;
            this.groupParentType = null;
        }

        public override void Dispose()
        {
            this.PreRegistering -= this.OnPreRegistering;
            this.PreRegisteringInstance -= this.OnPreRegisteringInstance;

            IGroupedMappings semantic = this.Container.Configure<IGroupedMappings>();

            if (semantic == null)
            {
                var extension = new MappingGroupExtension();

                semantic = extension;

                this.Container.AddExtension(extension);
            }

            semantic.AddPolicy(this.policy, this.groupParentType, this.groupName);
        }

        private void OnPreRegisteringInstance(object sender, PreRegisteringInstanceEventArgs args)
        {
            if (!string.IsNullOrEmpty(args.OriginalName) && string.IsNullOrEmpty(this.groupName))
            {
                this.groupName = args.OriginalName;
            }

            if (this.groupParentType == null)
            {
                this.groupParentType = args.To;
            }

            args.Name = this.groupName;

            this.policy.MappingInfos.Add(new MappingInfo { From = args.To, Name = args.Name, To = args.To });
        }

        private void OnPreRegistering(object sender, PreRegisteringEventArgs args)
        {
            if (!string.IsNullOrEmpty(args.OriginalName) && string.IsNullOrEmpty(this.groupName))
            {
                this.groupName = args.OriginalName;
            }

            if (this.groupParentType == null)
            {
                this.groupParentType = args.To;
            }

            args.Name = this.groupName;

            this.policy.MappingInfos.Add(new MappingInfo { From = args.From, Name = args.Name, To = args.To });
        }
    }
}