using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
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
using Network;
using Server;

namespace Client {
    public partial class MainWindow : Window {

        private static Transfer<MSG> transfer;
        private static TcpClient client = new TcpClient("localhost", 12345);
        private static ObservableCollection<List<Letter>> ItemsList = new ObservableCollection<List<Letter>>();
        private static string lastGuess;
        public MainWindow() {
            InitializeComponent();
            Results.ItemsSource = ItemsList;
            transfer = new Transfer<MSG>(client);

            transfer.OnMessageReceived += (object sender, MSG msg) => {
                List<Letter> letters = new List<Letter>();
                for (int i = 0; i < msg.Guess.Length; i++) {
                    SolidColorBrush color = Brushes.Gray;
                    if (msg.Results[i] == MSG.Result.CorrectPosition) {
                        color = Brushes.Green;
                    } else if (msg.Results[i] == MSG.Result.WrongPosition) {
                        color = Brushes.Yellow;
                    }
                    letters.Add(new Letter { Text = msg.Guess[i].ToString().ToUpper(), Color = color });
                }

                Application.Current.Dispatcher.Invoke(() => {
                    ItemsList.Add(letters);
                });

                if (msg.Results.All(r => r == MSG.Result.CorrectPosition)) {
                    Application.Current.Dispatcher.Invoke(() => {
                        MessageBox.Show("Congratulations! You've guessed the word!");
                    });
                } else if (ItemsList.Count >= 6) {
                    Application.Current.Dispatcher.Invoke(() => {
                        MessageBox.Show("Game over! You've used all your tries.");
                    });
                }
            };
        }

        private void Submit_Click(object sender, RoutedEventArgs e) {
            if (transfer != null) {
                lastGuess = Input.Text;

                if (string.IsNullOrWhiteSpace(lastGuess)) {
                    MessageBox.Show("Please enter a valid guess.");
                    return;
                }

                if (lastGuess.Length != 5) {
                    MessageBox.Show("Please enter a 5-letter guess.");
                    return;
                }

                transfer.Send(new MSG { Guess = lastGuess.ToLower() });
                Input.Clear();
            }
        }
    }
}