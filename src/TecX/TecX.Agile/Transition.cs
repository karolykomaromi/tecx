namespace TecX.Agile
{
    public struct Transition
    {
        public Transition(double x, double y, double angle)
            : this()
        {
            this.X = x;
            this.Y = y;
            this.Angle = angle % 360.0;
        }

        public double X { get; set; }

        public double Y { get; set; }

        public double Angle { get; set; }
    }
}