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
    public class Indicator : Control
    {
        static Indicator()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Indicator), new FrameworkPropertyMetadata(typeof(Indicator)));
        }

        public static readonly DependencyProperty MinimumProperty =
            DependencyProperty.Register(
                "Minimum",
                typeof(Int32),
                typeof(Indicator),
                new FrameworkPropertyMetadata(
                    0, OnMinimumChanged));

        public Int32 Minimum
        {
            get { return (Int32)base.GetValue(MinimumProperty); }
            set
            {
                base.SetValue(MinimumProperty, value);
            }
        }

        public static readonly DependencyProperty MaximumProperty =
            DependencyProperty.Register(
                "Maximum",
                typeof(Int32),
                typeof(Indicator),
                new FrameworkPropertyMetadata(
                    100, OnMaximumChanged));

        public Int32 Maximum
        {
            get { return (Int32)base.GetValue(MaximumProperty); }
            set
            {
                base.SetValue(MaximumProperty, value);
            }
        }

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(
                "Value",
                typeof(Int32),
                typeof(Indicator),
                new FrameworkPropertyMetadata(
                    0, OnValueChanged));



        public Int32 Value
        {
            get { return (Int32)base.GetValue(ValueProperty); }
            set
            {
                base.SetValue(ValueProperty, value);
            }
        }


        private double toDegrees(Int32 value)
        {
            double max = 287.0;

            return (max / (Maximum - Minimum)) * Value;
        }


        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (Indicator)d;

            int newValue = (int)e.NewValue;
            control.ReactValueChanged(newValue);
        }

        private protected virtual void ReactValueChanged(int newvalue)
        {
            Value = newvalue;

            rotateImage();
        }

        private static void OnMaximumChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (Indicator)d;

            int newValue = (int)e.NewValue;
            control.ReactMaximumChanged(newValue);
        }

        private protected virtual void ReactMaximumChanged(int newvalue)
        {
            Maximum = newvalue;

            rotateImage();
        }

        private static void OnMinimumChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (Indicator)d;

            int newValue = (int)e.NewValue;
            control.ReactMinimumChanged(newValue);
        }

        private protected virtual void ReactMinimumChanged(int newvalue)
        {
            Minimum = newvalue;

            rotateImage();
        }


        // noch den text einfügen beim bild, minimum, value und maximum

        Image Nadel;


        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            Nadel = (Image)this.Template.FindName("PART_NADEL", this);

            Label lbMinimum =
                (Label)this.Template.FindName("PART_MIN_LABEL", this);
            Binding bindingMinimum = new Binding();
            bindingMinimum.Source = this;
            bindingMinimum.Path = new PropertyPath("Minimum");
            lbMinimum.SetBinding(Label.ContentProperty, bindingMinimum);

            Label lbMaximum =
                (Label)this.Template.FindName("PART_MAX_LABEL", this);
            Binding bindingMaximum = new Binding();
            bindingMaximum.Source = this;
            bindingMaximum.Path = new PropertyPath("Maximum");
            lbMaximum.SetBinding(Label.ContentProperty, bindingMaximum);

            Label lbValue =
                (Label)this.Template.FindName("PART_VALUE_LABEL", this);
            Binding bindingValue = new Binding();
            bindingValue.Source = this;
            bindingValue.Path = new PropertyPath("Value");
            lbValue.SetBinding(Label.ContentProperty, bindingValue);


            rotateImage();
        }

        public void rotateImage()
        {
            if (Nadel == null)
            {
                return;
            }


            RotateTransform rt = new RotateTransform(toDegrees(Value));

            Nadel.RenderTransform = rt;
        }

        
    }
}