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
    internal class Stern : Basis
    {
        public static readonly DependencyProperty EProperty = DependencyProperty.Register("E", typeof(Double), typeof(Stern), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));
        public static readonly DependencyProperty R1Property = DependencyProperty.Register("R1", typeof(Double), typeof(Stern), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));
        public static readonly DependencyProperty R2Property = DependencyProperty.Register("R2", typeof(Double), typeof(Stern), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));

        [TypeConverter(typeof(LengthConverter))]
        public double E
        {
            get { return (double)base.GetValue(EProperty); }
            set { base.SetValue(EProperty, value); }
        }

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
        /// Zeichnet ein Stern
        /// </summary>
        protected override PathFigure CreatePathFigure()
        {
            PathFigure figure = new PathFigure();

            int zacken = (int)E; // E sollte eigentlich int sein
            double angleStep = Math.PI / zacken;
            double startAngle = -Math.PI / 2; // Stern zeigt nach oben

            // 🔹 erster Punkt (außen)
            Point start = new Point(
                X1 + R1 * Math.Cos(startAngle),
                Y1 + R1 * Math.Sin(startAngle)
            );

            figure.StartPoint = start;

            // 🔹 restliche Punkte
            for (int i = 1; i < 2 * zacken; i++)
            {
                double angle = startAngle + i * angleStep;
                double radius = (i % 2 == 0) ? R1 : R2;

                figure.Segments.Add(
                    new LineSegment(
                        new Point(
                            X1 + radius * Math.Cos(angle),
                            Y1 + radius * Math.Sin(angle)
                        ),
                        true
                    )
                );
            }

            figure.IsClosed = true;
            return figure;
        }

    }
}
