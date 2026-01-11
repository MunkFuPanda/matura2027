using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;

namespace WPF_Formen
{
    internal class Rhombus : Parallelogram
    {
        /// <summary>
        /// Zeichnet ein Rhombus
        /// </summary>
        protected override PathFigure CreatePathFigure()
        {
            double rad = W * Math.PI / 180;

            PathFigure myPathFigure = new PathFigure();
            myPathFigure.StartPoint = new Point(X1, Y1);

            // Punkt 2
            myPathFigure.Segments.Add(
                new LineSegment(
                    new Point(X1 + L, Y1),
                    true));

            // Punkt 3
            myPathFigure.Segments.Add(
                new LineSegment(
                    new Point(
                        X1 + L + L * Math.Cos(rad),
                        Y1 + L * Math.Sin(rad)
                    ),
                    true));

            // Punkt 4
            myPathFigure.Segments.Add(
                new LineSegment(
                    new Point(
                        X1 + L * Math.Cos(rad),
                        Y1 + L * Math.Sin(rad)
                    ),
                    true));
            myPathFigure.IsClosed = true;
            return myPathFigure;
        }
    }
}
