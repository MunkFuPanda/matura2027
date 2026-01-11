using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace WPF_Formen
{
    internal class Elipse : Basis
    {
        public static readonly DependencyProperty R1Property = DependencyProperty.Register("R1", typeof(Double), typeof(Elipse), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));
        public static readonly DependencyProperty R2Property = DependencyProperty.Register("R2", typeof(Double), typeof(Elipse), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));

        [TypeConverter(typeof(LengthConverter))]
        public double R1
        {
            get { return (double)base.GetValue(R1Property); }
            set { base.SetValue(R1Property, value); }
        }

        [TypeConverter(typeof(LengthConverter))]
        public double R2
        {
            get { return (double)base.GetValue(R2Property); }
            set { base.SetValue(R2Property, value); }
        }

        /// <summary>
        /// Zeichnet ein Elipse
        /// </summary>
        protected override PathFigure CreatePathFigure()
        {
            PathFigure myPathFigure = new PathFigure();
            myPathFigure.StartPoint = new Point(X1 + R1, Y1);
            myPathFigure.Segments.Add(new ArcSegment(
                new Point(X1 - R1, Y1),   // rechts
                new Size(R1, R2),
                0,
                false,
                SweepDirection.Clockwise,
                true));


            // untere Hälfte
            myPathFigure.Segments.Add(new ArcSegment(
                new Point(X1 + R1, Y1),   // zurück zum Start
                new Size(R1, R2),
                0,
                false,
                SweepDirection.Clockwise,
                true));

            myPathFigure.IsClosed = true;
            return myPathFigure;
        }
    }
}
