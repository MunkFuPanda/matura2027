using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace Dining_Philosophers
{
    public class philosopher
    {
        public int id;
        public string philosopherName;
        public string status;
        public int thinkingTime;
        public int thinkingVariance;
        public int eatingTime;
        public int eatingVariance;
        public int pickUpTime;
        public TextBox textbox;
        public fork leftFork;
        public fork rightFork;

        public philosopher(int id, TextBox textbox, fork leftFork, fork rightFork, int thinkingTime, int thinkingVariance, int eatingTime, int eatingVariance, int pickUpTime)
        {
            this.id = id;
            this.textbox = textbox;
            this.leftFork = leftFork;
            this.rightFork = rightFork;
            this.thinkingTime = thinkingTime;
            this.thinkingVariance = thinkingVariance;
            this.eatingTime = eatingTime;
            this.eatingVariance = eatingVariance;
            this.pickUpTime = pickUpTime;
        }

        public void begin()
        {
            try
            {
                while (true)
                {
                    think();
                    eat();
                }
            }
            catch(ThreadInterruptedException)
            {
                return;
            }
        }

        public void eat()
        {
            Random Random = new Random();
            textbox.Dispatcher.Invoke(new Action(() =>
            {
                textbox.Text = "wartet";
                textbox.Background = Brushes.Red;
            }));
            lock(leftFork)
            {
                Thread.Sleep(pickUpTime);
                lock(rightFork)
                {
                    Thread.Sleep(pickUpTime);
                    textbox.Dispatcher.Invoke(new Action(() =>
                    {
                        textbox.Text = "isst";
                        textbox.Background = Brushes.Green;
                    }));
                    Thread.Sleep((int)(eatingTime + Random.NextInt64(-(eatingVariance), eatingVariance)));
                }
            }
        }

        public void think()
        {
            Random Random = new Random();
            textbox.Dispatcher.Invoke(new Action(() =>
            {
                textbox.Text = "denkt";
                textbox.Background = Brushes.White;
            }));
            Thread.Sleep((int)(thinkingTime + Random.NextInt64(-(thinkingVariance), thinkingVariance)));
        }
    }
}
