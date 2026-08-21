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
    internal class Rechteck : Quadrat
    {
        public static readonly DependencyProperty BreiteProperty = DependencyProperty.Register("Breite", typeof(Double), typeof(Rechteck), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));

        [TypeConverter(typeof(LengthConverter))]
        public double Breite
        {
            get { return (double)base.GetValue(BreiteProperty); }
            set { base.SetValue(BreiteProperty, value); }
        }

        protected override PathFigure CreatePathFigure()
        {
            PathFigure myPathFigure = new PathFigure();
            myPathFigure.StartPoint = new Point(X1, Y1);
            myPathFigure.Segments.Add(new LineSegment(new Point(X1 + Laenge, Y1), true));
            myPathFigure.Segments.Add(new LineSegment(new Point(X1 + Laenge, Y1 + Breite), true));
            myPathFigure.Segments.Add(new LineSegment(new Point(X1, Y1 + Breite), true));
            myPathFigure.IsClosed = true;
            return myPathFigure;
        }
    }
}
