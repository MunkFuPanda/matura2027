using Microsoft.Win32;
using System;
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
using static System.Net.Mime.MediaTypeNames;

namespace Image_Rotator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    

    public enum Mirroring
    {
        Horizontal,
        Vertical
    }
    public partial class MainWindow : Window
    {

        private String work_dir;

        public MainWindow()
        {
            InitializeComponent();

            cb_spiegelung.Items.Add("Horizontal");
            cb_spiegelung.Items.Add("Vertikal");

            cb_spiegelung.SelectedIndex = 0;

            cb_rotation.Items.Add(0);
            cb_rotation.Items.Add(90);
            cb_rotation.Items.Add(180);
            cb_rotation.Items.Add(270);

            cb_rotation.SelectedIndex = 0;


        }

        private void open_folder_button_Click(object sender, RoutedEventArgs e)
        {
            OpenFolderDialog file_dialog = new OpenFolderDialog();
            file_dialog.ShowDialog();

            work_dir = file_dialog.FolderName;
            tb_path.Text = work_dir;

        }

        private void Worker(object dat)
        {
            data image = dat as data;

            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            if (image.mirroring == Mirroring.Horizontal)
            {
                encoder.FlipHorizontal = true;
                encoder.FlipVertical = false;
            }
            else
            {
                encoder.FlipVertical = true;
                encoder.FlipHorizontal = false;
            }
                           
            encoder.QualityLevel = image.quality;

            if (image.rotation == 90)
            {
                encoder.Rotation = Rotation.Rotate90;
            }
            else if (image.rotation == 180)
            {
                encoder.Rotation = Rotation.Rotate180;
            }
            else if (image.rotation == 270)
            {
                encoder.Rotation = Rotation.Rotate270;
            }
            else
            {
                encoder.Rotation = Rotation.Rotate0;
            }

            encoder.Frames.Add(BitmapFrame.Create(new Uri(image.item.ToString(), UriKind.Relative)));

            string filename = System.IO.Path.GetFileName(image.item.ToString());

            FileStream stream = new FileStream(image.save_dir + "\\" + filename, FileMode.Create);
            encoder.Save(stream);

            progressbar.Dispatcher.Invoke(new Action(() =>
            {
                progressbar.Value = ++progressbar.Value;
            }));

            stream.Close();


        }

        private void start_button_Click(object sender, RoutedEventArgs e)
        {
            Directory.CreateDirectory(work_dir + "\\bearbeitet");

            progressbar.Maximum = Directory.GetFiles(work_dir, "*", SearchOption.AllDirectories).Length;
            progressbar.Value = 0;

            foreach (var item in Directory.GetFiles(work_dir))
            {
                Mirroring mirroring = (Mirroring) cb_spiegelung.Items.CurrentPosition;
                int rot = (int)cb_rotation.SelectedItem;
                int qual = (int)jpeg_quality.Value;

                data d = new data(progressbar, rot, mirroring, qual, work_dir, work_dir + "\\bearbeitet", item);
                ThreadPool.QueueUserWorkItem(new WaitCallback(Worker), d);

            }
        }
    }
}