using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace WPF_Formen
{
    class Ellipse : Kreis
    {
        public static readonly DependencyProperty Radius2Property = DependencyProperty.Register("Radius2", typeof(Double), typeof(Ellipse), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));

        [TypeConverter(typeof(LengthConverter))]
        public double Radius2
        {
            get { return (double)base.GetValue(Radius2Property); }
            set { base.SetValue(Radius2Property, value); }
        }

        protected override PathFigure CreatePathFigure()
        {
            PathFigure myPathFigure = new PathFigure();
            myPathFigure.StartPoint = new Point(X1, Y1);
            myPathFigure.Segments.Add(new ArcSegment(new Point(X1 + Radius, Y1 + Radius2), new Size(Radius, Radius2), 0, true, SweepDirection.Clockwise, true));
            myPathFigure.Segments.Add(new ArcSegment(new Point(X1, Y1), new Size(Radius, Radius2), 0, false, SweepDirection.Clockwise, true));
            return myPathFigure;
        }
    }
}
