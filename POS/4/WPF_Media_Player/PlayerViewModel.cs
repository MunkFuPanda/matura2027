using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;

namespace WPF_Media_Player {
    public class PlayerViewModel : INotifyPropertyChanged {
        private double progress;
        private double duration;
        private double volume = 0.5;
        private int currentIndex;

        public event PropertyChangedEventHandler? PropertyChanged;

        public ObservableCollection<string> Playlist { get; } = new ObservableCollection<string>();

        public int CurrentIndex {
            get => currentIndex;
            set {
                if (currentIndex != value) {
                    currentIndex = value;
                    OnPropertyChanged(nameof(CurrentIndex));
                    OnPropertyChanged(nameof(CurrentItem));
                    OnPropertyChanged(nameof(CurrentFilename));
                }
            }
        }

        public string? CurrentItem => (Playlist.Count > 0 && CurrentIndex >= 0 && CurrentIndex < Playlist.Count) ? Playlist[CurrentIndex] : null;

        public string? CurrentFilename => CurrentItem != null ? Path.GetFileName(CurrentItem) : null;

        public double Progress {
            get => progress;
            set { progress = value; OnPropertyChanged(nameof(Progress)); }
        }

        public double Duration {
            get => duration;
            set { duration = value; OnPropertyChanged(nameof(Duration)); }
        }

        public double Volume {
            get => volume;
            set { volume = value; OnPropertyChanged(nameof(Volume)); }
        }

        private void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
