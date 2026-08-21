using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Controls;
using System.Security.Cryptography.Xml;

namespace Drei_Raucher
{
    public enum Zutaten {
        None,
        Tabak,
        Zigarettenpapier,
        Streichholz
    }
    internal class Smoker_Agent
    {
        private int id;
        private int smoke_time;
        private Zutaten self_storage;
        private List<Zutaten> needs = new List<Zutaten>();
        private Zutaten need_1;
        private Zutaten need_2;
        private static Table table = new Table();

        private int isSleep = 0;

        private Label status_label;
        private Label need_label;

        private Label table1_label;
        private Label table2_label;

        public Smoker_Agent(int id, int smoke_time, Zutaten self_storage, List<Zutaten> needs, Label table1_label, Label table2_label, Label status_label, Label need_label, Zutaten need_1, Zutaten need_2)
        {
            this.id = id;
            this.smoke_time = smoke_time;
            this.self_storage = self_storage;
            this.needs = needs;
            this.table1_label = table1_label;
            this.table2_label = table2_label;
            this.status_label = status_label;
            this.need_label = need_label;
            this.need_1 = need_1;
            this.need_2 = need_2;
        }

        public void Smoker()
            // auch nur ein produkt nutzbar heißt ich kann auch nur eine zutat nehmen die mir fehlt
            // stoppen mit interrupt und while schleife damit das aufhört
            // thread sleep außerhalb von thread
        {

            try
            {
                while (true)
                {
                    lock (table)
                    {
                        while (!table.tableFull())
                        {
                            Monitor.Wait(table);
                            status_label.Dispatcher.Invoke(new Action(() =>
                            {
                                status_label.Content = "wartet";
                                status_label.Background = Brushes.White;
                            }));
                        }
                        if (needs.Contains(table.ZutatEins) == true || needs.Contains(table.ZutatZwei) == true)
                        {
                            // neu überlegen und gscheit machen

                            if (needs.Contains(table.ZutatEins) == true)
                            {
                                table.ZutatEins = Zutaten.None;
                                need_label.Dispatcher.Invoke(new Action(() =>
                                {
                                    need_label.Content = (table.ZutatEins == need_1 ? need_2.ToString() : need_1.ToString());
                                    need_label.Background = Brushes.White;
                                }));

                                needs.Remove(table.ZutatEins);
                            }
                            if (needs.Contains(table.ZutatZwei) == true)
                            {

                                table.ZutatEins = Zutaten.None;
                                int temp = needs.IndexOf(table.ZutatEins);
                                need_label.Dispatcher.Invoke(new Action(() =>
                                {
                                    need_label.Content = (table.ZutatZwei == need_1 ? need_2.ToString() : need_1.ToString());
                                    need_label.Background = Brushes.White;
                                }));

                                needs.Remove(table.ZutatZwei);
                            }

                            if (needs.Contains(table.ZutatEins) == true && needs.Contains(table.ZutatZwei) == true)
                            {
                                table.ZutatZwei = Zutaten.None;
                                table.ZutatEins = Zutaten.None;
                                need_label.Dispatcher.Invoke(new Action(() =>
                                {
                                    need_label.Content = "";
                                    need_label.Background = Brushes.White;
                                }));
                                needs.Clear();
                            }

                            if (needs.Count == 0)
                            {


                                isSleep = 1;

                            }
                            Monitor.PulseAll(table);
                        }

                    }

                    lock (this)
                    {
                        if (isSleep == 1)
                        {
                            need_label.Dispatcher.Invoke(new Action(() =>
                            {
                                need_label.Content = "";
                                need_label.Background = Brushes.White;
                            }));

                            status_label.Dispatcher.Invoke(new Action(() =>
                            {
                                status_label.Content = "raucht";
                                status_label.Background = Brushes.Red;
                            }));

                            Thread.Sleep(smoke_time);

                            status_label.Dispatcher.Invoke(new Action(() =>
                            {
                                status_label.Content = "wartet";
                                status_label.Background = Brushes.White;
                            }));

                            needs.Add(Zutaten.Tabak);
                            needs.Add(Zutaten.Zigarettenpapier);
                            needs.Add(Zutaten.Streichholz);
                            needs.Remove(self_storage);

                            need_label.Dispatcher.Invoke(new Action(() =>
                            {
                                need_label.Content = needs[0].ToString() + ", " + needs[1].ToString();
                                need_label.Background = Brushes.White;
                            }));


                            isSleep = 0;
                            Thread.Sleep(1000);
                        }
                    }
                    
                }
            }
            catch (ThreadInterruptedException)
            {
                return;
            }

            
            
        }

        public void Agent()
        {
            try
            {
                while (true)
                {
                    lock (table)
                    {
                        while (table.tableFull()) Monitor.Wait(table);
                        Random random = new Random();
                        int r = random.Next(1, 4);
                        int r2 = random.Next(1, 4);
                        while (r == r2)
                        {
                            r2 = random.Next(1, 3);
                        }
                        table.ZutatEins = (Zutaten)r;
                        table.ZutatZwei = (Zutaten)r2;

                        table1_label.Dispatcher.Invoke(new Action(() =>
                        {
                            table1_label.Content = table.ZutatEins.ToString();
                            table1_label.Background = Brushes.White;
                        }));
                        table2_label.Dispatcher.Invoke(new Action(() =>
                        {
                            table2_label.Content = table.ZutatZwei.ToString();
                            table2_label.Background = Brushes.White;
                        }));

                        Monitor.PulseAll(table);
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
