using System;
using System.ComponentModel;

namespace TeamsPresencePublisher.Options
{
    public class ESPHomeAPIOptions : INotifyPropertyChanged
    {
        private bool _enabled = false;
        private Uri _baseUri = new Uri("http://192.168.1.153");
        private string _lightId = "xringname";
        private int _brightness = 64;
        private int _transition = 1;

        public bool Enabled
        {
            get => _enabled;
            set
            {
                _enabled = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Enabled)));
            }
        }

        public Uri BaseUri
        {
            get => _baseUri;
            set
            {
                _baseUri = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(BaseUri)));
            }
        }

        public string LightId
        {
            get => _lightId;
            set
            {
                _lightId = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LightId)));
            }
        }

        public int Brightness
        {
            get => _brightness;
            set
            {
                _brightness = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Brightness)));
            }
        }

        public int Transition
        {
            get => _transition;
            set
            {
                _transition = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Transition)));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
