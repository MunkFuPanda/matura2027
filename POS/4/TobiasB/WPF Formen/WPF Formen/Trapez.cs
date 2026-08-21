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
    class Trapez : Quadrat
    {
        public static readonly DependencyProperty X2Property = DependencyProperty.Register("X2", typeof(Double), typeof(Trapez), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));
        public static readonly DependencyProperty Y2Property = DependencyProperty.Register("Y2", typeof(Double), typeof(Trapez), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));

        public static readonly DependencyProperty Laenge2Property = DependencyProperty.Register("Laenge2", typeof(Double), typeof(Trapez), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));


        [TypeConverter(typeof(LengthConverter))]
        public double X2
        {
            get { return (double)base.GetValue(X2Property); }
            set { base.SetValue(X2Property, value); }
        }

        [TypeConverter(typeof(LengthConverter))]
        public double Y2
        {
            get { return (double)base.GetValue(Y2Property); }
            set { base.SetValue(Y2Property, value); }
        }

        [TypeConverter(typeof(LengthConverter))]
        public double Laenge2
        {
            get { return (double)base.GetValue(Laenge2Property); }
            set { base.SetValue(Laenge2Property, value); }
        }

        /// <summary>
        /// Zeichnet ein Parallelogramm
        /// </summary>
        protected override PathFigure CreatePathFigure()
        {
            PathFigure myPathFigure = new PathFigure();
            myPathFigure.StartPoint = new Point(X1, Y1);
            myPathFigure.Segments.Add(new LineSegment(new Point(X1 + Laenge, Y1), true));
            myPathFigure.Segments.Add(new LineSegment(new Point(X2, Y2), true));
            myPathFigure.Segments.Add(new LineSegment(new Point(X2 - Laenge2, Y2), true));
            myPathFigure.IsClosed = true;
            return myPathFigure;
        }
    }
}
