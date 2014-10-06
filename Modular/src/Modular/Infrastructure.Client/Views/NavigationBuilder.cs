namespace Infrastructure.Views
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Windows;
    using System.Windows.Input;
    using Infrastructure.Commands;
    using Infrastructure.I18n;
    using Infrastructure.Options;
    using Infrastructure.Reflection;
    using Infrastructure.ViewModels;
    using Microsoft.Practices.Prism;
    using Microsoft.Practices.Prism.Regions;

    public class NavigationBuilder
    {
        private readonly List<KeyValuePair<string, string>> parameters;
        private readonly IRegionManager regionManager;
        private ViewModel viewModel;
        private Action<IOptionsChanged<IOptions>, NavigationViewModel> handleOptionsChanged;

        public NavigationBuilder(IRegionManager regionManager)
        {
            Contract.Requires(regionManager != null);

            this.regionManager = regionManager;
            this.parameters = new List<KeyValuePair<string, string>>();
        }

        public static implicit operator NavigationView(NavigationBuilder navigate)
        {
            Contract.Requires(navigate != null);

            return navigate.Build();
        }

        public NavigationBuilder ToView(FrameworkElement view)
        {
            Contract.Requires(view != null);
            Contract.Requires(view.DataContext != null);
            Contract.Requires(view.DataContext is ViewModel);

            this.viewModel = (ViewModel)view.DataContext;

            return this;
        }

        public NavigationBuilder HideOn(Option option)
        {
            Action<IOptionsChanged<IOptions>, NavigationViewModel> onOptionsChanged =
                (msg, vm) =>
                {
                    Option option1 = option;
                    if (msg.OptionName != option1)
                    {
                        return;
                    }

                    object value = msg.Options[option1];

                    if (value is bool)
                    {
                        bool isEnabled = (bool)value;

                        if (isEnabled)
                        {
                            vm.Show();
                        }
                        else
                        {
                            vm.Hide();
                        }
                    }
                };

            this.handleOptionsChanged = onOptionsChanged;

            return this;
        }

        public NavigationView Build()
        {
            ICommand navigationCommand = new NavigateContentCommand(this.regionManager);

            Type viewModelType = this.viewModel.GetType();

            ResourceAccessor resource;
            DynamicListViewModel listView = this.viewModel as DynamicListViewModel;
            if (listView == null)
            {
                string module = ReflectionHelper.GetDefaultNamespace(viewModelType.Assembly);
                string titleResourceKey = module + "." + viewModelType.Name.Replace("ViewModel", string.Empty);
                resource = ResourceAccessor.Create(titleResourceKey);
            }
            else
            {
                resource = ResourceAccessor.Create(listView.ListViewId.ModuleQualifiedListViewName);
                this.parameters.Add(new KeyValuePair<string, string>("name", listView.ListViewId.ToString()));
            }

            UriQuery query = new UriQuery();

            foreach (var parameter in this.parameters)
            {
                query.Add(parameter.Key, parameter.Value);
            }

            NavigationViewModel navigationViewModel = new NavigationViewModel(
                navigationCommand,
                resource,
                new Uri(viewModelType.Name.Replace("ViewModel", "View") + query, UriKind.Relative),
                this.handleOptionsChanged)
                {
                    EventAggregator = this.viewModel.EventAggregator
                };

            this.viewModel.EventAggregator.Subscribe(navigationViewModel);

            NavigationView view = new NavigationView { DataContext = navigationViewModel };

            ViewModelBinder.Bind(view, navigationViewModel);

            return view;
        }
    }
}