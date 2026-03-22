using NetworkLibrary;
using System.Net;
using System.Net.Sockets;
using System.Windows;
using WPF_Gomoku;
using WPF_Gomoku.Controller;
using WPF_Gomoku.Model;

namespace WPF_Gomoku.Controller
{
    public class NetworkServerController : IController
    {
        public BoardModel Board { get; set; }
        public TcpListener listener;
        List<Transfer<Item>> clients = new();
        int size;

        public NetworkServerController(int boardSize, string iPAddress, int Port)
        {
            this.size = boardSize;
            Board = new BoardModel(boardSize);
            listener = new TcpListener(IPAddress.Parse(iPAddress), Port);
            listener.Start();
            ThreadPool.QueueUserWorkItem(_ => AcceptClients());
        }

        private void AcceptClients()
        {
            while (true)
            {
                try
                {
                    TcpClient client = listener.AcceptTcpClient();
                    var transfer = new Transfer<Item>(client);
                    clients.Add(transfer);

                    transfer.OnMessageReceived += Transfer_OnMessageReceived;
                    transfer.OnDisconnect += (s, e) =>
                    {
                        clients.Remove(transfer);
                    };

                    //versendet board size an neue connection
                    try
                    {
                        transfer.SendMessage(new Item(1000 + size, 0, "SIZE"));
                    }
                    catch (Exception)
                    {
                        // Client disconnected
                    }
                }
                catch (Exception)
                {
                    break;
                }
            }
        }

        private void Transfer_OnMessageReceived(object? sender, Item e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                Board.Cells[e.X * size + e.Y].Charater = e.Charater;
                turn = true;
            });

            // Broadcast the move to all connected clients
            foreach (var client in clients.ToList())
            {
                try
                {
                    client.SendMessage(e);
                }
                catch (Exception)
                {
                    // Client disconnected, will be cleaned up by OnDisconnect
                }
            }
        }

        public void Start() { }
        public void Stop()
        {
            listener.Stop();
        }

        public bool turn = true;

        public void OnCellClicked(Item item)
        {
            if (item.Charater == "" && turn)
            {
                item.Charater = "X";

                foreach (var client in clients.ToList())
                {
                    try
                    {
                        client.SendMessage(new Item(item.X, item.Y, item.Charater));
                        int result = ((IController)this).CheckWin();
                    }
                    catch (Exception)
                    {
                        // Client disconnected, will be cleaned up by OnDisconnect
                    }
                }

                turn = false;
            }
        }
    }
}
