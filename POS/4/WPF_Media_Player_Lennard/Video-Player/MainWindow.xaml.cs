using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
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

namespace Video_Player
{
    public partial class MainWindow : Window
    {

        public ObservableCollection<VideoList> videoFiles { get; set; }

        DispatcherTimer timer;
        public MainWindow()
        {
            InitializeComponent();
            videoFiles = new ObservableCollection<VideoList>();
            DataContext = this;

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;

            
        }

        private void playButton_Click(object sender, RoutedEventArgs e)
        {
            videoPlayer.Play();
            timer.Start();
        }

        private void pauseButton_Click(object sender, RoutedEventArgs e)
        {
            videoPlayer.Pause();
            timer.Stop();
        }

        private void stopButton_Click(object sender, RoutedEventArgs e)
        {
            videoPlayer.Stop();
            videoPlayer.Source = null;
            timer.Stop();
            progressBar.Value = 0;
        }

        private void loadButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Video Files|*.mp4;*.avi;*.mkv;*.mov;*.wmv|All Files|*.*";
            openFileDialog.Multiselect = false;
            if (openFileDialog.ShowDialog() == true)
            {
                videoFiles.Add(new VideoList(openFileDialog.SafeFileName, openFileDialog.FileName));
            }
        }

        private void loadFolderButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFolderDialog folderDialog = new OpenFolderDialog();
            string[] videoExtensions = { ".mp4", ".mkv", ".avi", ".mov", ".wmv" };

            if (folderDialog.ShowDialog() == true)
            {
                foreach (var file in Directory.GetFiles(folderDialog.FolderName))
                {
                    if (videoExtensions.Contains(System.IO.Path.GetExtension(file)))
                    {
                        videoFiles.Add(new VideoList(
                             System.IO.Path.GetFileName(file).ToString(),
                             file));
                        
                    }
                }
            }
        }

        private void fullscreenButton_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState == WindowState.Normal ? WindowState.Maximized : WindowState.Normal;
        }

        private void videoListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        { 
            if (videoListBox.SelectedItem is VideoList selectedVideo)
            {
                videoPlayer.Source = new System.Uri(selectedVideo.videoFilePath);
                /*
                videoPlayer.MediaOpened += (s, e) =>
                {
                    if (videoPlayer.NaturalDuration.HasTimeSpan)
                    {
                        progressBar.Minimum = 0;
                        progressBar.Maximum = videoPlayer.NaturalDuration.TimeSpan.TotalSeconds;
                    }
                };*/

                Dispatcher.BeginInvoke(new Action(() =>
                {
                    if (videoPlayer.NaturalDuration.HasTimeSpan)
                    {
                        progressBar.Minimum = 0;
                        progressBar.Maximum = videoPlayer.NaturalDuration.TimeSpan.TotalSeconds;
                    }
                }), DispatcherPriority.Loaded);
                timer.Start();
                videoPlayer.Play();
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (videoPlayer.NaturalDuration.HasTimeSpan)
            {
                double progress = videoPlayer.Position.TotalSeconds /
                                  videoPlayer.NaturalDuration.TimeSpan.TotalSeconds * 100;
                progressBar.Value = progress;
            }
        }

        private void progressBar_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (sender is ProgressBar pb && pb.ActualWidth > 0)
            {
                double clickX = e.GetPosition(pb).X;
                double ratio = clickX / pb.ActualWidth;

                double targetSeconds = ratio * videoPlayer.NaturalDuration.TimeSpan.TotalSeconds;

                videoPlayer.Position = TimeSpan.FromSeconds(targetSeconds);

                progressBar.Value = targetSeconds;
            }
        }
    }
}