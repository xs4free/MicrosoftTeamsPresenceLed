using System;
using System.ComponentModel;
using System.Windows.Media.Imaging;
using TeamsPresencePublisher.Options;

namespace TeamsPresencePublisher.Models
{
    public class PresenceViewModel : INotifyPropertyChanged
    {
        private string _availability = "PresenceUnknown";
        private string _activity = "PresenceUnknown";
        private DateTime _lastUpdate;
        private BitmapImage _profileImage;
        private bool _isSignedIn;
        private bool _refreshingPresence;
        private bool _publishingPresence;
        private string _userName;

        public event PropertyChangedEventHandler PropertyChanged;

        public string Availability
        {
            get => _availability;
            set
            {
                _availability = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Availability)));
            }
        }

        public string Activity
        {
            get => _activity;
            set
            {
                _activity = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Activity)));
            }
        }

        public DateTime LastUpdate
        {
            get => _lastUpdate;
            set
            {
                _lastUpdate = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LastUpdate)));
            }
        }

        public string UserName
        {
            get => _userName;
            set
            {
                _userName = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(UserName)));
            }
        }

        public BitmapImage ProfileImage
        {
            get => _profileImage;
            set
            {
                _profileImage = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ProfileImage)));
            }
        }

        public bool IsSignedIn
        {
            get => _isSignedIn;
            set
            {
                _isSignedIn = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsSignedIn)));
            }
        }

        public bool RefreshingPresence
        {
            get => _refreshingPresence;
            set
            {
                _refreshingPresence = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(RefreshingPresence)));
            }
        }

        public bool PublishingPresence
        {
            get => _publishingPresence;
            set
            {
                _publishingPresence = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PublishingPresence)));
            }
        }

        public ESPHomeAPIOptions ESPHomeAPIOptions { get; set; }
        public MQTTOptions MQTTOptions { get; set; }

        public PresenceViewModel(ESPHomeAPIOptions espHomeAPIOptions, MQTTOptions mqttOptions)
        {
            ESPHomeAPIOptions = espHomeAPIOptions;
            MQTTOptions = mqttOptions;
        }
    }
}
