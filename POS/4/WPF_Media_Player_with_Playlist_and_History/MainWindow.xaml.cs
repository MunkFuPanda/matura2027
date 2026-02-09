using Microsoft.Win32;
using System.IO;
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

        string path = string.Empty;

        List<string> videoFiles = new List<string>();

        int currentVideoIndex = 0;

        List<string> history = new List<string>();

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
            using (var dialog = new FolderBrowserDialog())
            {
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    path = dialog.SelectedPath;
                }
            }

            foreach (var file in Directory.GetFiles(path))
            {
                if (file.EndsWith(".mp4") || file.EndsWith(".avi") || file.EndsWith(".mkv"))
                {
                    videoFiles.Add(file);
                }
            }

            if (videoFiles.Count > 0)
            {
                video.Source = new Uri(videoFiles[0]);
            }
            else
            {
                System.Windows.MessageBox.Show("No video files found in the selected folder.");
            }

            playlistListBox.ItemsSource = videoFiles.Select(f => System.IO.Path.GetFileName(f)).ToList();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void forwardButton_Click(object sender, RoutedEventArgs e)
        {
            if (videoFiles.Count == 0)
            {
                return;
            }

            if (currentVideoIndex < videoFiles.Count - 1)
            {
                currentVideoIndex++;
            }
            else
            {
                currentVideoIndex = 0;
            }

            video.Source = new Uri(videoFiles[currentVideoIndex]);
            if (!history.Contains(video.Source.LocalPath))
            {
                history.Add(video.Source.LocalPath);
                historyListBox.ItemsSource = null;
                historyListBox.ItemsSource = history.Select(f => System.IO.Path.GetFileName(f)).ToList();
            }
        }

        private void backwardButton_Click(object sender, RoutedEventArgs e)
        {
            if (videoFiles.Count == 0)
            {
                return;
            }

            if (video.Position.TotalSeconds > 5)
            {
                video.Position = TimeSpan.Zero;
            }
            else
            {
                if (currentVideoIndex > 0)
                {
                    currentVideoIndex--;
                }
                else
                {
                    currentVideoIndex = videoFiles.Count - 1;
                }

                if (!history.Contains(video.Source.LocalPath))
                {
                    history.Add(video.Source.LocalPath);
                    historyListBox.ItemsSource = null;
                    historyListBox.ItemsSource = history.Select(f => System.IO.Path.GetFileName(f)).ToList();
                }
                video.Source = new Uri(videoFiles[currentVideoIndex]);
            }
        }

        bool playing = false;
        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            playing = !playing;
            if (playing)
            {
                img_play.Source = new BitmapImage(new Uri("pack://application:,,,/Ressourcen/pause.png"));
                timer.Start();
                video.Play();
                if (!history.Contains(video.Source.LocalPath))
                {
                    history.Add(video.Source.LocalPath);
                    historyListBox.ItemsSource = null;
                    historyListBox.ItemsSource = history.Select(f => System.IO.Path.GetFileName(f)).ToList();
                }
            }
            else
            {
                img_play.Source = new BitmapImage(new Uri("pack://application:,,,/Ressourcen/play-button.png"));
                timer.Stop();
                video.Pause();
            }
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            video.Stop();
        }

        private void MuteButton_Click(object sender, RoutedEventArgs e)
        {
            ((PlayerViewModel)DataContext).Volume = 0;
        }

        private void PlaylistListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            currentVideoIndex = playlistListBox.SelectedIndex;
            if (currentVideoIndex >= 0 && currentVideoIndex < videoFiles.Count)
            {
                video.Source = new Uri(videoFiles[currentVideoIndex]);
                video.Play();
                playing = true;
                img_play.Source = new BitmapImage(new Uri("pack://application:,,,/Ressourcen/pause.png"));
                timer.Start();
            }
        }

        private void HistoryListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            {
                video.Source = new Uri(history[historyListBox.SelectedIndex]);
                video.Play();
                playing = true;
                img_play.Source = new BitmapImage(new Uri("pack://application:,,,/Ressourcen/pause.png"));
                timer.Start();
            }
        }
    }
}