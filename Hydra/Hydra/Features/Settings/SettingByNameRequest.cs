namespace Hydra.Features.Settings
{
    using Hydra.Infrastructure.Configuration;
    using Hydra.Infrastructure.Mediator;

    public class SettingByNameRequest : IRequest<Setting>
    {
        public SettingName Name { get; set; }
    }
}