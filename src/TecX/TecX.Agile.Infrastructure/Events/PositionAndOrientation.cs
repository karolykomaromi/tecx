using System.Runtime.Serialization;

namespace TecX.Agile.Infrastructure.Events
{
    [DataContract]
    public class PositionAndOrientation
    {
        [DataMember]
        private readonly double _x;
        [DataMember]
        private readonly double _y;
        [DataMember]
        private readonly double _angle;

        public double Angle
        {
            get { return _angle; }
        }

        public double Y
        {
            get { return _y; }
        }

        public double X
        {
            get { return _x; }
        }

        public PositionAndOrientation(double x, double y, double angle)
        {
            _x = x;
            _y = y;
            _angle = angle;
        }
    }
}