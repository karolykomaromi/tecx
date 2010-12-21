﻿using System.Windows;
using System.Windows.Controls;

using TecX.Common;

namespace TecX.Agile.View.Behavior
{
    public class TouchBehavior : MovementBehaviorBase
    {
        #region DependencyProperties

        /// <summary>
        /// This is a dependency property.
        /// </summary>
        public static readonly DependencyProperty IsEnabledProperty =
            DependencyProperty.RegisterAttached(
                "IsEnabled",
                typeof (bool),
                typeof (TouchBehavior),
                new FrameworkPropertyMetadata(
                    false,
                    FrameworkPropertyMetadataOptions.AffectsArrange |
                    FrameworkPropertyMetadataOptions.AffectsMeasure,
                    OnMovementBehaviorEnabledChanged<TouchHandler>));

        /// <summary>
        /// Setter for <see cref="DependencyProperty"/> <see cref="IsEnabledProperty"/>
        /// </summary>
        public static void SetIsEnabled(UserControl userControl, bool value)
        {
            Guard.AssertNotNull(userControl, "userControl");

            userControl.SetValue(IsEnabledProperty, value);
        }

        /// <summary>
        /// Getter for <see cref="DependencyProperty"/> <see cref="IsEnabledProperty"/>
        /// </summary>
        public static bool GetIsEnabled(UserControl userControl)
        {
            Guard.AssertNotNull(userControl, "userControl");

            return (bool) userControl.GetValue(IsEnabledProperty);
        }

        #endregion DependencyProperties
    }
}