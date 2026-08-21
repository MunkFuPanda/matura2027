using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace Kreuzung
{
    internal class LargeCrossroad : Crossroad
    {

        private int north_south_count;
        private int east_west_count;

        private ListBox middle;

        public LargeCrossroad(ListBox middle) : base(middle)
        {
            this.middle = middle;
        }

        private void addMiddle(TextBox status, ListBox from)
        {
            from.Dispatcher.Invoke(new Action(() =>
            {
                from.Items.Remove(status);
            }));

            status.Dispatcher.Invoke(new Action(() =>
            {
                status.Background = Brushes.Yellow;
            }));

            middle.Dispatcher.Invoke(new Action(() =>
            {
                middle.Items.Add(status);
            }));
        }

        private void addTo(TextBox status, ListBox to)
        {
            middle.Dispatcher.Invoke(new Action(() =>
            {
                middle.Items.Remove(status);
            }));

            status.Dispatcher.Invoke(new Action(() =>
            {
                status.Background = Brushes.Green;
            }));

            to.Dispatcher.Invoke(new Action(() =>
            {
                to.Items.Add(status);
            }));
        }


        override public void cross(TextBox status, ListBox from, ListBox to, Direction direction)
        {

            from.Dispatcher.Invoke(new Action(() =>
            {
                from.Items.Add(status);
            }));


            if (direction == Direction.North || direction == Direction.South)
            {
                lock (this)
                {
                    while (east_west_count > 0)
                    {
                        Monitor.Wait(this);
                    }
                }

                addMiddle(status, from);

                lock (this)
                {
                    north_south_count++;
                }

                Thread.Sleep(1000);

                addTo(status, to);

                lock (this)
                {
                    north_south_count--;
                    Monitor.PulseAll(this);
                }
              
            }
            else
            {
                lock (this)
                {
                    while (north_south_count > 0)
                    {
                        Monitor.Wait(this);
                    }
                }

                addMiddle(status, from);

                lock (this)
                {
                    east_west_count++;
                }

                Thread.Sleep(1000);

                addTo(status, to);

                lock (this)
                {
                    east_west_count--;
                    Monitor.PulseAll(this);
                }

                
            }
        }
    }
}
