using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace WPF_Formen
{
    class Raute : Quadrat
    {
        protected override PathFigure CreatePathFigure()
        {
            // Raute ist verbesserungswürdig e und f berücksichtigen

            PathFigure myPathFigure = new PathFigure();
            myPathFigure.StartPoint = new Point(X1, Y1 - Laenge);
            myPathFigure.Segments.Add(new LineSegment(new Point(X1 + Laenge, Y1), true));
            myPathFigure.Segments.Add(new LineSegment(new Point(X1, Y1 + Laenge), true));
            myPathFigure.Segments.Add(new LineSegment(new Point(X1 - Laenge, Y1), true));

            myPathFigure.IsClosed = true;
            return myPathFigure;
        }
    }
}
