using System.ComponentModel;

namespace TeamsPresencePublisher.Options
{
    public class MQTTOptions : INotifyPropertyChanged
    {
        private bool _enabled;
        private string _host = "localhost";
        private int? _port = 1883;
        private string _username = "Hans";
        private string _password = "Test";
        private string _topic = "microsoft/graph/me/presence";

        public bool Enabled
        {
            get => _enabled;
            set
            {
                _enabled = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Enabled)));
            }
        }

        public string Host
        {
            get => _host;
            set
            {
                _host = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Host)));
            }
        }

        public int? Port
        {
            get => _port;
            set
            {
                _port = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Port)));
            }
        }

        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Username)));
            }

        }

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Password)));
            }
        }

        public string Topic
        {
            get => _topic;
            set
            {
                _topic = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Topic)));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
