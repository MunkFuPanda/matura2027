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
    class Quadrat : Basis
    {
        public static readonly DependencyProperty LaengeProperty = DependencyProperty.Register("Laenge", typeof(Double), typeof(Quadrat), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));

        [TypeConverter(typeof(LengthConverter))]
        public double Laenge
        {
            get { return (double)base.GetValue(LaengeProperty); }
            set { base.SetValue(LaengeProperty, value); }
        }

        /// <summary>
        /// Zeichnet ein Quadrat
        /// </summary>
        protected override PathFigure CreatePathFigure()
        {
            PathFigure myPathFigure = new PathFigure();
            myPathFigure.StartPoint = new Point(X1, Y1);
            myPathFigure.Segments.Add(new LineSegment(new Point(X1 + Laenge, Y1), true));
            myPathFigure.Segments.Add(new LineSegment(new Point(X1 + Laenge, Y1 + Laenge), true));
            myPathFigure.Segments.Add(new LineSegment(new Point(X1, Y1 + Laenge), true));
            myPathFigure.IsClosed = true;
            return myPathFigure;
        }
    }
}
