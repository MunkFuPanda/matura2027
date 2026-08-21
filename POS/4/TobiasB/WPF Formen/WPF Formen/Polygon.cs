using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace WPF_Formen
{
    class Polygon : Basis
    {
        public static readonly DependencyProperty LaengeProperty = DependencyProperty.Register("Laenge", typeof(Double), typeof(Polygon), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));
        public static readonly DependencyProperty CornerProperty = DependencyProperty.Register("Corner", typeof(Int32), typeof(Polygon), new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));

        [TypeConverter(typeof(LengthConverter))]
        public double Laenge
        {
            get { return (double)base.GetValue(LaengeProperty); }
            set { base.SetValue(LaengeProperty, value); }
        }

        public Int32 Corner
        {
            get { return (Int32)base.GetValue(CornerProperty); }
            set { base.SetValue(CornerProperty, value); }
        }


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
