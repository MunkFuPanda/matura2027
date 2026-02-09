using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Media_Player
{
    public class PlayerViewModel : INotifyPropertyChanged
    {
        private int _volume = 50;

        public int Volume
        {
            get => _volume;
            set
            {
                if (_volume != value)
                {
                    _volume = value;
                    OnPropertyChanged(nameof(Volume));
                    OnPropertyChanged(nameof(MediaVolume));
                }
            }
        }

        public double MediaVolume => Volume / 100.0;

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string name)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        private double _progress;
        private double _duration;

        public double Progress
        {
            get => _progress;
            set
            {
                if (_progress != value)
                {
                    _progress = value;
                    OnPropertyChanged(nameof(Progress));
                }
            }
        }

        public double Duration
        {
            get => _duration;
            set
            {
                if (_duration != value)
                {
                    _duration = value;
                    OnPropertyChanged(nameof(Duration));
                }
            }
        }



    }

}
