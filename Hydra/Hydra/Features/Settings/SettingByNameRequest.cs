namespace Hydra.Features.Settings
{
    using FluentValidation.Attributes;
    using Hydra.Infrastructure.Configuration;
    using Hydra.Infrastructure.Mediator;

    [Validator(typeof(SettingByNameRequestValidator))]
    public class SettingByNameRequest : IRequest<Setting>
    {
        public SettingName Name { get; set; }
    }
}