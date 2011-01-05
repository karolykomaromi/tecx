using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

using TecX.Agile.Infrastructure;
using TecX.Common;

namespace TecX.Agile.View.Behavior
{
    public class HighlightFieldBehavior : BehaviorBase
    {
        /// <summary>
        /// This is a dependency property.
        /// </summary>
        public static readonly DependencyProperty FieldNameProperty =
            DependencyProperty.RegisterAttached(
                "FieldName",
                typeof(string),
                typeof(HighlightFieldBehavior),
                new PropertyMetadata(null, OnFieldNameChanged));

        /// <summary>
        /// Setter for <see cref="DependencyProperty"/> <see cref="FieldNameProperty"/>
        /// </summary>
        public static void SetFieldName(TextBox textBox, string fieldName)
        {
            Guard.AssertNotNull(textBox, "textBox");

            textBox.SetValue(FieldNameProperty, fieldName);
        }

        /// <summary>
        /// Getter for <see cref="DependencyProperty"/> <see cref="FieldNameProperty"/>
        /// </summary>
        public static string GetFieldName(TextBox textBox)
        {
            Guard.AssertNotNull(textBox, "textBox");

            return (string)textBox.GetValue(FieldNameProperty);
        }


        private static void OnFieldNameChanged(DependencyObject dependencyObject,
            DependencyPropertyChangedEventArgs e)
        {
            Guard.AssertNotNull(dependencyObject, "dependencyObject");
            Guard.AssertNotNull(e, "e");

            if (!DesignerProperties.GetIsInDesignMode(dependencyObject))
            {
                string fieldName = e.NewValue as string;
                TextBox textBox = dependencyObject as TextBox;

                if (fieldName != null &&
                    textBox != null)
                {
                    var attachedHandlers = GetAttachedHandlers(textBox);

                    if (attachedHandlers == null)
                    {
                        attachedHandlers = new List<IBehaviorHandler>();

                        SetAttachedHandlers(textBox, attachedHandlers);
                    }

                    var handler = attachedHandlers.OfType<HighlightFieldHandler>().SingleOrDefault();

                    //do nothing if a handler is already attached
                    if (handler != null)
                        return;

                    handler = new HighlightFieldHandler(EventAggregatorAccessor.EventAggregator, 
                        fieldName);

                    handler.Attach(textBox);

                    attachedHandlers.Add(handler);
                }
            }
        }
    }
}
