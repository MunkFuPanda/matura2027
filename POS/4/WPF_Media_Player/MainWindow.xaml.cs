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
using System.Windows.Threading;

namespace WPF_Media_Player
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer timer;
        public int Volume { get; set; }
        public MainWindow()
        {
            InitializeComponent();

            DataContext = new PlayerViewModel();

            timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(200)
            };
            timer.Tick += Timer_Tick;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (video.NaturalDuration.HasTimeSpan)
            {
                var vm = (PlayerViewModel)DataContext;

                vm.Duration = video.NaturalDuration.TimeSpan.TotalSeconds;
                vm.Progress = video.Position.TotalSeconds;
            }
        }

        private void OpenFile_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {

        }

        private void About_Click(object sender, RoutedEventArgs e)
        {

        }

        private void forwardButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void backwardButton_Click(object sender, RoutedEventArgs e)
        {

        }

        bool playing = false;
        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            video.Source = new Uri("C:/Schule/POS/test_videos/video.mp4");

            playing = !playing;
            if (playing)
            {
                img_play.Source = new BitmapImage(new Uri("pack://application:,,,/Ressourcen/pause.png"));
                video.Play();
            }
            else
            {
                img_play.Source = new BitmapImage(new Uri("pack://application:,,,/Ressourcen/play-button.png"));
                video.Pause();
            }

        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MuteButton_Click(object sender, RoutedEventArgs e)
        {
            ((PlayerViewModel)DataContext).Volume = 0;
        }
    }
}