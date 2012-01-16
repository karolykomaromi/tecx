namespace TecX.Agile.Phone
{
    using System;
    using System.Collections.Generic;
    using System.ServiceModel;
    using System.ServiceModel.Channels;
    using System.Windows.Controls;

    using Caliburn.Micro;

    using Microsoft.Phone.Controls;

    using TecX.Agile.Phone.Data;
    using TecX.Agile.Phone.Service;
    using TecX.Agile.Phone.ViewModels;

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

            Binding binding = new BasicHttpBinding(BasicHttpSecurityMode.None);
            EndpointAddress remoteAddress = new EndpointAddress("http://localhost/phone/project");

            this.container.RegisterInstance(typeof(IAsyncProjectService), null, new ProjectServiceClient(binding, remoteAddress));

            AddCustomConventions();

            LogManager.GetLog = type => new DebugLog(type);
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
