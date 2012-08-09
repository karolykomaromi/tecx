namespace TecX.Unity.Groups
{
    using System.Collections.Generic;

    public class MappingGroupPolicy : IMappingGroupPolicy
    {
        private readonly List<MappingInfo> mappingInfos;

        public MappingGroupPolicy()
        {
            this.mappingInfos = new List<MappingInfo>();
        }

        public ICollection<MappingInfo> MappingInfos
        {
            get
            {
                return this.mappingInfos;
            }
        }
    }
}