using System;
using System.Windows;

using Caliburn.Micro;

namespace TecX.Agile.Infrastructure
{
    public interface IShell
    {
        void AddOverlay(Screen overlay);

        Point PointFromScreen(Point point);

        Point PointToScreen(Point point);

        void AddStoryCard(double x, double y, double angle);
    }
}
