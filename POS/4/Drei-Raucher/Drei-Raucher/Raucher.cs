using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace Drei_Raucher
{
    internal class Raucher
    {
        public string infiniteProduct;
        public TextBox status;
        public string firstItemNeeded;
        public bool firstItem = false;
        public string secondItemNeeded;
        public bool secondItem = false;
        public int smokingTime;
        public Table table;
        public TextBox taken;
        public Raucher(string infiniteProduct, TextBox status, string firstItemNeeded, string secondItemNeeded, int smokingTime, Table table , TextBox taken)
        {
            this.infiniteProduct = infiniteProduct;
            this.status = status;
            this.firstItemNeeded = firstItemNeeded;
            this.secondItemNeeded = secondItemNeeded;
            this.smokingTime = smokingTime;
            this.table = table;
            this.taken = taken;
        }

        public void start()
        {
            try
            {
                while (true)
                {
                    status.Dispatcher.Invoke(new Action(() =>
                    {
                        status.Text = "waiting";
                        status.Background = Brushes.Green;
                        status.UpdateLayout();
                    }));
                    lock (table)
                    {
                        if (table.availableObject2 == null && table.availableObject1 == null)
                        {
                            Monitor.PulseAll(table);
                        }
                        else if (!firstItem && (firstItemNeeded == table.availableObject2 || firstItemNeeded == table.availableObject1))
                        {
                            firstItem = true;
                            if (firstItemNeeded == table.availableObject1)
                            {
                                table.availableObject1 = null;
                            }
                            else
                            {
                                table.availableObject2 = null;
                            }
                            taken.Dispatcher.Invoke(new Action(() => 
                            {
                                taken.Text = taken.Text + " " + firstItemNeeded;
                            }));
                        }
                        else if (!secondItem && (secondItemNeeded == table.availableObject2 || secondItemNeeded == table.availableObject1))
                        {
                            secondItem = true;
                            if (secondItemNeeded == table.availableObject2)
                            {
                                table.availableObject2 = null;
                            }
                            else
                            {
                                table.availableObject1 = null;
                            }
                            taken.Dispatcher.Invoke(new Action(() => 
                            {
                                taken.Text = taken.Text + " " + secondItemNeeded;
                            }));
                        }
                        if (!firstItem || !secondItem)
                        {
                            Monitor.Wait(table);
                        }
                    }
                    if (firstItem && secondItem)
                    {
                        firstItem = false;
                        secondItem = false;
                        taken.Dispatcher.Invoke(new Action(() =>
                        {
                            taken.Text = "";
                        }));
                        status.Dispatcher.Invoke(new Action(() =>
                        {
                            status.Text = "smoking";
                            status.Background = Brushes.Red;
                        }));
                        Thread.Sleep(smokingTime);
                    }
                }
            } catch(ThreadInterruptedException)
            {
                return;
            }
        }
    }
}
