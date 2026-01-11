using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPF_Indicator
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:WPF_Indicator"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:WPF_Indicator;assembly=WPF_Indicator"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:CustomControl1/>
    ///
    /// </summary>
    public class CustomControl1 : Control
    {
        static CustomControl1()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CustomControl1), new FrameworkPropertyMetadata(typeof(CustomControl1)));
        }

        public static readonly DependencyProperty MinimumValueProperty =
            DependencyProperty.Register(
                nameof(MinimumValue),
                typeof(double),
                typeof(CustomControl1),
                new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender,
                OnValueChanged));

        public double MinimumValue
        {
            get => (double)GetValue(MinimumValueProperty);
            set => SetValue(MinimumValueProperty, value);
        }

        public static readonly DependencyProperty MaximumValueProperty =
            DependencyProperty.Register(
                nameof(MaximumValue),
                typeof(double),
                typeof(CustomControl1),
                new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender,
                OnValueChanged));

        public double MaximumValue
        {
            get => (double)GetValue(MaximumValueProperty);
            set => SetValue(MaximumValueProperty, value);
        }

        public static readonly DependencyProperty CurrentValueProperty =
            DependencyProperty.Register(
                nameof(CurrentValue),     // ← extrem wichtig
                typeof(double),
                typeof(CustomControl1),
                new FrameworkPropertyMetadata(
                    0.0,
                    FrameworkPropertyMetadataOptions.AffectsRender,
                    OnValueChanged)
             );

        public double CurrentValue
        {
            get => (double)GetValue(CurrentValueProperty);
            set => SetValue(CurrentValueProperty, value);
        }

        public static readonly DependencyProperty AngleProperty =
            DependencyProperty.Register(
                nameof(Angle),
                typeof(double),
                typeof(CustomControl1),
                new FrameworkPropertyMetadata(0.0)
            );

        public double Angle
        {
            get => (double)GetValue(AngleProperty);
            set => SetValue(AngleProperty, value);
        }


        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is CustomControl1 c)
                c.CalculateAngle();
        }

        

        private void CalculateAngle()
        {
            if (MaximumValue <= MinimumValue)
            {
                Angle = 0;
                return;
            }

            double clamped = Math.Max(MinimumValue, Math.Min(CurrentValue, MaximumValue));
            double ratio = (clamped - MinimumValue) / (MaximumValue - MinimumValue);

            Angle = ratio * 287.0;
        }

        /*
        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            double percentage = CalculatePercentage();
            Rect rect = new Rect(0, 0, ActualWidth * percentage, ActualHeight);
            drawingContext.DrawRectangle(Brushes.Green, null, rect);
            Rect borderRect = new Rect(0, 0, ActualWidth, ActualHeight);
            drawingContext.DrawRectangle(null, new Pen(Brushes.Black, 1), borderRect);
        }
        */
    }
}