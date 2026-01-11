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
    internal class Quadrat : Basis
    {
        public static readonly DependencyProperty LProperty = DependencyProperty.Register("L", typeof(Double), typeof(Quadrat), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));

        [TypeConverter(typeof(LengthConverter))]
        public double L
        {
            get { return (double)base.GetValue(LProperty); }
            set { base.SetValue(LProperty, value); }
        }

        /// <summary>
        /// Zeichnet ein Rechteck
        /// </summary>
        protected override PathFigure CreatePathFigure()
        {
            PathFigure myPathFigure = new PathFigure();
            myPathFigure.StartPoint = new Point(X1, Y1);
            myPathFigure.Segments.Add(new LineSegment(new Point(X1, Y1 + L), true));
            myPathFigure.Segments.Add(new LineSegment(new Point(X1 + L, Y1 + L), true));
            myPathFigure.Segments.Add(new LineSegment(new Point(X1 + L, Y1), true));
            myPathFigure.IsClosed = true;
            return myPathFigure;
        }
    }
}
