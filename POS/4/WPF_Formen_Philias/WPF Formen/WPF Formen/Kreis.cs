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
    internal class Kreis : Basis
    {
        public static readonly DependencyProperty RProperty = DependencyProperty.Register("R", typeof(Double), typeof(Kreis), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));

        [TypeConverter(typeof(LengthConverter))]
        public double R
        {
            get { return (double)base.GetValue(RProperty); }
            set { base.SetValue(RProperty, value); }
        }

        /// <summary>
        /// Zeichnet ein Kreis
        /// </summary>
        protected override PathFigure CreatePathFigure()
        {
            PathFigure myPathFigure = new PathFigure();
            myPathFigure.StartPoint = new Point(X1 + R, Y1);
            myPathFigure.Segments.Add(new ArcSegment(
                new Point(X1 - R, Y1),   // rechts
                new Size(R, R),
                0,
                false,
                SweepDirection.Clockwise,
                true));


            // untere Hälfte
            myPathFigure.Segments.Add(new ArcSegment(
                new Point(X1 + R, Y1),   // zurück zum Start
                new Size(R, R),
                0,
                false,
                SweepDirection.Clockwise,
                true));

            myPathFigure.IsClosed = true;
            return myPathFigure;
        }
    }
}
