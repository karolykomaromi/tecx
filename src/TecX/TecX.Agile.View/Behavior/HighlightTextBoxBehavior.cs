using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

using TecX.Common;
using TecX.Common.Event;

namespace TecX.Agile.View.Behavior
{
    public class HighlightTextBoxBehavior : ItemBehavior
    {
        #region Properties

        /// <summary>
        /// This is a dependency property.
        /// </summary>
        public static readonly DependencyProperty IsEnabledProperty =
            DependencyProperty.RegisterAttached(
                "IsEnabled",
                typeof (bool),
                typeof (HighlightTextBoxBehavior),
                new FrameworkPropertyMetadata(false, OnBehaviorEnabledChanged));

        /// <summary>
        /// Setter for <see cref="DependencyProperty"/> <see cref="IsEnabledProperty"/>
        /// </summary>
        public static void SetIsEnabled(TextBox textBox, bool value)
        {
            Guard.AssertNotNull(textBox, "textBox");

            textBox.SetValue(IsEnabledProperty, value);
        }

        /// <summary>
        /// Getter for <see cref="DependencyProperty"/> <see cref="IsEnabledProperty"/>
        /// </summary>
        public static bool GetIsEnabled(TextBox textBox)
        {
            Guard.AssertNotNull(textBox, "textBox");

            return (bool) textBox.GetValue(IsEnabledProperty);
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Called when [behavior enabled].
        /// </summary>
        /// <param name="dependencyObject">The dependency object.</param>
        /// <param name="args">The <see cref="System.Windows.DependencyPropertyChangedEventArgs"/> 
        /// instance containing the event data.</param>
        private static void OnBehaviorEnabledChanged(DependencyObject dependencyObject,
            DependencyPropertyChangedEventArgs args)
        {
            //don't do anything when you are in the designer
            if (!DesignerProperties.GetIsInDesignMode(dependencyObject))
            {
                bool isEnabled = (bool)args.NewValue;
                bool oldValue = (bool)args.OldValue;

                //do nothing if the setting did not change
                if (isEnabled == oldValue)
                    return;

                FrameworkElement element = dependencyObject as FrameworkElement;

                if (element != null)
                {
                    var attachedHandlers = GetAttachedHandlers(element);

                    if (attachedHandlers == null)
                    {
                        attachedHandlers = new List<IBehaviorHandler>();

                        SetAttachedHandlers(element, attachedHandlers);
                    }

                    //when turned on
                    if (isEnabled)
                    {
                        var handler = attachedHandlers.SingleOrDefault(
                            x => typeof(HighlightTextBoxHandler).IsAssignableFrom(x.GetType()));

                        //do nothing if a handler is already attached
                        if (handler != null)
                            return;

                        //create a new handler that listens to incoming requests via EA
                        //and publishes outgoing notifications via EA
                        IEventAggregator eventAggregator = EventAggregatorAccessor.EventAggregator;

                        handler = new HighlightTextBoxHandler(eventAggregator);

                        handler.Attach(element);

                        attachedHandlers.Add(handler);
                    }
                    else
                    {
                        var handler = attachedHandlers.SingleOrDefault(
                            x => typeof(HighlightTextBoxHandler).IsAssignableFrom(x.GetType()));

                        if (handler != null)
                        {
                            handler.Detach();

                            attachedHandlers.Remove(handler);
                        }
                    }
                }
            }
        }

        #endregion Methods
    }
}