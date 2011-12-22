namespace TecX.Agile.ViewModels
{
    using System;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Windows.Media;

    using Caliburn.Micro;

    using TecX.Agile.Infrastructure;
    using TecX.Agile.Infrastructure.Commands;
    using TecX.Agile.Infrastructure.Events;
    using TecX.Agile.Messaging.Context;
    using TecX.Common;
    using TecX.Common.Comparison;
    using TecX.Event;

    using IEventAggregator = TecX.Event.IEventAggregator;

    public abstract class CardViewModel : Screen, ISubscribeTo<ChangeProperty>
    {
        #region Fields

        private Guid id;

        private double x;

        private double y;

        private double angle;

        private readonly IEventAggregator eventAggregator;

        private Color background;

        private double width;

        private double height;

        private bool isPinned;

        private double scale;

        private double opacity;

        #endregion Fields

        protected CardViewModel(IEventAggregator eventAggregator)
        {
            Guard.AssertNotNull(eventAggregator, "eventAggregator");

            this.eventAggregator = eventAggregator;

            this.EventAggregator.Subscribe(this);

            this.x = 0.0;
            this.y = 0.0;
            this.angle = 0.0;
            this.isPinned = false;
            this.scale = Defaults.Scale;
            this.width = Defaults.StoryCard.Width;
            this.height = Defaults.StoryCard.Height;
            this.opacity = Defaults.Opacity;
            this.background = Constants.Colors.Yellow;
        }

        #region Properties

        public double X
        {
            get
            {
                return this.x;
            }

            set
            {
                this.Set(() => this.x, value);
            }
        }

        public double Y
        {
            get
            {
                return this.y;
            }

            set
            {
                this.Set(() => this.y, value);
            }
        }

        public double Angle
        {
            get
            {
                return this.angle;
            }

            set
            {
                this.Set(() => this.angle, value);
            }
        }

        public Guid Id
        {
            get
            {
                return this.id;
            }

            set
            {
                this.Set(() => this.id, value);
            }
        }

        public bool IsPinned
        {
            get
            {
                return this.isPinned;
            }

            set
            {
                this.Set(() => this.isPinned, value);
            }
        }

        public double Width
        {
            get
            {
                return this.width;
            }

            set
            {
                this.Set(() => this.width, value);
            }
        }

        public double Height
        {
            get
            {
                return this.height;
            }

            set
            {
                this.Set(() => this.height, value);
            }
        }

        public double Scale
        {
            get
            {
                return this.scale;
            }

            set
            {
                this.Set(() => this.scale, value);
            }
        }

        public double Opacity
        {
            get
            {
                return this.opacity;
            }

            set
            {
                this.Set(() => this.opacity, value);
            }
        }

        public Color Background
        {
            get
            {
                return this.background;
            }

            set
            {
                this.Set(() => this.background, value);
            }
        }

        public IEventAggregator EventAggregator
        {
            get
            {
                return this.eventAggregator;
            }
        }

        #endregion Properties

        public void Move(double dx, double dy, double angle)
        {
            this.X += dx;
            this.Y += dy;
            this.Angle += angle;
        }

        public void Set<T>(Expression<Func<T>> fieldSelector, T value)
        {
            Guard.AssertNotNull(fieldSelector, "fieldSelector");

            var expr = (MemberExpression)fieldSelector.Body;

            var field = expr.Member as FieldInfo;

            if (field != null)
            {
                object currentValue = field.GetValue(this);

                bool areEqual = false;

                if(value is double)
                {
                    // TODO weberse 2011-12-22 how can I avoid two casts?
                    areEqual = EpsilonComparer.AreEqual((double)currentValue, (double)(object)value);
                }
                else
                {
                    areEqual = Equals(currentValue, value);
                }

                // TODO weberse 2011-12-19 should use EpsilonComparer for double values
                if (!areEqual)
                {
                    string propertyName = this.ToPropertyName(field.Name);

                    field.SetValue(this, value);

                    this.NotifyOfPropertyChange(propertyName);

                    PropertyChanged @event = new PropertyChanged(this.Id, propertyName, currentValue, value);

                    this.EventAggregator.Publish(@event);
                }
            }
        }

        public void Handle(ChangeProperty message)
        {
            Guard.AssertNotNull(message, "message");
            Guard.AssertNotEmpty(message.PropertyName, "message.PropertyName");

            var property = this.GetType().GetProperty(
                message.PropertyName, BindingFlags.Instance | BindingFlags.Public);

            Type propertyType;

            if (message.To != null)
            {
                propertyType = message.To.GetType();
            }
            else if (message.From != null)
            {
                propertyType = message.From.GetType();
            }
            else
            {
                return;
            }

            if (property != null && property.PropertyType.IsAssignableFrom(propertyType))
            {
                property.SetValue(this, message.To, null);
            }
        }

        protected void Initialize(Guid id, double x, double y, double angle)
        {
            this.id = id;
            this.x = x;
            this.y = y;
            this.angle = angle;
        }

        private string ToPropertyName(string fieldName)
        {
            Guard.AssertNotEmpty(fieldName, "fieldName");

            string propertyName = fieldName.Substring(0, 1).ToUpper() + fieldName.Substring(1);

            return propertyName;
        }
    }
}