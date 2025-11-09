using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace Kreuzung {
    public partial class MainWindow : Window, INotifyPropertyChanged {
        public MainWindow() {
            InitializeComponent();
            DataContext = this;
            instance = this;
        }

        private static MainWindow? instance;

        public event PropertyChangedEventHandler? PropertyChanged;

        public readonly Crossing crossing = new Crossing();

        public static MainWindow GetInstance() => instance ??= new MainWindow();

        public ObservableCollection<Car> Cars { get; } = [];
        public ObservableCollection<Car> CarsNorth { get => getCarsForLocation(Location.North); }
        public ObservableCollection<Car> CarsEast { get => getCarsForLocation(Location.East); }
        public ObservableCollection<Car> CarsSouth { get => getCarsForLocation(Location.South); }
        public ObservableCollection<Car> CarsWest { get => getCarsForLocation(Location.West); }
        public ObservableCollection<Car> CarsInCrossing { get => getCarsForLocation(Location.InCrossing); }

        private ObservableCollection<Car> getCarsForLocation(Location location) {
            var carsForLocation = new ObservableCollection<Car>();
            foreach (var car in Cars) {
                if (car.location == location) {
                    carsForLocation.Add(car);
                }
            }
            return carsForLocation;
        }

        private void StartSimulation_Click(object sender, RoutedEventArgs e) {
            foreach (var car in Cars) {
                car.thread.Interrupt();
            }

            Cars.Clear();

            int carCount;
            if (!int.TryParse(CarsAmountTextBox.Text, out carCount)) {
                MessageBox.Show("Please enter a valid number for car count.");
            }

            Random rand = new Random();
            for (int i = 0; i < carCount; i++) {
                Location location = (Location)rand.Next(0, 4); // North, East, South, West
                Location destination;
                do {
                    destination = (Location)(((int)location + 2) % 4); // Ensure destination is different from location
                } while (destination == location); // Ensure destination is different from location
                Car car = new Car(location, destination);
                Cars.Add(car);
            }

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CarsNorth)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CarsEast)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CarsSouth)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CarsWest)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CarsInCrossing)));

            foreach (var car in Cars) {
                car.Start();
            }
        }

        public void UpdateX() {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CarsEast)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CarsWest)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CarsInCrossing)));
        }

        public void UpdateY() {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CarsNorth)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CarsSouth)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CarsInCrossing)));
        }
    }
}
