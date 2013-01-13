using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;

using TecX.Common;

namespace TecX.Agile.View.Behavior
{
    public abstract class ItemBehavior : DependencyObject
    {
        #region Properties

        /// <summary>
        /// This is a dependency property.
        /// </summary>
        public static readonly DependencyProperty AttachedHandlersProperty =
            DependencyProperty.RegisterAttached(
                "AttachedHandlers",
                typeof (IList<IBehaviorHandler>),
                typeof (ItemBehavior),
                new FrameworkPropertyMetadata(null));

        /// <summary>
        /// Setter for <see cref="DependencyProperty"/> <see cref="AttachedHandlersProperty"/>
        /// </summary>
        public static void SetAttachedHandlers(DependencyObject dependencyObject, IList<IBehaviorHandler> value)
        {
            Guard.AssertNotNull(dependencyObject, "dependencyObject");

            dependencyObject.SetValue(AttachedHandlersProperty, value);
        }

        /// <summary>
        /// Getter for <see cref="DependencyProperty"/> <see cref="AttachedHandlersProperty"/>
        /// </summary>
        public static IList<IBehaviorHandler> GetAttachedHandlers(DependencyObject dependencyObject)
        {
            Guard.AssertNotNull(dependencyObject, "dependencyObject");

            return (IList<IBehaviorHandler>) dependencyObject.GetValue(AttachedHandlersProperty);
        }

        #endregion Properties

        /// <summary>
        /// Called when [behavior enabled].
        /// </summary>
        /// <param name="dependencyObject">The dependency object.</param>
        /// <param name="args">The <see cref="System.Windows.DependencyPropertyChangedEventArgs"/> 
        /// instance containing the event data.</param>
        protected static void OnBehaviorEnabledChanged<THandler>(DependencyObject dependencyObject,
                                                                 DependencyPropertyChangedEventArgs args)
            where THandler : IBehaviorHandler, new()
        {
            //don't do anything when you are in the designer
            if (!DesignerProperties.GetIsInDesignMode(dependencyObject))
            {
                bool isEnabled = (bool) args.NewValue;
                bool oldValue = (bool) args.OldValue;

                //do nothing if the setting did not change
                if (isEnabled == oldValue)
                    return;

                FrameworkElement element = dependencyObject as FrameworkElement;

                if (element != null)
                {
                    var attachedHandlers = GetAttachedHandlers(element);

                    if(attachedHandlers == null)
                    {
                        attachedHandlers = new List<IBehaviorHandler>();

                        SetAttachedHandlers(element, attachedHandlers);
                    }

                    //when turned on
                    if (isEnabled)
                    {
                        var handler = attachedHandlers.SingleOrDefault(
                            x => typeof (THandler).IsAssignableFrom(x.GetType()));

                        //do nothing if a handler is already attached
                        if (handler != null)
                            return;

                        handler = new THandler();

                        handler.Attach(element);

                        attachedHandlers.Add(handler);
                    }
                    else
                    {
                        var handler = attachedHandlers.SingleOrDefault(
                            x => typeof (THandler).IsAssignableFrom(x.GetType()));

                        if (handler != null)
                        {
                            handler.Detach();

                            attachedHandlers.Remove(handler);
                        }
                    }
                }
            }
        }
    }
}