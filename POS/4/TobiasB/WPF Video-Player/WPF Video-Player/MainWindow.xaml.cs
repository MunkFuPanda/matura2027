using Microsoft.VisualBasic;
using Microsoft.Win32;
using System.Collections.ObjectModel;
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
using System.Windows.Threading;

namespace WPF_Video_Player
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ObservableCollection<Video> playlist = new ObservableCollection<Video>();
        private ObservableCollection<Video> playhistory = new ObservableCollection<Video>();

        private int currentvideoindex = -1;

        private DispatcherTimer timer;

        public MainWindow()
        {
            InitializeComponent();

            // for testing
            // media.Source = new Uri("C:\\Users\\Tobias\\Downloads\\POS sample videos\\Fat Cat.mp4");

            // media settings (default) Manual!!! sonst kann man nicht im Code steuern
            media.LoadedBehavior = MediaState.Manual;
            media.IsMuted = false;
            media.Volume = 0;
            volumeSlider.Value = 0;
            videoprogressbar.Value = 0;

            playlistbox.ItemsSource = playlist;
            playhistorybox.ItemsSource = playhistory;

            listpanel.Visibility = Visibility.Visible;

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;

        }



        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            media.Volume = (double)e.NewValue;
        }

        #region CommandBindings

        private void Play_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (media == null)
            {
                return;
            }

            if (media.Source == null) 
            { 
                e.CanExecute = false;
            }
            else
            {
                e.CanExecute = true;
            }
                
        }

        private void Play_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            media.Play();
            timer.Start();
        }

        private void Pause_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (media == null)
            {
                return;
            }

            if (media.CanPause == true)
            {
                e.CanExecute = true;
            }
            else
            {
                e.CanExecute = false;
            }

        }

        private void Pause_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            media.Pause();
            timer.Stop();
        }

        private void Stop_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (media == null)
            {
                return;
            }

            if (media.Source != null)
            {
                e.CanExecute = true;
            }
            else
            {
                e.CanExecute = false;
            }

        }

        private void Stop_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            media.Stop();
            timer.Stop();
            videoprogressbar.Value = 0;
        }

        private void Mute_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void Mute_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (media.IsMuted == true)
            {
                media.IsMuted = false;
            }
            else
            {
                media.IsMuted = true;
            }
        }


        #endregion

        private void togglelist_Click(object sender, RoutedEventArgs e)
        {
            if (listpanel.IsVisible == true)
            {
                listpanel.Visibility = Visibility.Collapsed;
            }
            else
            {
                listpanel.Visibility = Visibility.Visible;
            }
        }

        private void videoload_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "MP4-Dateien|*.mp4| Alle Dateien | *.* ";
            bool? result = dialog.ShowDialog();
            if (result == true)
            {
                Video newvideo = new Video(dialog.SafeFileName, new Uri(dialog.FileName));
                playlist.Add(newvideo);
            }
            currentvideoindex = 0;
        }

        private void videodirload_Click(object sender, RoutedEventArgs e)
        {
            OpenFolderDialog dialog = new OpenFolderDialog();
            bool? result = dialog.ShowDialog();
            if (result == true)
            {
                foreach (String item in Directory.GetFiles(dialog.FolderName))
                {
                    string safeFileName = Path.GetFileName(item);
                    Video newvideo = new Video(safeFileName, new Uri(item));
                    playlist.Add(newvideo);
                }
            }

            currentvideoindex = 0;
        }

        private void media_MediaEnded(object sender, RoutedEventArgs e)
        {
            if (playlist.Count == 0)
            {
                return;
            }

            try
            {
                timer.Stop();
                foreach (Video vid in playlist)
                {
                    if (media.Source == vid.Path)
                    {
                        playhistory.Add(vid);
                        break;
                    }
                }

                currentvideoindex++;
                media.Source = playlist.ElementAt(currentvideoindex).Path;

                timer.Start();
                media.Play();
            }
            catch (Exception ex)
            {
                media.Source = null;
                return;
            }


        }

        private void playlistbox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            timer.Stop();
            media.Stop();

            foreach (Video vid in playlist)
            {
                if (playlistbox.SelectedItem.ToString() == vid.Name)
                {
                    media.Source = vid.Path;
                    currentvideoindex = playlist.IndexOf(vid);

                    timer.Start();
                    media.Play();
                    return;
                }
            }


            
        }

        private void Prev_Click(object sender, RoutedEventArgs e)
        {
            if (playlist.Count == 0)
            {
                return;
            }

            try
            {
                timer.Stop();
                foreach (Video vid in playlist)
                {
                    if (media.Source == vid.Path)
                    {
                        playhistory.Add(vid);
                        break;
                    }
                }

                currentvideoindex--;
                media.Source = playlist.ElementAt(currentvideoindex).Path;

                timer.Start();
                media.Play();
            }
            catch (Exception ex)
            {
                media.Source = null;
                return;
            }
        }

        private void Next_Click(object sender, RoutedEventArgs e)
        {
            if (playlist.Count == 0)
            {
                return;
            }

            try
            {
                timer.Stop();
                foreach (Video vid in playlist)
                {
                    if (media.Source == vid.Path)
                    {
                        playhistory.Add(vid);
                        break;
                    }
                }

                currentvideoindex++;
                media.Source = playlist.ElementAt(currentvideoindex).Path;


                timer.Start();
                media.Play();
            }
            catch (Exception ex)
            {
                media.Source = null;
                return;
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (media.NaturalDuration.HasTimeSpan)
            {
                double progress = media.Position.TotalSeconds /
                                  media.NaturalDuration.TimeSpan.TotalSeconds * 100;
                videoprogressbar.Value = progress;
            }
        }
    }
}