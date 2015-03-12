namespace Hydra.Features.Settings
{
    using Hydra.Infrastructure.Configuration;
    using Hydra.Infrastructure.Mediator;

    public class AllSettingsQuery : IRequest<SettingsCollection>
    {
    }
}