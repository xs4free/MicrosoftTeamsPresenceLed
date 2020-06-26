using System;
using System.Windows.Forms;
using System.IO;
using TeamsPresencePublisher.Models;
using System.Drawing;
using TeamsPresencePublisher.Converters;

namespace TeamsPresencePublisher.Controls
{
    public class TraybarIcon
    {
        private readonly NotifyIcon _notifyIcon = new NotifyIcon();
        private readonly ContextMenuStrip _contextMenuStrip = new ContextMenuStrip();
        private readonly MainWindow _mainWindow;
        private readonly PresenceViewModel _presenceViewModel;

        private Icon _whiteIcon;
        private Icon _greenIcon;
        private Icon _yellowIcon;
        private Icon _redIcon;

        public TraybarIcon(MainWindow mainWindow, PresenceViewModel presenceViewModel)
        {
            _mainWindow = mainWindow;
            _presenceViewModel = presenceViewModel;
            _presenceViewModel.PropertyChanged += _presenceViewModel_PropertyChanged;

            InitializeNotifyIcon();
        }

        private void _presenceViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(PresenceViewModel.Availability))
            {
                ActivityColor color = ActivityToActivityColorConverter.Convert(_presenceViewModel.Activity);
                SetColor(color);
            }
        }

        private void InitializeNotifyIcon()
        {
            _whiteIcon = LoadIcon("TraybarIcon.ico");
            _greenIcon = LoadIcon("TraybarIcon-green.ico");
            _yellowIcon = LoadIcon("TraybarIcon-yellow.ico");
            _redIcon = LoadIcon("TraybarIcon-red.ico");

            _notifyIcon.Icon = _whiteIcon;

            ToolStripItem itemShow = _contextMenuStrip.Items.Add("Show");
            itemShow.Click += ItemShow_Click;
            ToolStripItem itemQuit = _contextMenuStrip.Items.Add("Quit");
            itemQuit.Click += ItemQuit_Click;

            _notifyIcon.ContextMenuStrip = _contextMenuStrip;

            _notifyIcon.Text = "Teams Presence Publisher";

            _notifyIcon.DoubleClick += ItemShow_Click;
        }

        public void SetColor(ActivityColor color)
        {
            switch (color)
            {
                case ActivityColor.Green:
                    _notifyIcon.Icon = _greenIcon;
                    break;
                case ActivityColor.Yellow:
                    _notifyIcon.Icon = _yellowIcon;
                    break;
                case ActivityColor.Red:
                    _notifyIcon.Icon =
                        _redIcon; break;
                case ActivityColor.Default:
                default:
                    _notifyIcon.Icon = _whiteIcon;
                    break;
            }
        }

        private Icon LoadIcon(string assetsName)
        {
            Uri uri = new Uri($"pack://application:,,,/TeamsPresencePublisher;component/Assets/{assetsName}");
            using Stream iconStream = System.Windows.Application.GetResourceStream(uri).Stream;
            return new Icon(iconStream);
        }

        public void Show()
        {
            _notifyIcon.Visible = true;
        }

        private void ItemShow_Click(object sender, EventArgs e)
        {
            _mainWindow.Show();
            _mainWindow.Activate();
        }

        private void ItemQuit_Click(object sender, EventArgs e)
        {
            _notifyIcon.Visible = false;
            Environment.Exit(0);
        }
    }
}
