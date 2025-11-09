using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading;
using System.Windows.Controls;

public enum Status { Hidden, Waiting, Crossing, Passed }
public enum Location { North, East, South, West, InCrossing }

namespace Kreuzung {
    public class Car {
        public Location location { get; set; }
        public Location destination { get; set; }

        public Thread thread;

        public Car(Location location, Location destination) {
            this.location = location;
            this.destination = destination;
            this.thread = new Thread(new ThreadStart(Drive));
        }

        public override String ToString() {
            return $"Car from {location} to {destination}";
        }

        private void Drive() {
            // Simulate driving to the crossing
            Thread.Sleep(new Random().Next(1000, 10000));
            // Simulate crossing the intersection
            Crossing crossing = MainWindow.GetInstance().crossing;
            crossing.cross(this);
            // Simulate driving away from the crossing
            Thread.Sleep(new Random().Next(1000, 5000));
        }

        public void Start() {
            thread.Start();
        }
    }
}
