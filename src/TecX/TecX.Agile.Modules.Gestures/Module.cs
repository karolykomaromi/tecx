using System;
using System.Diagnostics;
using System.Windows;

using TecX.Agile.Infrastructure;
using TecX.Agile.Modules.Gestures.ViewModels;
using TecX.Common;

namespace TecX.Agile.Modules.Gestures
{
    [DebuggerDisplay("{Description}")]
    public class Module : IModule
    {
        private readonly GestureViewModel _gestureViewModel;

        public string Description
        {
            get
            {
                return "Gesture Recognition";
            }
        }

        public Module(GestureViewModel gestureViewModel)
        {
            Guard.AssertNotNull(gestureViewModel, "gestureViewModel");

            _gestureViewModel = gestureViewModel;
        }

        public void Initialize()
        {
            ResourceDictionary dictionary = new ResourceDictionary
                {
                    //Source = new Uri("pack://application:,,,/GestureStyles.xaml")
                    Source = new Uri("TecX.Agile.Modules.Gestures;component/GestureStyles.xaml", UriKind.RelativeOrAbsolute)
                };

            Application.Current.Resources.MergedDictionaries.Add(dictionary);
        }
    }
}
