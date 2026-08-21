using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace Achterbahn

// KOMMT in den park
// wandert ein bisschen
// eine qeue machen wagen.queue
// wenn der wagen nicht mehr rennt return, heimgehen
// aus der queue draußen
// wagen einsteigen
// wagen warten
// wagen fahren
// wagen aussteigen

// in der gui große listbox mit status pro passagier mit id + status & cart status
// fahrten anzahl in der gui einstellen mit einem slider probieren


{
    internal class cart
    {

        public Boolean Working {get => isWorking;set { isWorking = value; }}

        private int cart_count;
        private int cart_count_now = 0;
        private int drive_count;
        private Label status;
        private Boolean isWorking = true;
        private Boolean isDriving = false;

        // Signaling

        ManualResetEvent _queueingHandler = new ManualResetEvent(false);
        AutoResetEvent _barrierHandler = new AutoResetEvent(false);
        CountdownEvent _countdown;



        public cart(int cart_count, int drive_count, Label status)
        {
            this.cart_count = cart_count;
            this.status = status;
            this.drive_count = drive_count;
            _countdown = new CountdownEvent(cart_count);
        }



        // doch kein thread mit for schleife machen

        public void CartThread()
        {
            while (true)
            {
                _countdown.Reset();
                _barrierHandler.Reset();
                _queueingHandler.Reset();
                if (drive_count == 0)
                {
                    return;
                }

               
            }
        }

        public void Queue()
        {
            if (drive_count == 0)
            {
                isWorking = false;
                return;
            }
            _barrierHandler.WaitOne();
        }

        public void Einsteigen()
        {
            _countdown.Signal();
            cart_count_now++;
            _queueingHandler.WaitOne();
        }

        public void Fahren()
        {
            Thread.Sleep(3000);
        }

        public void cartFahren()
        {
            status.Dispatcher.Invoke(new Action(() =>
            {
                status.Content = "Fährt";
                status.Background = Brushes.Green;
            }));

            isDriving = true;
            Thread.Sleep(3000);
            isDriving = false;
            drive_count--;

            status.Dispatcher.Invoke(new Action(() =>
            {
                status.Content = "Steht";
                status.Background = Brushes.White;
            }));
        }

        public void Aussteigen()
        {
            _countdown.Signal();
            cart_count_now--;
            _queueingHandler.WaitOne();
        }
    }
}
