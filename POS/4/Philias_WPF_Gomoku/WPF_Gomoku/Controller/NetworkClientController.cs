using NetworkLibrary;
using System.Net.Sockets;
using System.Windows;
using System.Windows.Threading;
using WPF_Gomoku;
using WPF_Gomoku.Controller;
using WPF_Gomoku.Model;

namespace WPF_Gomoku.Controller
{
    public class NetworkClientController : IController
    {
        TcpClient client;
        Transfer<Item> transfer;
        public BoardModel Board { get; set; }

        public NetworkClientController(int boardSize, string IPadresse, int Port)
        {
            Board = new BoardModel(boardSize);

            try
            {
                client = new TcpClient(IPadresse, Port);
                transfer = new Transfer<Item>(client);
                transfer.OnMessageReceived += Transfer_OnMessageReceived;
                transfer.OnDisconnect += Transfer_OnDisconnect;
            }
            catch (SocketException)
            {
                System.Windows.MessageBox.Show("Could not connect to server.");
                throw new Exception("Could not connect to server.");
            }
        }

        private void Transfer_OnMessageReceived(object? sender, Item e)
        {
            if (e.X >= 1000)
            {
                int serverSize = e.X - 1000;
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Board.Cells.Clear();
                    Board.size = serverSize;
                    for (int i = 0; i < serverSize; i++)
                    {
                        for (int j = 0; j < serverSize; j++)
                        {
                            Board.Cells.Add(new Item(i, j, ""));
                        }
                    }
                });
            }
            else
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Board.Cells[e.X * Board.size + e.Y].Charater = e.Charater;
                    if (e.Charater == "X")
                        turn = true;
                });
            }
        }

        private void Transfer_OnDisconnect(object? sender, EventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                System.Windows.MessageBox.Show("Disconnected from server.");
            });
        }

        public void Start() { }
        public void Stop() { }

        public bool turn = true; // true for X, false for O

        public void OnCellClicked(Item item)
        {
            if (item.Charater == "" && turn)
            {
                item.Charater = "O";
                transfer.SendMessage(new Item(item.X, item.Y, "O"));
                turn = false;
                int result = ((IController)this).CheckWin();
            }
        }
    }
}
