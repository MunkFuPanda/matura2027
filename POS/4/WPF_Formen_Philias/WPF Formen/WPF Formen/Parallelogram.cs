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
    internal class Parallelogram : Basis
    {
        public static readonly DependencyProperty LProperty = DependencyProperty.Register("L", typeof(Double), typeof(Parallelogram), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));
        public static readonly DependencyProperty BProperty = DependencyProperty.Register("B", typeof(Double), typeof(Parallelogram), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));
        public static readonly DependencyProperty WProperty = DependencyProperty.Register("W", typeof(Double), typeof(Parallelogram), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));


        [TypeConverter(typeof(LengthConverter))]
        public double L
        {
            get { return (double)base.GetValue(LProperty); }
            set { base.SetValue(LProperty, value); }
        }

        [TypeConverter(typeof(LengthConverter))]
        public double B
        {
            get { return (double)base.GetValue(BProperty); }
            set { base.SetValue(BProperty, value); }
        }

        [TypeConverter(typeof(LengthConverter))]
        public double W
        {
            get { return (double)base.GetValue(WProperty); }
            set { base.SetValue(WProperty, value); }
        }

        /// <summary>
        /// Zeichnet ein Parallelogram
        /// </summary>
        protected override PathFigure CreatePathFigure()
        {
            PathFigure myPathFigure = new PathFigure();
            myPathFigure.StartPoint = new Point(X1, Y1);
            myPathFigure.Segments.Add(new LineSegment(new Point(X1 + L, Y1), true));
            myPathFigure.Segments.Add(new LineSegment(new Point(X1 + L + B * Math.Tan((2 * Math.PI) * (W / 360)), Y1 + B), true));
            myPathFigure.Segments.Add(new LineSegment(new Point(X1 + B * Math.Tan((2 * Math.PI) * (W / 360)), Y1 + B), true));
            myPathFigure.IsClosed = true;
            return myPathFigure;
        }
    }
}
