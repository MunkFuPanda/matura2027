using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace WPF_Formen
{
    internal class Polygon : Basis
    {
        public static readonly DependencyProperty EProperty = DependencyProperty.Register("E", typeof(Double), typeof(Polygon), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));
        public static readonly DependencyProperty RProperty = DependencyProperty.Register("R", typeof(Double), typeof(Polygon), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));

        [TypeConverter(typeof(LengthConverter))]
        public double E
        {
            get { return (double)base.GetValue(EProperty); }
            set { base.SetValue(EProperty, value); }
        }

        [TypeConverter(typeof(LengthConverter))]
        public double R
        {
            get { return (double)base.GetValue(RProperty); }
            set { base.SetValue(RProperty, value); }
        }

        /// <summary>
        /// Zeichnet ein Polygon
        /// </summary>
        protected override PathFigure CreatePathFigure()
        {
            PathFigure myPathFigure = new PathFigure();

            int ecken = (int)E;   // E sollte eigentlich int sein!
            double winkel0 = -Math.PI / 2;

            // 🔹 erster Eckpunkt
            Point start = new Point(
                X1 + R * Math.Cos(winkel0),
                Y1 + R * Math.Sin(winkel0)
            );

            myPathFigure.StartPoint = start;

            // 🔹 restliche Eckpunkte
            for (int i = 1; i < ecken; i++)
            {
                double winkel = i * 2 * Math.PI / ecken - Math.PI / 2;

                myPathFigure.Segments.Add(
                    new LineSegment(
                        new Point(
                            X1 + R * Math.Cos(winkel),
                            Y1 + R * Math.Sin(winkel)
                        ),
                        true
                    )
                );
            }

            myPathFigure.IsClosed = true;
            return myPathFigure;
        }
    }
}
