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
    internal class Spirale : Basis
    {

        public static readonly DependencyProperty EndRadiusProperty = DependencyProperty.Register("EndRadius", typeof(Double), typeof(Parallelogram), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));
        public static readonly DependencyProperty GesamtWinkelProperty = DependencyProperty.Register("GesamtWinkel", typeof(Double), typeof(Parallelogram), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));
        public static readonly DependencyProperty StepsNProperty = DependencyProperty.Register("StepsN", typeof(Double), typeof(Parallelogram), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));

        [TypeConverter(typeof(LengthConverter))]
        public double EndRadius
        {
            get { return (double)base.GetValue(EndRadiusProperty); }
            set { base.SetValue(EndRadiusProperty, value); }
        }

        [TypeConverter(typeof(LengthConverter))]
        public double GesamtWinkel
        {
            get { return (double)base.GetValue(GesamtWinkelProperty); }
            set { base.SetValue(GesamtWinkelProperty, value); }
        }

        [TypeConverter(typeof(LengthConverter))]
        public double StepsN
        {
            get { return (double)base.GetValue(StepsNProperty); }
            set { base.SetValue(StepsNProperty, value); }
        }

        /// <summary>
        /// Zeichnet eine Spirale
        /// </summary>
        protected override PathFigure CreatePathFigure()
        {
            PathFigure myPathFigure = new PathFigure();
            myPathFigure.StartPoint = new Point(X1, Y1);

            int nOfSteps = (int)StepsN;

            for (int i = 0; i < nOfSteps; ++i)
            {
                var r = (EndRadius / nOfSteps) * i;
                var theta = (GesamtWinkel / nOfSteps) * i;

                Point to = new Point(
                    myPathFigure.StartPoint.X + r * Math.Cos(theta),
                    myPathFigure.StartPoint.Y + r * Math.Sin(theta)
                );

                myPathFigure.Segments.Add(new LineSegment(to, true));
            }

            myPathFigure.IsClosed = false;
            return myPathFigure;
        }

    }
}
