using Microsoft.Win32;       // WPF OpenFileDialog
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
        private PlayerViewModel Vm => (PlayerViewModel)DataContext;

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
            Vm.Duration = video.NaturalDuration.TimeSpan.TotalSeconds;
            Vm.Progress = video.Position.TotalSeconds;
        }

        private void OpenFiles_Click(object sender, RoutedEventArgs e) {
            var dialog = new OpenFileDialog {
                Filter = "Video files|*.mp4;*.mkv;*.avi;*.mov;*.wmv|All files|*.*",
                Multiselect = true,
                Title = "Select media files"
            };

            if (dialog.ShowDialog() == true) {
                Vm.Playlist.Clear();
                foreach (var file in dialog.FileNames)
                    Vm.Playlist.Add(file);

                if (Vm.Playlist.Count > 0) {
                    Vm.CurrentIndex = 0;
                    PlayCurrent();
                }
            }
        }

        private void PlayCurrent() {
            if (Vm.CurrentItem == null) return;

            video.Stop();
            video.Source = new Uri(Vm.CurrentItem, UriKind.Absolute);
            video.Play();
            playing = true;
            img_play.Source = new BitmapImage(new Uri("pack://application:,,,/resources/pause.png"));

            timer.Start();
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e) {
            if (video.Source == null && Vm.CurrentItem != null) {
                PlayCurrent();
                return;
            }

            if (!playing) {
                video.Play();
                img_play.Source = new BitmapImage(new Uri("pack://application:,,,/resources/pause.png"));
                timer.Start();
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

        private void MuteButton_Click(object sender, RoutedEventArgs e) => Vm.Volume = 0;

        private void NextButton_Click(object sender, RoutedEventArgs e) {
            if (Vm.CurrentIndex + 1 < Vm.Playlist.Count) {
                Vm.CurrentIndex++;
                PlayCurrent();
            }
        }

        private void PrevButton_Click(object sender, RoutedEventArgs e) {
            if (Vm.CurrentIndex - 1 >= 0) {
                Vm.CurrentIndex--;
                PlayCurrent();
            }
        }

        private void PlaylistBox_MouseDoubleClick(object sender, MouseButtonEventArgs e) {
            if (playlistBox.SelectedIndex >= 0) {
                Vm.CurrentIndex = playlistBox.SelectedIndex;
                PlayCurrent();
            }
        }

        private void Exit_Click(object sender, RoutedEventArgs e) {
            if (System.Windows.MessageBox.Show("Do you really want to exit?", "Exit", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                Close();
        }

        private void ProgressBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
            if (!video.NaturalDuration.HasTimeSpan) return;

            var pos = e.GetPosition(progressBar);
            double ratio = pos.X / progressBar.ActualWidth;
            video.Position = TimeSpan.FromSeconds(ratio * video.NaturalDuration.TimeSpan.TotalSeconds);
        }

        private void Window_MouseMove(object sender, MouseEventArgs e) {
            ControlOverlay.Visibility = Visibility.Visible;
            hideTimer.Stop();
            hideTimer.Start();
        }
    }
}
