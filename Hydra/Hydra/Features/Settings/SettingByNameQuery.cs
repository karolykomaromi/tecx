namespace Hydra.Features.Settings
{
    using Hydra.Infrastructure.Configuration;
    using Hydra.Queries;

    public class SettingByNameQuery : IQuery<Setting>
    {
        public SettingName Name { get; set; }
    }
}