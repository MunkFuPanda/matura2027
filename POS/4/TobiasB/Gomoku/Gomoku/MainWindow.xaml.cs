using System.Collections.ObjectModel;
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

namespace Gomoku
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    public enum SelController
    {
        Local,
        NetworkServer,
        NetworkClient,
        Computer
    }

    public partial class MainWindow : Window
    {

        // i property changed implementieren
        // mit communitytoolkit.mvvm nuget paket


        public char current_player;
        public SelController selcon;

        public Controller controller;

        public static PlayingField playingField;
        public ObservableCollection<Field> view;

        public MainWindow()
        {
            InitializeComponent();

            cb_selsize.Items.Add(15);
            cb_selsize.Items.Add(19);
            cb_selsize.Items.Add(11);
            cb_selsize.SelectedIndex = 0;

            cb_selcontroller.Items.Add(SelController.Local);
            cb_selcontroller.Items.Add(SelController.NetworkServer);
            cb_selcontroller.Items.Add(SelController.NetworkClient);
            cb_selcontroller.Items.Add(SelController.Computer);
            cb_selcontroller.SelectedIndex = 0;

            
        }

        private void lv_playarea_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (lv_playarea.SelectedItem is Field clickedfield)
            {
                int res = controller.Input(playingField, clickedfield.x, clickedfield.y, current_player);

                if (res == 0)
                {

                    if (current_player == 'A')
                    {
                        current_player = 'B';
                        this.Dispatcher.Invoke(new Action(() =>
                        {
                            lb_current_player.Content = "Player B";
                        }));

                    }
                    else
                    {
                        current_player = 'A';
                        this.Dispatcher.Invoke(new Action(() =>
                        {
                            lb_current_player.Content = "Player A";
                        }));
                    }
                }

                else if (res == 1)
                {
                    MessageBox.Show("Feld schon besetzt", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else if (res == 2)
                {

                    MessageBox.Show("Spieler " + current_player + " hat gewonnen", "Gratulation", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else if (res == 3)
                { 

                    MessageBox.Show("Unentschieden", "Spiel vorbei", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        private void startButton_Click(object sender, RoutedEventArgs e)
        {

            playingField = new PlayingField((int)cb_selsize.SelectedItem);

            current_player = 'A';
            selcon = (SelController)cb_selcontroller.SelectedItem;

            if (selcon == SelController.Local)
            {
                controller = new LocalController();
            }
            else if (selcon == SelController.NetworkServer)
            {
                // network connection, select ip, connection handeling, networkinglibrary
                controller = new NetworkController(true, playingField);
            }
            else if (selcon == SelController.NetworkClient)
            {
                controller = new NetworkController(false, playingField);
            }
            else if (selcon == SelController.Computer)
            {
                // computer programmieren, irgendwo random die sachen hin
                controller = new ComputerController();
            }
            else
            {
                controller = null;
            }

            

            view = new ObservableCollection<Field>(playingField.board.Cast<Field>().ToList());
            lv_playarea.ItemsSource = view;

            this.Dispatcher.Invoke(new Action(() =>
            {
                lb_current_player.Content = "Player A";
            }));

        }
    }
}