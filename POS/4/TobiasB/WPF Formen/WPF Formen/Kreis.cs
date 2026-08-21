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
    class Kreis : Basis
    {
        public static readonly DependencyProperty RadiusProperty = DependencyProperty.Register("Radius", typeof(Double), typeof(Kreis), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));

        [TypeConverter(typeof(LengthConverter))]
        public double Radius
        {
            get { return (double)base.GetValue(RadiusProperty); }
            set { base.SetValue(RadiusProperty, value); }
        }

        protected override PathFigure CreatePathFigure()
        {
            PathFigure myPathFigure = new PathFigure();
            myPathFigure.StartPoint = new Point(X1, Y1);
            myPathFigure.Segments.Add(new ArcSegment(new Point(X1 + Radius, Y1), new Size(Radius, Radius), 0, true, SweepDirection.Clockwise, true));
            myPathFigure.Segments.Add(new ArcSegment(new Point(X1, Y1), new Size(Radius, Radius), 0, false, SweepDirection.Clockwise, true));
            return myPathFigure;
        }
    }
}
