using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Drei_Raucher
{
    public partial class MainWindow : Window
    {
        Raucher[] smoker = new Raucher[3];
        Thread[] threads = new Thread[4];
        Table table;

        public enum Waren
        {
            Tabak,
            Papier,
            Streichholz
        }
        public MainWindow()
        {
            InitializeComponent();
            Random Random = new Random();
            table = new Table(((Waren)Random.Next(3)).ToString(), ((Waren)Random.Next(3)).ToString());
        }

        public void startSmoking(Object sender, EventArgs e)
        {
            smoker[0] = new Raucher(Waren.Tabak.ToString(), this.statusA, Waren.Papier.ToString(), Waren.Streichholz.ToString(), int.Parse(this.timeToSmoke.Text), table, takenA);
            smoker[1] = new Raucher(Waren.Papier.ToString(), this.statusB, Waren.Tabak.ToString(), Waren.Streichholz.ToString(), int.Parse(this.timeToSmoke.Text), table, takenB);
            smoker[2] = new Raucher(Waren.Streichholz.ToString(), this.statusC, Waren.Tabak.ToString(), Waren.Papier.ToString(), int.Parse(this.timeToSmoke.Text), table, takenC);

            for (int i = 0; i < 3; i++)
            {
                threads[i] = new Thread(new ThreadStart(smoker[i].start));
            }
            threads[threads.Length - 1] = new Thread(new ThreadStart(pulser));
            for (int i = 0; i < threads.Length; i++)
            {
                threads[i].Start();
            }
        }

        public void endSmoking(Object sender, EventArgs e)
        {
            for (int i = 0; i < 3; i++)
            {
                threads[i].Interrupt();
            }
            statusA.Text = "nicht gestartet";
            statusB.Text = "nicht gestartet";
            statusC.Text = "nicht gestartet";
            statusA.Background = Brushes.White;
            statusB.Background = Brushes.White;
            statusC.Background = Brushes.White;

            takenA.Text = "";
            takenB.Text = "";
            takenC.Text = "";
        }

        public void pulser()
        {
            try
            {
                while (true)
                {
                    lock(table)
                    {
                        if (table.availableObject1 != null && table.availableObject2 != null) Monitor.Wait(table);
                        setIngredients();
                        Monitor.PulseAll(table);
                    }
                }
            } catch(ThreadInterruptedException)
            {
                return;
            }
        }
        
        public void setIngredients()
        {
            Random Random = new Random();
            string object1 = ((Waren)Random.Next(3)).ToString();
            string object2 = ((Waren)Random.Next(3)).ToString();
            while (object2.Equals(object1))
            {
                object2 = ((Waren)Random.Next(3)).ToString();
            }
            table.availableObject1 = object1;
            table.availableObject2 = object2;
        }
    }
}
