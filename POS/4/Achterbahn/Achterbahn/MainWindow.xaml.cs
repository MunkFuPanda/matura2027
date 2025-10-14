using System.Diagnostics;
using System.IO.Pipes;
using System.Net.NetworkInformation;
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

namespace Achterbahn
{

    public partial class MainWindow : Window
    {
        static int x = 0; // Anzahl der Passagiere die Mitfahren können
        private int t = 5; // Anzahl wie oft der Zug fährt
        private int time_driving = 5000;

        AutoResetEvent rideFinished = new AutoResetEvent(false); // Fahrt vorbei
        AutoResetEvent canBoard = new AutoResetEvent(false);     // Freigabe für Passagiere zum Einsteigen
        AutoResetEvent allBoarded = new AutoResetEvent(false);

        Object queuelock = new object();

        private int currentpassengers = 0;

        public MainWindow()
        {
            InitializeComponent();

        }

        public void start(Object sender, EventArgs e)
        {
            x = int.Parse(seats.Text);
            int j = int.Parse(p.Text);
            for (int i = 1; i <= j; i++)
            {
                int passengerid = i;
                Thread thread = new Thread(() => passenger(passengerid));
                thread.Start();
            }

            Thread train1 = new Thread(() => train());
            train1.Start();
        }
        private void passenger(int id)
        {
            Random rand = new Random();
            while (true)
            {
                Dispatcher.Invoke(new Action(() =>
                {
                    if (!(wandering_list.Items.Contains(id))) wandering_list.Items.Add(id);
                }));

                Thread.Sleep(rand.Next(1000, 20000));

                Dispatcher.Invoke(new Action(() =>
                {
                    wandering_list.Items.Remove(id);
                    if (!(waiting_list.Items.Contains(id))) waiting_list.Items.Add(id);
                }));
                
                canBoard.WaitOne();
                lock (queuelock) {
                    if (currentpassengers != x)
                    {
                        Dispatcher.Invoke(new Action(() =>
                        {
                            waiting_list.Items.Remove(id);
                            if (!(driving_list.Items.Contains(id))) driving_list.Items.Add(id);
                        }));
                        currentpassengers++;
                    } else
                    {
                        continue;
                    }
                }

                rideFinished.WaitOne();
                Dispatcher.Invoke(new Action(() =>
                {
                    driving_list.Items.Remove(id);
                }));
            }
        }

        private void train()
        {
            Random rand = new Random();
            while (true)
            {
                lock (this)
                {
                    while (currentpassengers < x)
                    {
                        Thread.Sleep(1000);
                        canBoard.Set();
                    }
                }
                canBoard.Reset();
                allBoarded.Set();

                Dispatcher.Invoke(new Action(() =>
                {
                    status_train.Background = Brushes.Green;
                    status_train.Content = "Fährt";
                }));
                Thread.Sleep(5000);
                Dispatcher.Invoke(new Action(() =>
                {
                    status_train.Background = Brushes.Red;
                    status_train.Content = "Waiting";
                }));
                allBoarded.Reset();
                
                while (currentpassengers > 0) { 
                    Thread.Sleep(1000);
                    currentpassengers--;
                    rideFinished.Set();
                }
            }

        }
    }
}