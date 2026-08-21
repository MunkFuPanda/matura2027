using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace Kreuzung
{
    internal class Crossroad
    {
        private CountdownEvent countdownEvent = new CountdownEvent(1);

        private ListBox middle;

        public Crossroad (ListBox middle)
        {
            this.middle = middle;
        }

        // schauen, dass ich nicht immer addMiddle und addTo kopieren muss

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

        virtual public void cross(TextBox status, ListBox from, ListBox to, Direction direction)
        {

            from.Dispatcher.Invoke(new Action(() =>
            {
                from.Items.Add(status);
            }));

            while (countdownEvent.CurrentCount != 1) 
            {
                
            }

            lock (this)
            {

                addMiddle(status, from);

                // main code
                countdownEvent.Signal();
                Thread.Sleep(1000);
                countdownEvent.Reset();

                addTo(status, to);
            }

            

        }
    }
}
