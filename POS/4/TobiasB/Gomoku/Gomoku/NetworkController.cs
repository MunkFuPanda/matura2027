using NetworkLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Gomoku
{
    public class NetworkController : Controller
    {
        // implementieren, damit ich genau weiß welcher spieler jetzt dran ist
        // damit man nicht platzieren kann, wenn ich nicht darf


        public char player;

        public char current_player = 'A';

        TcpClient client;

        Transfer<Message> transfer;

        PlayingField playingField = MainWindow.playingField;

        public NetworkController(bool server, PlayingField pf)
        {
            if (server)
            {

                player = 'A';
                TcpListener listener = new TcpListener(IPAddress.Any, 12345);

                listener.Start();

                client = listener.AcceptTcpClient();

                transfer = new Transfer<Message>(client);

                transfer.OnMessageReceived += HandleMessage;

                
            }
            else
            {

                player = 'B';
                // Port und IP änderbar machen
                client = new TcpClient("localhost", 12345);

                transfer = new Transfer<Message>(client);

                transfer.OnMessageReceived += HandleMessage;

                
            }
        }

        public override int Input(PlayingField playingField, int x, int y, char pl)
        {
            if (true)
            {
                Message m = new Message();
                m.y = y;
                m.x = x;

                for (int i = 0; i < playingField.size; i++)
                {
                    for (int j = 0; j < playingField.size; j++)
                    {
                        if (j == x && i == y)
                        {
                            if (playingField.board[i, j].colour == 'N')
                            {
                                playingField.board[i, j].Colour = (char)player;
                                transfer.SendMessage(m);
                            }
                            else
                            {
                                return 1;
                            }
                        }
                    }
                }

                // Win check

                int[][] directions =
                {
                new int[] {1, 0 },
                new int[] {0, 1},
                new int[] {1, 1},
                new int[] {1, -1},
            };

                foreach (int[] dir in directions)
                {
                    int count = 1;

                    count += CountInDirection(playingField, x, y, dir[0], dir[1], player);
                    count += CountInDirection(playingField, x, y, -dir[0], -dir[1], player);

                    if (count == 5)
                    {
                        return 2;
                    }
                    else
                    {
                        count = 0;

                        for (int i = 0; i < playingField.size; i++)
                        {
                            for (int j = 0; j < playingField.size; j++)
                            {
                                if (playingField.board[j, i].colour == 'N')
                                {
                                    count++;
                                    break;
                                }
                            }
                        }

                        if (count == 0)
                        {
                            return 3;

                        }
                    }

                }
                return 0;

            }
            if (current_player == player)
            {
                current_player = 'B';
            }
            else
            {
                current_player = 'A';
            }
            return 0;
        }

        private int CountInDirection(PlayingField pf, int x, int y, int dx, int dy, char player)
        {
            int count = 0;
            int curX = x + dx;
            int curY = y + dy;

            while (curX >= 0 && curX < pf.size && curY >= 0 && curY < pf.size && pf.board[curY, curX].colour == player)
            {
                count++;
                curX += dx;
                curY += dy;
            }

            return count;
        }

        private void HandleMessage(object sender, Message e)
        {
            if (true)
            {
                for (int i = 0; i < playingField.size; i++)
                {
                    for (int j = 0; j < playingField.size; j++)
                    {
                        if (j == e.x && i == e.y)
                        {
                            if (playingField.board[i, j].colour == 'N')
                            {
                                if (player == 'A')
                                {
                                    playingField.board[i, j].Colour = 'B';
                                }
                                else
                                {
                                    playingField.board[i, j].Colour = 'A';
                                }
                                    
                            }
                        }
                    }
                }

                // Win check

                int[][] directions =
                {
                new int[] {1, 0 },
                new int[] {0, 1},
                new int[] {1, 1},
                new int[] {1, -1},
            };

                foreach (int[] dir in directions)
                {
                    int count = 1;

                    count += CountInDirection(playingField, e.x, e.y, dir[0], dir[1], (char)current_player);
                    count += CountInDirection(playingField, e.x, e.y, -dir[0], -dir[1], (char)current_player);

                    if (count == 5)
                    {
                        MessageBox.Show("Gewonnen", "Gegner");
                        return;
                    }
                    else
                    {
                        count = 0;

                        for (int i = 0; i < playingField.size; i++)
                        {
                            for (int j = 0; j < playingField.size; j++)
                            {
                                if (playingField.board[j, i].colour == 'N')
                                {
                                    count++;
                                    break;
                                }
                            }
                        }

                        if (count == 0)
                        {
                            MessageBox.Show("Unentschieden");
                            return;
                        }
                    }

                }

            }
            else
            {
                return;
            }

            if (current_player != player)
            {
                current_player = 'B';
            }
            else
            {
                current_player = 'A';
            }
        }
    }
}
