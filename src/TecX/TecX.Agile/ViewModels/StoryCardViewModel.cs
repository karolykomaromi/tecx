using System;

using Caliburn.Micro;

namespace TecX.Agile.ViewModels
{
    public class StoryCardViewModel : Screen
    {
        private double _x;
        private double _y;
        private double _angle;
        private Guid _id;

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
    }
}
