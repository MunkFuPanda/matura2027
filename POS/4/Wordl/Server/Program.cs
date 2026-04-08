using System.Net.Sockets;
using DataModels;
using LinqToDB;
using Network;
using Server;

internal class Program {
    internal Program() {
        Dictionary<Transfer<MSG>, Wordl> games = new Dictionary<Transfer<MSG>, Wordl>();
        TcpListener server = new TcpListener(System.Net.IPAddress.Any, 12345);
        WordlDB db = new WordlDB(new DataOptions().UseSQLite("Data Source=wordl.db;"));

        server.Start();
        Console.WriteLine("Listening on localhost:12345");

        while (true) {
            TcpClient client = server.AcceptTcpClient();
            Console.WriteLine("Client connected: " + client.Client.RemoteEndPoint);

            Task.Run(() => {
                Transfer<MSG> transfer = new Transfer<MSG>(client);
                Wordl game = new Wordl(db.Words.Skip(new Random().Next((int)(db.Words.Max(r => r.Id)))).First());
                Console.WriteLine("Word is " + game.word.WordColumn);

                transfer.OnMessageReceived += (object sender, MSG msg) => {
                    if (game.IsGameOver) {
                        Console.WriteLine("Received guess after game over. Ignoring.");
                        return;
                    }

                    if (string.IsNullOrWhiteSpace(msg.Guess)) {
                        Console.WriteLine("Received empty or whitespace guess.");
                        return;
                    }

                    if (msg.Guess.Length != 5) {
                        Console.WriteLine("Received invalid guess: " + msg.Guess);
                        return;
                    }

                    if (game.Tries >= 6) {
                        Console.WriteLine("Game over. Maximum number of tries reached.");
                        return;
                    }

                    if (msg.Results == null) {
                        msg.Results = new List<MSG.Result>();
                    }

                    for (int i = 0; i < msg.Guess.Length; i++) {
                        if (msg.Guess[i] == game.word.WordColumn[i]) {
                            Console.WriteLine("Letter " + msg.Guess[i] + " is correct and in the correct position.");
                            msg.Results.Add(MSG.Result.CorrectPosition);
                        } else if (game.word.WordColumn.Contains(msg.Guess[i])) {
                            Console.WriteLine("Letter " + msg.Guess[i] + " is correct but in the wrong position.");
                            msg.Results.Add(MSG.Result.WrongPosition);
                        } else {
                            Console.WriteLine("Letter " + msg.Guess[i] + " is incorrect.");
                            msg.Results.Add(MSG.Result.Incorrect);
                        }
                    }

                    if (msg.Results.All(r => r == MSG.Result.CorrectPosition)) {
                        game.IsGameOver = true;
                        Console.WriteLine("Client guessed the word correctly!");
                    } else if (game.Tries >= 5) {
                        game.IsGameOver = true;
                        Console.WriteLine("Game over. Client used all tries.");
                    }

                    game.Tries++;
                    transfer.Send(msg);
                };

                games.Add(transfer, game);
            });
        }
    }

    public static void Main(string[] args) {
        Program program = new Program();

        Console.ReadLine(); //Damit der Server nicht gleich wieder endet
    }
}