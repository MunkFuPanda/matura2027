using System.ComponentModel;

namespace WPF_Media_Player {
    public class PlayerViewModel : INotifyPropertyChanged {
        private double progress;
        private double duration;
        private double volume = 0.5;

        public event PropertyChangedEventHandler? PropertyChanged;

        public double Progress {
            get => progress;
            set {
                if (progress != value) {
                    progress = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Progress)));
                }
            }
        }

        public double Duration {
            get => duration;
            set {
                if (duration != value) {
                    duration = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Duration)));
                }
            }
        }

        public double Volume {
            get => volume;
            set {
                if (volume != value) {
                    volume = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Volume)));
                }
            }
        }
    }
}
