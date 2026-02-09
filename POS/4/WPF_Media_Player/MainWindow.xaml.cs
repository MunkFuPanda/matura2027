using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace WPF_Media_Player {
    public partial class MainWindow : Window {
        private readonly DispatcherTimer timer;
        private readonly DispatcherTimer hideTimer;
        private bool playing;

        public MainWindow() {
            InitializeComponent();
            DataContext = new PlayerViewModel();

            timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(200) };
            timer.Tick += Timer_Tick;

            hideTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(3) };
            hideTimer.Tick += (s, e) => ControlOverlay.Visibility = Visibility.Collapsed;
        }

        private void Timer_Tick(object? sender, EventArgs e) {
            if (!video.NaturalDuration.HasTimeSpan) return;

            var vm = (PlayerViewModel)DataContext;
            vm.Duration = video.NaturalDuration.TimeSpan.TotalSeconds;
            vm.Progress = video.Position.TotalSeconds;
        }

        private void OpenFile_Click(object sender, RoutedEventArgs e) {
            var dialog = new OpenFileDialog {
                Filter = "Video files|*.mp4;*.mkv;*.avi;*.mov;*.wmv|All files|*.*",
                Title = "Select a media file"
            };

            if (dialog.ShowDialog() == true) {
                video.Stop();
                timer.Stop();
                video.Source = new Uri(dialog.FileName, UriKind.Absolute);
                playing = false;
                img_play.Source = new BitmapImage(new Uri("pack://application:,,,/resources/play-button.png"));
            }
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e) {
            if (video.Source == null) return;

            if (!playing) {
                video.Play();
                timer.Start();
                img_play.Source = new BitmapImage(new Uri("pack://application:,,,/resources/pause.png"));
            } else {
                video.Pause();
                img_play.Source = new BitmapImage(new Uri("pack://application:,,,/resources/play-button.png"));
            }

            playing = !playing;
        }

        private void StopButton_Click(object sender, RoutedEventArgs e) {
            video.Stop();
            timer.Stop();
            playing = false;
            img_play.Source = new BitmapImage(new Uri("pack://application:,,,/resources/play-button.png"));
        }

        private void MuteButton_Click(object sender, RoutedEventArgs e) {
            ((PlayerViewModel)DataContext).Volume = 0;
        }

        private void Exit_Click(object sender, RoutedEventArgs e) {
            if (MessageBox.Show("Do you really want to exit?", "Exit", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                Close();
        }

        // ProgressBar click to seek
        private void ProgressBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
            if (!video.NaturalDuration.HasTimeSpan) return;

            var pos = e.GetPosition(progressBar);
            double ratio = pos.X / progressBar.ActualWidth;
            video.Position = TimeSpan.FromSeconds(ratio * video.NaturalDuration.TimeSpan.TotalSeconds);
        }

        // Show controls on mouse move
        private void Window_MouseMove(object sender, MouseEventArgs e) {
            ControlOverlay.Visibility = Visibility.Visible;
            hideTimer.Stop();
            hideTimer.Start();
        }
    }
}
