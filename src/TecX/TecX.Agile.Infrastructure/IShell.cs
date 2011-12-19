namespace TecX.Agile.Infrastructure
{
    using System;
    using System.Windows;

    using Caliburn.Micro;

    public interface IShell
    {
        void AddOverlay(Screen overlay);

        Point PointFromScreen(Point point);

        Point PointToScreen(Point point);

        void AddStoryCard(Guid id, double x, double y, double angle);
    }
}
