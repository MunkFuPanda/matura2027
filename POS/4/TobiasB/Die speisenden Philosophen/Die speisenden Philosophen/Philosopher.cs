using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace Die_speisenden_Philosophen
{
    internal class Philosopher
    {
        private int id;

        public int AVG_THINK { get => avg_think; set { avg_think = value; } }
        public int VAR_THINK { get => var_think; set { var_think = value; } }
        public int AVG_EAT { get => avg_eat; set { avg_eat = value; } }
        public int VAR_EAT { get => var_eat; set { var_eat = value; } }
        public int TAKE_FORK { get => take_fork; set { take_fork = value; } }

        public int I_END { get => i; set { i = value; } }


        private static int avg_think = 0;
        private static int var_think = 0;
        private static int avg_eat = 0;
        private static int var_eat = 0;
        private static int take_fork = 0;

        private Fork fork_left;
        private Fork fork_right;

        private Label status;

        private int i = 0;

        public Philosopher(int id, Label status, Fork left, Fork right)
        {
            this.id = id;
            this.status = status;
            fork_left = left;
            fork_right = right;
        }


        public void ThinkingEatingPhilosopher()
        {

            // try catch interrupt exception

            try
            {
                while (i == 0)
                {
                    Random random = new Random();
                    // check if any of this can get null
                    int think_sleep = random.Next(avg_think - var_think, avg_think + var_think);
                    status.Dispatcher.Invoke(new Action(() =>
                    {
                        status.Content = "denkt";
                        status.Background = Brushes.White;
                    }));
                    Thread.Sleep(think_sleep);
                    status.Dispatcher.Invoke(new Action(() =>
                    {
                        status.Content = "Gabel aufnehmen";
                        status.Background = Brushes.Red;
                    }));
                    lock (fork_left)
                    {
                        Thread.Sleep(take_fork);
                        lock (fork_right)
                        {
                            Thread.Sleep(take_fork);
                            int eat_sleep = random.Next(avg_eat - var_eat, avg_eat + var_eat);
                            status.Dispatcher.Invoke(new Action(() =>
                            {
                                status.Content = "isst";
                                status.Background = Brushes.Green;
                            }));
                            Thread.Sleep(eat_sleep);
                        }

                    }

                }
            }
            catch (ThreadInterruptedException)
            {
                return;
            }
        }
    }
}
