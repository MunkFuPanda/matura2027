using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace Kreuzung
{

    public enum Direction
    {
        North,
        South,
        West,
        East
    };

    internal class Car
    {

        private int id;
        private Direction direction;

        private ListBox from;
        private ListBox to;

        private TextBox status = new TextBox();

        private Crossroad road;


        public Car(int id, Direction direction, ListBox from, ListBox to, Crossroad road)
        {
            this.id = id;
            this.direction = direction;
            this.from = from;
            this.to = to;
            this.road = road;
        }

        public void drive()
        {
            

            Thread.Sleep(new Random().Next(1000, 10000));

            status.Dispatcher.Invoke(new Action(() =>
            {
                status.Text = id + " " + direction.ToString();
                status.Background = Brushes.Red;
            }));

            road.cross(status, from, to, direction);



            
            
        }
    }
}
