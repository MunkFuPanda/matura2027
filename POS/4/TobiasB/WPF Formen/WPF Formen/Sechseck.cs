using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace WPF_Formen
{
    class Sechseck : Polygon
    {
        protected override PathFigure CreatePathFigure()
        {
            PathFigure myPathFigure = new PathFigure();
            myPathFigure.StartPoint = new Point(X1, Y1);

            double angleStep = 2 * Math.PI / Corner;
            double angle = 0;

            double x = X1;
            double y = Y1;

            for (int i = 1; i < Corner; i++)
            {
                angle += angleStep;

                double newX = x + Laenge * Math.Cos(angle);
                double newY = y + Laenge * Math.Sin(angle);

                myPathFigure.Segments.Add(new LineSegment(new Point(newX, newY), true));

                x = newX;
                y = newY;
            }

            myPathFigure.IsClosed = true;
            return myPathFigure;
        }
    }
}
