using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace Achterbahn
{    
    internal class rollercoaster
    {
        private int id;
        private int walk_time = 2500; // in ms

        private ListBox wait_box;
        private ListBox cart_box;
        private ListBox walk_box;

        private cart roller_cart;

        public rollercoaster(int id, cart roller_cart, ListBox wait_box, ListBox cart_box, ListBox walk_box)
        {
            this.id = id;
            this.roller_cart = roller_cart;
            this.wait_box = wait_box;
            this.cart_box = cart_box;
            this.walk_box = walk_box;
        }



        public void Person()
        {
            while (true)
            {
                walk_box.Dispatcher.Invoke(new Action(() =>
                {
                    walk_box.Items.Add("Person " + id);
                }));
                Thread.Sleep((int) new Random().NextInt64(1000, 5000));

                walk_box.Dispatcher.Invoke(new Action(() =>
                {
                    walk_box.Items.Remove("Person " + id);
                }));
                wait_box.Dispatcher.Invoke(new Action(() =>
                {
                    wait_box.Items.Add("Person " + id);
                    wait_box.Items.MoveCurrentToLast();
                }
                ));

                roller_cart.Queue();

                if (!roller_cart.Working)
                {
                    wait_box.Dispatcher.Invoke(new Action(() =>
                    {
                        wait_box.Items.Remove("Person " + id);
                    }));
                    return;
                }

                wait_box.Dispatcher.Invoke(new Action(() =>
                {
                    wait_box.Items.Remove("Person " + id);
                }));
                cart_box.Dispatcher.Invoke(new Action(() =>
                {
                    cart_box.Items.Add("Person " + id);
                    cart_box.Items.MoveCurrentToLast();
                }
                ));

                roller_cart.Einsteigen();
                roller_cart.Fahren();
                roller_cart.Aussteigen();

                cart_box.Dispatcher.Invoke(new Action(() =>
                {
                    cart_box.Items.Remove("Person " + id);
                }));
            }
        }

    }
}
