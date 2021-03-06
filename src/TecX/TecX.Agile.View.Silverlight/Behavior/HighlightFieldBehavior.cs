﻿using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;

using Microsoft.Practices.Prism.Commands;

using TecX.Agile.Infrastructure;
using TecX.Agile.Infrastructure.Events;
using TecX.Agile.ViewModel;
using TecX.Common;
using TecX.Common.Event;

namespace TecX.Agile.View.Behavior
{
    public class HighlightFieldBehavior : Behavior<TextBox>
    {
        #region Fields

        private readonly IEventAggregator _eventAggregator;

        private Guid _artefactId;

        private readonly DelegateCommand<FieldHighlighted> _highlightFieldCommand;
        private readonly DelegateCommand<CaretMoved> _moveCaretCommand;

        #endregion Fields

        #region Properties

        public string UniqueFieldName { get; set; }

        #endregion Properties

        #region Dependency Properties

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

        #endregion Dependency Properties

        #region c'tor

        public HighlightFieldBehavior()
        {
            _eventAggregator = EventAggregatorAccessor.EventAggregator;

            _highlightFieldCommand = new DelegateCommand<FieldHighlighted>(message =>
            {
                if (message.ArtefactId == _artefactId &&
                    message.FieldName == UniqueFieldName &&
                    FocusManager.GetFocusedElement() != AssociatedObject)
                {
                    bool success = AssociatedObject.Focus();
                }
            });

            _moveCaretCommand = new DelegateCommand<CaretMoved>(message =>
            {
                if (message.ArtefactId == _artefactId &&
                    message.FieldName == UniqueFieldName &&
                    message.CaretIndex != AssociatedObject.SelectionStart)
                {
                    AssociatedObject.Select(message.CaretIndex, 0);
                }
            });
        }

        #endregion c'tor

        #region Overrides of Behavior<T>

        protected override void OnAttached()
        {
            base.OnAttached();

            AssociatedObject.Loaded += OnLoaded;

            AssociatedObject.GotFocus += OnGotFocus;
            AssociatedObject.KeyUp += OnKeyUp;

            Commands.HighlightField.RegisterCommand(_highlightFieldCommand);
            Commands.MoveCaret.RegisterCommand(_moveCaretCommand);
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            UserControl parent;
            PlanningArtefact pa;
            if (UIHelper.TryFindAncestor(AssociatedObject, out parent) &&
                (pa = parent.DataContext as PlanningArtefact) != null)
            {
                _artefactId = pa.Id;
            }
            else
            {
                throw new InvalidOperationException("Can't attach this behavior to a control whose parent " +
                    "UserControl does not have a PlanningArtefact as its DataContext");
            }

            AssociatedObject.Loaded -= OnLoaded;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();

            _artefactId = Guid.Empty;

            AssociatedObject.GotFocus -= OnGotFocus;
            AssociatedObject.KeyUp -= OnKeyUp;

            Commands.HighlightField.UnregisterCommand(_highlightFieldCommand);
            Commands.MoveCaret.UnregisterCommand(_moveCaretCommand);
        }

        #endregion Overrides of Behavior<T>

        #region EventHandling

        private void OnGotFocus(object sender, RoutedEventArgs e)
        {
#if DEBUG
            lock (_lock)
            {
                if (IsLastFocused(AssociatedObject))
                    return;
            }
#endif

            _eventAggregator.Publish(new FieldHighlighted(_artefactId, UniqueFieldName));

#if DEBUG
            RunLocked(() => SetLastFocused(AssociatedObject));
#endif
        }

        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            FrameworkElement focusedElement = FocusManager.GetFocusedElement() as FrameworkElement;

            if (focusedElement != null &&
                focusedElement == AssociatedObject)
            {
                _eventAggregator.Publish(new CaretMoved(_artefactId, UniqueFieldName, AssociatedObject.SelectionStart));
            }
        }

        private static void OnFieldNameChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (DesignerProperties.GetIsInDesignMode(d))
                return;

            string fieldName = e.NewValue as string;
            TextBox textBox = d as TextBox;

            if (fieldName != null &&
                textBox != null)
            {
                BehaviorCollection behaviors = Interaction.GetBehaviors(d);

                var behavior = behaviors.OfType<HighlightFieldBehavior>().SingleOrDefault();

                //do nothing if a handler is already attached
                if (behavior != null)
                    return;

                behavior = new HighlightFieldBehavior { UniqueFieldName = fieldName };

                behaviors.Add(behavior);
            }
        }

        #endregion EventHandling

#if DEBUG
        private static readonly object _lock = new object();

        private static void RunLocked(Action action)
        {
            lock (_lock)
            {
                action();
            }
        }

        private static WeakReference _lastFocused;

        private static void SetLastFocused(TextBox textBox)
        {
            _lastFocused = new WeakReference(textBox);
        }

        private static bool IsLastFocused(TextBox textBox)
        {
            lock (_lock)
            {
                if (_lastFocused != null &&
                    _lastFocused.IsAlive)
                {
                    return ReferenceEquals(_lastFocused.Target, textBox);
                }

                return false;
            }
        }
#endif
    }
}
