using System;
using System.Security.Principal;
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

namespace WPF_Rating_System_Lib
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:WPF_Rating_System_Lib"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:WPF_Rating_System_Lib;assembly=WPF_Rating_System_Lib"
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
    public class RatingSystem : Control
    {

        // <Image Source="pack://application:,,,component/Resources/stern_full.jpg" Stretch="UniformToFill" Margin="4"></Image>
        static RatingSystem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RatingSystem), new FrameworkPropertyMetadata(typeof(RatingSystem)));
        }

        public RatingSystem()
        {

        }
        private static void OnStarCountChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (RatingSystem)d;
            control.OnStarCountChanged();
        }

        private void OnStarCountChanged()
        {
            setStars(StarCount);
        }


        public void setStars(int n)
        {
            var star1 = GetTemplateChild("star1") as Image;
            var star2 = GetTemplateChild("star2") as Image;
            var star3 = GetTemplateChild("star3") as Image;
            var star4 = GetTemplateChild("star4") as Image;
            var star5 = GetTemplateChild("star5") as Image;

            List<Image> starImages = new List<Image>() { star1, star2, star3, star4, star5 };

            foreach (Image img in starImages)
            {
                img.Source = new BitmapImage(
                    new Uri("pack://application:,,,/WPF_Rating_System_Lib;component/Resources/stern_leer.png", UriKind.Absolute));
            }

            if (n == 0)
            {
                return;
            }
            if (n>=1)
            {
                star1.Source = new BitmapImage(
                    new Uri("pack://application:,,,/WPF_Rating_System_Lib;component/Resources/stern_full.jpg", UriKind.Absolute));
            }
            if (n >= 2)
            {
                star2.Source = new BitmapImage(
                    new Uri("pack://application:,,,/WPF_Rating_System_Lib;component/Resources/stern_full.jpg", UriKind.Absolute));
            }
            if (n >= 3)
            {
                star3.Source = new BitmapImage(
                    new Uri("pack://application:,,,/WPF_Rating_System_Lib;component/Resources/stern_full.jpg", UriKind.Absolute));
            }
            if (n >= 4)
            {
                star4.Source = new BitmapImage(
                    new Uri("pack://application:,,,/WPF_Rating_System_Lib;component/Resources/stern_full.jpg", UriKind.Absolute));
            }
            if (n == 5)
            {
                star5.Source = new BitmapImage(
                    new Uri("pack://application:,,,/WPF_Rating_System_Lib;component/Resources/stern_full.jpg", UriKind.Absolute));
            }
        }

        public static readonly DependencyProperty StarCountProperty =
        DependencyProperty.Register(
            nameof(StarCount),
            typeof(int),
            typeof(RatingSystem),
            new PropertyMetadata(5, OnStarCountChanged));
        public int StarCount
        {
            get => (int)GetValue(StarCountProperty);
            set => SetValue(StarCountProperty, value);
        }


        // Path bindings sind auf süß auch dabei aber werden halt nicht verwendet

        public static readonly DependencyProperty
            StarPicturePathProperty = DependencyProperty.Register(
        "StarPicturePath",
        typeof(BitmapImage),
        typeof(RatingSystem),
        new PropertyMetadata(new BitmapImage(
                    new Uri("pack://application:,,,/WPF_Rating_System_Lib;component/Resources/stern_leer.png", UriKind.Absolute)), null));

        public BitmapImage StarPicturePath
        {
            get { return (BitmapImage)base.GetValue(StarPicturePathProperty); }
            set { base.SetValue(StarPicturePathProperty, value); }
        }

    }
}