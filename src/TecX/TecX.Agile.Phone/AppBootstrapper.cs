namespace TecX.Agile.Phone
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Controls;

    using Caliburn.Micro;

    using Microsoft.Phone.Controls;

    using TecX.Agile.Phone.Data;
    using TecX.Agile.Phone.ViewModels;
    using TecX.CaliburnEx;

    public class AppBootstrapper : PhoneBootstrapper
    {
        private PhoneContainer container;

        protected override void Configure()
        {
            this.container = new PhoneContainer(this.RootFrame);

            this.container.RegisterPhoneServices();
            this.container.PerRequest<MainPageViewModel>();
            this.container.PerRequest<PivotPageViewModel>();
            this.container.PerRequest<TabViewModel>();
            this.container.Handler<IProjectApplicationService>(sc => new DummyProjectApplicationService());

            AddCustomConventions();

            LogManager.GetLog = type => new DebugLog(type);

            MessageBinder.CustomConverters.Add(typeof(ScrollChangedParameter), new ScrollChangedConverter().Convert);
        }

        static void AddCustomConventions()
        {
            ConventionManager.AddElementConvention<Pivot>(Pivot.ItemsSourceProperty, "SelectedItem", "SelectionChanged").ApplyBinding =
                (viewModelType, path, property, element, convention) =>
                {
                    if (ConventionManager
                        .GetElementConvention(typeof(ItemsControl))
                        .ApplyBinding(viewModelType, path, property, element, convention))
                    {
                        ConventionManager
                            .ConfigureSelectedItem(element, Pivot.SelectedItemProperty, viewModelType, path);
                        ConventionManager
                            .ApplyHeaderTemplate(element, Pivot.HeaderTemplateProperty, viewModelType);
                        return true;
                    }

                    return false;
                };

            ConventionManager.AddElementConvention<Panorama>(Panorama.ItemsSourceProperty, "SelectedItem", "SelectionChanged").ApplyBinding =
                (viewModelType, path, property, element, convention) =>
                {
                    if (ConventionManager
                        .GetElementConvention(typeof(ItemsControl))
                        .ApplyBinding(viewModelType, path, property, element, convention))
                    {
                        ConventionManager
                            .ConfigureSelectedItem(element, Panorama.SelectedItemProperty, viewModelType, path);
                        ConventionManager
                            .ApplyHeaderTemplate(element, Panorama.HeaderTemplateProperty, viewModelType);
                        return true;
                    }

                    return false;
                };
        }

        protected override object GetInstance(Type service, string key)
        {
            return this.container.GetInstance(service, key);
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return this.container.GetAllInstances(service);
        }

        protected override void BuildUp(object instance)
        {
            this.container.BuildUp(instance);
        }
    }
}
