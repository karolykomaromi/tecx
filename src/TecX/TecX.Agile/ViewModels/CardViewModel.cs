namespace TecX.Agile.ViewModels
{
    using System;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Windows.Media;

    using Caliburn.Micro;

    using TecX.Agile.Infrastructure.Commands;
    using TecX.Agile.Infrastructure.Events;
    using TecX.Common;
    using TecX.Common.Comparison;
    using TecX.Event;

    using IEventAggregator = TecX.Event.IEventAggregator;

    public abstract class CardViewModel : Screen, ISubscribeTo<ChangeProperty>
    {
        #region Fields

        private readonly IEventAggregator eventAggregator;

        private Color background;

        private double x;

        private double y;

        private double angle;

        private double width;

        private double height;

        private Guid id;

        private bool isPinned;

        private double scale;

        private double opacity;

        #endregion Fields

        protected CardViewModel(IEventAggregator eventAggregator)
        {
            Guard.AssertNotNull(eventAggregator, "eventAggregator");

            this.eventAggregator = eventAggregator;

            this.EventAggregator.Subscribe(this);
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
                if (EpsilonComparer.AreEqual(this.x, value))
                {
                    return;
                }

                ////this.x = value;
                ////this.NotifyOfPropertyChange(() => this.X);
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
                if (EpsilonComparer.AreEqual(this.y, value))
                {
                    return;
                }

                ////this.y = value;
                ////this.NotifyOfPropertyChange(() => this.AbsoluteY);
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
                if (EpsilonComparer.AreEqual(this.angle, value))
                {
                    return;
                }

                ////this.angle = value;
                ////this.NotifyOfPropertyChange(() => this.Angle);
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
                if (this.id == value)
                {
                    return;
                }

                this.id = value;
                this.NotifyOfPropertyChange(() => this.Id);
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
                if (this.isPinned == value)
                {
                    return;
                }

                this.isPinned = value;
                this.NotifyOfPropertyChange(() => this.IsPinned);
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
                if (EpsilonComparer.AreEqual(this.width, value))
                {
                    return;
                }

                this.width = value;
                this.NotifyOfPropertyChange(() => this.Width);
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
                if (EpsilonComparer.AreEqual(this.height, value))
                {
                    return;
                }

                this.height = value;
                this.NotifyOfPropertyChange(() => this.Height);
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
                if (EpsilonComparer.AreEqual(this.scale, value))
                {
                    return;
                }

                this.scale = value;
                this.NotifyOfPropertyChange(() => this.Scale);
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
                if (EpsilonComparer.AreEqual(this.opacity, value))
                {
                    return;
                }

                this.opacity = value;
                this.NotifyOfPropertyChange(() => this.Opacity);
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
                if (this.background == value)
                {
                    return;
                }

                this.background = value;
                this.NotifyOfPropertyChange(() => this.Background);
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
                var currentValue = field.GetValue(this);

                // TODO weberse 2011-12-19 should use EpsilonComparer for double values
                if (!Equals(currentValue, value))
                {
                    string propertyName = this.ToPropertyName(field.Name);

                    var msg = new PropertyChanged(this.Id, propertyName, currentValue, value);

                    field.SetValue(this, value);

                    NotifyOfPropertyChange(propertyName);

                    this.EventAggregator.Publish(msg);
                }
            }
        }

        public void Handle(ChangeProperty message)
        {
            Guard.AssertNotNull(message, "message");

            var property = typeof(CardViewModel).GetProperty(
                message.PropertyName, BindingFlags.Instance | BindingFlags.Public);

            if (property != null && 
                property.PropertyType.IsAssignableFrom(message.To.GetType()))
            {
                property.SetValue(this, message.To, null);
            }
        }

        private string ToPropertyName(string fieldName)
        {
            Guard.AssertNotEmpty(fieldName, "fieldName");

            string propertyName = fieldName.Substring(0, 1).ToUpper() + fieldName.Substring(1);

            return propertyName;
        }
    }
}
