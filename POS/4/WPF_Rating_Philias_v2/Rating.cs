using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPF_Rating
{
    public class Rating : Control
    {
        private Slider _ratingSlider;
        private List<Image> _starImages;
        private TextBlock _ratingTextBlock;
        private Button _submitButton;

        static Rating()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Rating), new FrameworkPropertyMetadata(typeof(Rating)));
        }

        public int RatingSlider
        {
            get => (int)GetValue(RatingSliderProperty);
            set => SetValue(RatingSliderProperty, value);
        }

        public static readonly DependencyProperty RatingSliderProperty =
            DependencyProperty.Register(nameof(RatingSlider), typeof(int), typeof(Rating));

        public string RatingText
        {
            get => (string)GetValue(RatingTextProperty);
            set => SetValue(RatingTextProperty, value);
        }

        public static readonly DependencyProperty RatingTextProperty =
            DependencyProperty.Register(nameof(RatingText), typeof(string), typeof(Rating));

        public static readonly RoutedEvent RatedEvent =
            EventManager.RegisterRoutedEvent(
                nameof(Rated),
                RoutingStrategy.Bubble,
                typeof(RoutedEventHandler),
                typeof(Rating));

        public event RoutedEventHandler Rated
        {
            add => AddHandler(RatedEvent, value);
            remove => RemoveHandler(RatedEvent, value);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _ratingSlider = GetTemplateChild("PART_RatingSlider") as Slider;
            _ratingTextBlock = GetTemplateChild("PART_RatingTextBlock") as TextBlock;
            _starImages = new List<Image>();
            _submitButton = GetTemplateChild("PART_SubmitButton") as Button;

            if (_submitButton != null)
            {
                _submitButton.Click += (s, e) =>
                {
                    RaiseEvent(new RoutedEventArgs(RatedEvent));
                };
            }

            if (_starImages != null)
            {
                for (int i = 1; i <= 6; i++)
                {
                    var starImage = GetTemplateChild($"PART_Star{i}") as Image;
                    if (starImage != null)
                    {
                        _starImages.Add(starImage);
                    }
                }
            }

            if (_ratingSlider != null)
            {
                _ratingSlider.ValueChanged += RatingSlider_ValueChanged;
            }
            
            if (_ratingTextBlock != null)
            {
                _ratingTextBlock.Text = RatingText;
            }
        }
        private void RatingSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            RatingSlider = (int)e.NewValue;
            for (int i = 1; i <= 5; i++)
            {
                if (i <= e.NewValue)
                {
                    _starImages[i - 1].Source = new BitmapImage(
                        new Uri(
                            "pack://application:,,,/WPF_Rating;component/Ressourcen/star_filled.png",
                            UriKind.Absolute
                        )
                    );
                }
                else
                {
                    _starImages[i - 1].Source = new BitmapImage(
                        new Uri(
                            "pack://application:,,,/WPF_Rating;component/Ressourcen/star_unfilled.png",
                            UriKind.Absolute
                        )
                    );
                }
            }
        }


    }
}
