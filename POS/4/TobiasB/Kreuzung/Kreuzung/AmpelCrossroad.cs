using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace Kreuzung
{
    internal class AmpelCrossroad : Crossroad
    {
        private ListBox middle;

        private Label ampel;

        private ManualResetEvent north_south_manual_reset = new ManualResetEvent(false);
        private ManualResetEvent east_west_manual_reset = new ManualResetEvent(false);
        
        public AmpelCrossroad(ListBox middle, Label ampel) : base(middle)
        {
            this.middle = middle;
            this.ampel = ampel;
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

        public override void cross(TextBox status, ListBox from, ListBox to, Direction direction)
        {
            from.Dispatcher.Invoke(new Action(() =>
            {
                from.Items.Add(status);
            }));

            if (direction == Direction.North || direction == Direction.South)
            {
                
                north_south_manual_reset.WaitOne();

                addMiddle(status, from);

                Thread.Sleep(1000);

                addTo(status, to);
            }

            else
            { 
                east_west_manual_reset.WaitOne();

                addMiddle(status, from);

                Thread.Sleep(1000);

                addTo(status, to);
            }

            
        }

        // den thread interrupten mit dem reset button
        public void Ampel()
        {
            while (true)
            {
                ampel.Dispatcher.Invoke(new Action(() =>
                {
                    ampel.Background = Brushes.Green;
                    ampel.Content = "North South";
                }));
                north_south_manual_reset.Set();
                Thread.Sleep(3000);
                ampel.Dispatcher.Invoke(new Action(() =>
                {
                    ampel.Background = Brushes.Yellow;
                    ampel.Content = "Waiting";
                }));
                north_south_manual_reset.Reset();
                Thread.Sleep(2000);
                ampel.Dispatcher.Invoke(new Action(() =>
                {
                    ampel.Background = Brushes.Green;
                    ampel.Content = "East West";
                }));
                east_west_manual_reset.Set();
                Thread.Sleep(3000);
                ampel.Dispatcher.Invoke(new Action(() =>
                {
                    ampel.Background = Brushes.Yellow;
                    ampel.Content = "Waiting";
                }));
                east_west_manual_reset.Reset();
                Thread.Sleep(2000);
            }
        }
    }
}
