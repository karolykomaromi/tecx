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
    public class HighlightFieldBehavior : System.Windows.Interactivity.Behavior<TextBox>
    {
        #region DependencyProperties

        public static readonly DependencyProperty IsEnabledProperty = DependencyProperty.RegisterAttached(
            "IsEnabled",
            typeof (bool),
            typeof (HighlightFieldBehavior),
            new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.None, OnEnabledChanged));

        public static bool GetIsEnabled(TextBox tb)
        {
            Guard.AssertNotNull(tb, "tb");

            return (bool) tb.GetValue(IsEnabledProperty);
        }

        public static void SetIsEnabled(TextBox tb, bool isEnabled)
        {
            Guard.AssertNotNull(tb, "tb");

            tb.SetValue(IsEnabledProperty, isEnabled);
        }

        private static void OnEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
                return;

            TextBox tb = d as TextBox;

            if (tb == null)
                return;

            var behaviors = Interaction.GetBehaviors(tb);

            if (e.NewValue is bool == false)
            {
                foreach (var pfb in behaviors.ToArray().OfType<HighlightFieldBehavior>())
                {
                    behaviors.Remove(pfb);
                }
            }
            else
            {
                var behavior = new HighlightFieldBehavior();

                behaviors.Add(behavior);
            }
        }

        #endregion DependencyProperties

        #region Fields

        private Guid _id;
        private string _fieldName;

        private readonly IEventAggregator _eventAggregator;

        private readonly DelegateCommand<FieldHighlighted> _highlightFieldCommand;
        private readonly DelegateCommand<CaretMoved> _moveCaretCommand;

        #endregion Fields

        #region c'tor
        
        public HighlightFieldBehavior()
        {
            _eventAggregator = EventAggregatorAccessor.EventAggregator;

            _highlightFieldCommand = new DelegateCommand<FieldHighlighted>(message =>
                                                                               {
                                                                                   if (message.ArtefactId == _id &&
                                                                                       message.FieldName == _fieldName)
                                                                                   {
                                                                                       AssociatedObject.Focus();
                                                                                   }
                                                                               });

            _moveCaretCommand = new DelegateCommand<CaretMoved>(message =>
                                                                    {
                                                                        if (message.ArtefactId == _id &&
                                                                            message.FieldName == _fieldName)
                                                                        {
                                                                            AssociatedObject.CaretIndex = message.CaretIndex;
                                                                        }
                                                                    });
        }
        
        #endregion c'tor

        #region Overrides of BehaviorHandler

        protected override void OnAttached()
        {
            base.OnAttached();

            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
                return;

            UserControl ctrl;
            string fieldName;
            if (UIHelper.TryFindAncestor(AssociatedObject, out ctrl) &&
                UIHelper.TryGetName(ctrl, AssociatedObject, out fieldName))
            {
                //TODO weberse if ctrl.DataContext == null should hook up a handler to ctrl.DataContextChanged and
                //act a soon as the context is available. hook up handlers and remove listener to DataContextChanged
                PlanningArtefact artefact = ctrl.DataContext as PlanningArtefact;

                if (artefact != null)
                {
                    _id = artefact.Id;
                }

                ctrl.DataContextChanged += (s, e) =>
                                               {
                                                   artefact = ctrl.DataContext as PlanningArtefact;

                                                   if (artefact != null)
                                                   {
                                                       _id = artefact.Id;
                                                   }
                                               };

                _fieldName = fieldName;

                AssociatedObject.GotFocus += OnGotFocus;
                AssociatedObject.KeyUp += OnKeyUp;

                Commands.HighlightField.RegisterCommand(_highlightFieldCommand);
                Commands.MoveCaret.RegisterCommand(_moveCaretCommand);
            }
        }

        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            if (AssociatedObject.IsFocused)
            {
                _eventAggregator.Publish(new CaretMoved(_id, _fieldName, AssociatedObject.CaretIndex));
            }
        }

        //TODO weberse this would be a perfect little hook to add textbox focus
        //notification at the top level. just hook up the behavior to a story card and that
        //would attach handlers to all textboxes inside that story card
        //private void OnLoaded(object sender, RoutedEventArgs e)
        //{
        //    UserControl ctrl = sender as UserControl;

        //    var textBoxes = TreeHelper.FindChildren<TextBox>(ctrl);

        //    int count = textBoxes.Count();
        //}

        private void OnGotFocus(object sender, RoutedEventArgs e)
        {
            _eventAggregator.Publish(new FieldHighlighted(_id, _fieldName));
        }

        protected override void OnDetaching()
        {
            AssociatedObject.GotFocus -= OnGotFocus;

            _fieldName = null;
            _id = Guid.Empty;

            Commands.HighlightField.UnregisterCommand(_highlightFieldCommand);
            Commands.MoveCaret.UnregisterCommand(_moveCaretCommand);
        }

        #endregion
    }
}