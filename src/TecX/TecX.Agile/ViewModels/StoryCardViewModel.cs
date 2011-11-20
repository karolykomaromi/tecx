namespace TecX.Agile.ViewModels
{
    using System;
    using System.Windows.Media;

    using Caliburn.Micro;

    public class StoryCardViewModel : Screen
    {
        #region Fields

        private readonly IObservableCollection<object> decorators;

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

        private string name;

        private double mostLikelyEstimate;

        private double actualEffort;

        private string taskOwner;

        private bool isHandWritingEnabled;

        private string description;

        #endregion Fields

        public StoryCardViewModel()
        {
            this.Width = Constants.StoryCard.Width;
            this.Height = Constants.StoryCard.Height;
            this.Opacity = Constants.Opacity;

            this.Background = Constants.Colors.Yellow;

            this.decorators = new BindableCollection<object>();
        }

        #region Properties

        public IObservableCollection<object> Decorators
        {
            get
            {
                return this.decorators;
            }
        }

        public double X
        {
            get
            {
                return this.x;
            }

            set
            {
                if (this.x == value)
                {
                    return;
                }

                this.x = value;
                this.NotifyOfPropertyChange(() => this.X);
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
                if (this.y == value)
                {
                    return;
                }

                this.y = value;
                this.NotifyOfPropertyChange(() => this.Y);
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
                if (this.angle == value)
                {
                    return;
                }

                this.angle = value;
                this.NotifyOfPropertyChange(() => this.Angle);
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
                if (this.width == value)
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
                if (this.height == value)
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
                if (this.scale == value)
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
                if (this.opacity == value)
                {
                    return;
                }

                this.opacity = value;
                this.NotifyOfPropertyChange(() => this.Opacity);
            }
        }

        public string Name
        {
            get
            {
                return this.name;
            }

            set
            {
                if (this.name == value)
                {
                    return;
                }

                this.name = value;
                this.NotifyOfPropertyChange(() => this.Name);
            }
        }

        public double MostLikelyEstimate
        {
            get
            {
                return this.mostLikelyEstimate;
            }

            set
            {
                if (this.mostLikelyEstimate == value)
                {
                    return;
                }

                this.mostLikelyEstimate = value;
                this.NotifyOfPropertyChange(() => this.MostLikelyEstimate);
            }
        }

        public double ActualEffort
        {
            get
            {
                return this.actualEffort;
            }

            set
            {
                if (this.actualEffort == value)
                {
                    return;
                }

                this.actualEffort = value;
                this.NotifyOfPropertyChange(() => this.ActualEffort);
            }
        }

        public string TaskOwner
        {
            get
            {
                return this.taskOwner;
            }

            set
            {
                if (this.taskOwner == value)
                {
                    return;
                }

                this.taskOwner = value;
                this.NotifyOfPropertyChange(() => this.TaskOwner);
            }
        }

        public bool IsHandWritingEnabled
        {
            get
            {
                return this.isHandWritingEnabled;
            }

            set
            {
                if (this.isHandWritingEnabled == value)
                {
                    return;
                }

                this.isHandWritingEnabled = value;
                this.NotifyOfPropertyChange(() => this.IsHandWritingEnabled);
            }
        }

        public string Description
        {
            get
            {
                return this.description;
            }

            set
            {
                if (this.description == value)
                {
                    return;
                }

                this.description = value;
                this.NotifyOfPropertyChange(() => this.Description);
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

        #endregion Properties

        public void Move(double dx, double dy, double angle)
        {
            this.X += dx;
            this.Y += dy;
            this.Angle += angle;
        }
    }
}
