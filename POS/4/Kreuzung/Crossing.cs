using System;
using System.Collections.Generic;
using System.Threading;

namespace Kreuzung {
    public class Crossing {
        private readonly object lockObject = new object();
        public void cross(Car car) {
            lock (lockObject) {
                // Car enters the crossing
                car.location = Location.InCrossing;

                if (car.destination == Location.North || car.destination == Location.South) {
                    // Simulate north-south crossing
                    MainWindow.GetInstance().UpdateY();
                } else if (car.destination == Location.East || car.destination == Location.West) {
                    // Simulate east-west crossing
                    MainWindow.GetInstance().UpdateX();
                }
                // Simulate time taken to cross
                Thread.Sleep(new Random().Next(1000, 5000));
                // Car leaves the crossing
                car.location = car.destination;

                if (car.destination == Location.North || car.destination == Location.South) {
                    // Simulate north-south crossing
                    MainWindow.GetInstance().UpdateY();
                } else if (car.destination == Location.East || car.destination == Location.West) {
                    // Simulate east-west crossing
                    MainWindow.GetInstance().UpdateX();
                }
            }
        }
    }
}
