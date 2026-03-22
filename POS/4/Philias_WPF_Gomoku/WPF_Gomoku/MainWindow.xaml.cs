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
using WPF_Gomoku.Controller;
using WPF_Gomoku.Controller;

namespace WPF_Gomoku
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public IController? icontroller;
        private bool over = false;
        public MainWindow()
        {

            InitializeComponent();
            var dialog = new StartWindow();
            if (dialog.ShowDialog() == true)
            {
                CreateController(dialog.GameMode, dialog.size, dialog.IPAddress, dialog.Port);

                //BoardGrid1.Rows = dialog.size;
                //BoardGrid1.Columns = dialog.size;
            }
            else
            {
                Application.Current.Shutdown();
                return;
            }
            BoardGrid.ItemsSource = icontroller.Board.Cells;
            icontroller?.Start();

        }

        private void CreateController(int gameMode, int size, string iPAddress, int port)
        {
            switch (gameMode)
            {
                case 1:
                    icontroller = new LocalController(size);
                    break;
                case 2:
                    icontroller = new ComputerController(size);
                    break;
                case 3:
                    icontroller = new NetworkServerController(size, iPAddress, port);
                    break;
                case 4:
                    icontroller = new NetworkClientController(size, iPAddress, port);
                    break;
                default:
                    MessageBox.Show("Invalid game mode selected.");
                    Application.Current.Shutdown();
                    break;
            }
            
        }

        private void Cell_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (over) { return; }
            int result = ((IController)icontroller).CheckWin();
            if (result == 1)
            {
                over = true;
                StatusText.Text = "Player X wins!";
                return;
            }
            else if (result == 2)
            {
                over = true;
                StatusText.Text = "Player O wins!";
                return;
            }


            icontroller?.OnCellClicked((sender as Border)?.DataContext as Item);
            result = ((IController)icontroller).CheckWin();
            if (result == 1)
            {
                over = true;
                StatusText.Text = "Player X wins!";
            }
            else if (result == 2)
            {
                over = true;
                StatusText.Text = "Player O wins!";
            }
        }
    }
}