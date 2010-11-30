﻿using System;
using System.Windows;
using System.Windows.Controls;

using TecX.Common;

namespace TecX.Agile.View
{
    public static class Tabletop
    {
        #region Properties

        /// <summary>
        /// This is a dependency property.
        /// </summary>
        public static readonly DependencyProperty IsSurfaceProperty =
            DependencyProperty.RegisterAttached(
                "IsSurface",
                typeof(bool),
                typeof(Tabletop),
                new FrameworkPropertyMetadata(
                    false,
                    OnDependencyPropertyChanged));

        private static void OnDependencyPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
        {
            if ((bool)args.NewValue)
            {
                Canvas canvas = dependencyObject as Canvas;

                if (canvas != null)
                    Surface = canvas;
            }
        }

        /// <summary>
        /// Setter for <see cref="DependencyProperty"/> <see cref="IsSurfaceProperty"/>
        /// </summary>
        public static void SetIsSurface(Canvas canvas, bool value)
        {
            Guard.AssertNotNull(canvas, "canvas");

            canvas.SetValue(IsSurfaceProperty, value);
        }

        /// <summary>
        /// Getter for <see cref="DependencyProperty"/> <see cref="IsSurfaceProperty"/>
        /// </summary>
        public static bool GetIsEnabled(Canvas canvas)
        {
            Guard.AssertNotNull(canvas, "canvas");

            return (bool)canvas.GetValue(IsSurfaceProperty);
        }

        private static Canvas _surface;

        public static Canvas Surface
        {
            get
            {
                if(_surface == null)
                {
                    throw new InvalidOperationException(
                        "Surface not initialized. Either set the property directly or use" +
                        "use the Tabletop.IsSurfaceProperty DependencyProperty to declare a Canvas as the default drawing plain.");
                }

                return _surface;
            }
            set
            {
                Guard.AssertNotNull(value, "value");

                _surface = value;
            }
        }

        #endregion Properties

    }
}