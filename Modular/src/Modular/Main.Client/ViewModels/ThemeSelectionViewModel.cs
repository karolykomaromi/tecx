﻿namespace Main.ViewModels
{
    using System;
    using System.Collections.ObjectModel;
    using System.Diagnostics.Contracts;
    using System.Windows.Input;
    using Infrastructure.ViewModels;

    public class ThemeSelectionViewModel : ViewModel
    {
        private readonly ObservableCollection<Uri> availableThemes;
        private readonly ICommand changeThemeCommand;
        private Uri selectedTheme;

        public ThemeSelectionViewModel(ICommand changeThemeCommand)
        {
            Contract.Requires(changeThemeCommand != null);

            this.changeThemeCommand = changeThemeCommand;
            this.availableThemes = new ObservableCollection<Uri>
                {
                    new Uri("/Infrastructure.Client;component/Assets/Themes/DefaultTheme.xaml", UriKind.Relative),
                    new Uri("/Infrastructure.Client;component/Assets/Themes/BlueTheme.xaml", UriKind.Relative),
                };

            this.selectedTheme = this.availableThemes[0];
        }

        public Uri SelectedTheme
        {
            get
            {
                return this.selectedTheme;
            }

            set
            {
                if (this.selectedTheme != value)
                {
                    this.OnPropertyChanging(() => this.SelectedTheme);
                    this.selectedTheme = value;
                    this.OnPropertyChanged(() => this.SelectedTheme);
                }
            }
        }

        public ObservableCollection<Uri> AvailableThemes
        {
            get { return this.availableThemes; }
        }

        public ICommand ChangeThemeCommand
        {
            get { return this.changeThemeCommand; }
        }
    }
}
