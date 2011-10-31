using System;
using System.Windows.Media;

using Caliburn.Micro;

namespace TecX.Agile.ViewModels
{
    public class StoryCardViewModel : Screen
    {
        #region Fields

        private readonly IObservableCollection<object> _decorators;

        private Color _background;

        private double _x;

        private double _y;

        private double _angle;

        private double _width;

        private double _height;

        private Guid _id;

        private bool _isPinned;

        private double _scale;

        private double _opacity;

        private string _name;

        private double _mostLikelyEstimate;

        private double _actualEffort;

        private string _taskOwner;

        private bool _isHandWritingEnabled;

        private string _description;

        #endregion Fields

        public StoryCardViewModel()
        {
            Width = Constants.StoryCard.Width;
            Height = Constants.StoryCard.Height;
            Opacity = Constants.Opacity;

            Background = Constants.Colors.Yellow;

            _decorators = new BindableCollection<object>();
        }

        #region Properties

        public IObservableCollection<object> Decorators
        {
            get
            {
                return _decorators;
            }
        }

        public double X
        {
            get
            {
                return _x;
            }

            set
            {
                if (_x == value)
                {
                    return;
                }

                _x = value;
                NotifyOfPropertyChange(() => X);
            }
        }

        public double Y
        {
            get
            {
                return _y;
            }

            set
            {
                if (_y == value)
                {
                    return;
                }

                _y = value;
                NotifyOfPropertyChange(() => Y);
            }
        }

        public double Angle
        {
            get
            {
                return _angle;
            }

            set
            {
                if (_angle == value)
                {
                    return;
                }

                _angle = value;
                NotifyOfPropertyChange(() => Angle);
            }
        }

        public Guid Id
        {
            get
            {
                return _id;
            }

            set
            {
                if (_id == value)
                {
                    return;
                }

                _id = value;
                NotifyOfPropertyChange(() => Id);
            }
        }

        public bool IsPinned
        {
            get
            {
                return _isPinned;
            }

            set
            {
                if (_isPinned == value)
                {
                    return;
                }

                _isPinned = value;
                NotifyOfPropertyChange(() => IsPinned);
            }
        }

        public double Width
        {
            get
            {
                return _width;
            }

            set
            {
                if (_width == value)
                {
                    return;
                }

                _width = value;
                NotifyOfPropertyChange(() => Width);
            }
        }

        public double Height
        {
            get
            {
                return _height;
            }

            set
            {
                if (_height == value)
                {
                    return;
                }

                _height = value;
                NotifyOfPropertyChange(() => Height);
            }
        }

        public double Scale
        {
            get
            {
                return _scale;
            }

            set
            {
                if (_scale == value)
                {
                    return;
                }

                _scale = value;
                NotifyOfPropertyChange(() => Scale);
            }
        }

        public double Opacity
        {
            get
            {
                return _opacity;
            }

            set
            {
                if (_opacity == value)
                {
                    return;
                }

                _opacity = value;
                NotifyOfPropertyChange(() => Opacity);
            }
        }

        public string Name
        {
            get
            {
                return _name;
            }

            set
            {
                if (_name == value)
                {
                    return;
                }

                _name = value;
                NotifyOfPropertyChange(() => Name);
            }
        }

        public double MostLikelyEstimate
        {
            get
            {
                return _mostLikelyEstimate;
            }

            set
            {
                if (_mostLikelyEstimate == value)
                {
                    return;
                }

                _mostLikelyEstimate = value;
                NotifyOfPropertyChange(() => MostLikelyEstimate);
            }
        }

        public double ActualEffort
        {
            get
            {
                return _actualEffort;
            }

            set
            {
                if (_actualEffort == value)
                {
                    return;
                }

                _actualEffort = value;
                NotifyOfPropertyChange(() => ActualEffort);
            }
        }

        public string TaskOwner
        {
            get
            {
                return _taskOwner;
            }

            set
            {
                if (_taskOwner == value)
                {
                    return;
                }

                _taskOwner = value;
                NotifyOfPropertyChange(() => TaskOwner);
            }
        }

        public bool IsHandWritingEnabled
        {
            get
            {
                return _isHandWritingEnabled;
            }

            set
            {
                if (_isHandWritingEnabled == value)
                {
                    return;
                }

                _isHandWritingEnabled = value;
                NotifyOfPropertyChange(() => IsHandWritingEnabled);
            }
        }

        public string Description
        {
            get
            {
                return _description;
            }

            set
            {
                if (_description == value)
                {
                    return;
                }

                _description = value;
                NotifyOfPropertyChange(() => Description);
            }
        }

        public Color Background
        {
            get { return _background; }
            set
            {
                if (_background == value)
                {
                    return;
                }

                _background = value;
                NotifyOfPropertyChange(() => Background);
            }
        }

        #endregion Properties

        public void Move(double dx, double dy, double angle)
        {
            X += dx;
            Y += dy;
            Angle += angle;
        }
    }
}
