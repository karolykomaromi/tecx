using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using Microsoft.Practices.Prism.Commands;

using TecX.Agile.Infrastructure;
using TecX.Agile.Infrastructure.Events;
using TecX.Agile.ViewModel;
using TecX.Common;
using TecX.Common.Event;

namespace TecX.Agile.View.Behavior
{
    public class HighlightFieldHandler : BehaviorHandler
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly string _fieldName;
        private Guid _artefactId;
        private TextBox _textBox;

        private readonly DelegateCommand<FieldHighlighted> _highlightFieldCommand;
        private readonly DelegateCommand<CaretMoved> _moveCaretCommand;

        #region Overrides of BehaviorHandler

        public HighlightFieldHandler(IEventAggregator eventAggregator, string fieldName)
        {
            Guard.AssertNotNull(eventAggregator, "eventAggregator");
            Guard.AssertNotEmpty(fieldName, "fieldName");

            _eventAggregator = eventAggregator;
            _fieldName = fieldName;

            _highlightFieldCommand = new DelegateCommand<FieldHighlighted>(message =>
                                            {
                                                if (message.ArtefactId == _artefactId &&
                                                    message.FieldName == _fieldName)
                                                {
                                                    _textBox.Focus();
                                                }
                                            });

            _moveCaretCommand = new DelegateCommand<CaretMoved>(message =>
                                            {
                                                if (message.ArtefactId == _artefactId &&
                                                    message.FieldName == _fieldName)
                                                {
                                                    _textBox.Select(message.CaretIndex, 0);
                                                }
                                            });
        }

        protected override void DoAttach(FrameworkElement element)
        {
            TextBox textBox = element as TextBox;

            if (textBox == null)
                return;

            //TODO weberse 2010-12-27 extract from datacontext of parent usercontrol
            _artefactId = Guid.Empty;

            _textBox = textBox;

            Element.GotFocus += OnGotFocus;
            Element.KeyUp += OnKeyUp;

            Commands.HighlightField.RegisterCommand(_highlightFieldCommand);
            Commands.MoveCaret.RegisterCommand(_moveCaretCommand);
        }

        #endregion

        private void OnGotFocus(object sender, RoutedEventArgs e)
        {
            _eventAggregator.Publish(new FieldHighlighted(_artefactId, _fieldName));
        }

        protected override void DoDetach()
        {
            _artefactId = Guid.Empty;
            _textBox = null;

            Element.GotFocus -= OnGotFocus;
            Element.KeyUp -= OnKeyUp;

            Commands.HighlightField.UnregisterCommand(_highlightFieldCommand);
            Commands.MoveCaret.UnregisterCommand(_moveCaretCommand);
        }

        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            FrameworkElement focusedElement = FocusManager.GetFocusedElement() as FrameworkElement;

            if (focusedElement != null &&
                focusedElement == _textBox)
            {
                _eventAggregator.Publish(new CaretMoved(_artefactId, _fieldName, _textBox.SelectionStart));
            }
        }
    }
}