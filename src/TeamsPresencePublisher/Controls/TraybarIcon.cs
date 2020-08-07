using System;
using System.Windows.Forms;
using System.IO;
using TeamsPresencePublisher.Models;
using System.Drawing;
using TeamsPresencePublisher.Converters;
using System.Windows.Media.Imaging;
using System.Collections.Generic;

namespace TeamsPresencePublisher.Controls
{
    public class TraybarIcon
    {
        private readonly NotifyIcon _notifyIcon = new NotifyIcon();
        private readonly ContextMenuStrip _contextMenuStrip = new ContextMenuStrip();
        private readonly MainWindow _mainWindow;
        private readonly PresenceViewModel _presenceViewModel;

        private Dictionary<IconEnum, Tuple<Icon, BitmapFrame>> _icons = new Dictionary<IconEnum, Tuple<Icon, BitmapFrame>>();

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
            LoadIcon(IconEnum.White, "TraybarIcon.ico");
            LoadIcon(IconEnum.Green, "TraybarIcon-green.ico");
            LoadIcon(IconEnum.Yellow, "TraybarIcon-yellow.ico");
            LoadIcon(IconEnum.Red, "TraybarIcon-red.ico");

            ToolStripItem itemShow = _contextMenuStrip.Items.Add("Show");
            itemShow.Click += ItemShow_Click;
            ToolStripItem itemQuit = _contextMenuStrip.Items.Add("Quit");
            itemQuit.Click += ItemQuit_Click;

            _notifyIcon.ContextMenuStrip = _contextMenuStrip;

            _notifyIcon.Text = "Teams Presence Publisher";

            _notifyIcon.DoubleClick += ItemShow_Click;

            SetColor(ActivityColor.Default);
        }

        public void SetColor(ActivityColor color)
        {
            IconEnum icon;

            switch (color)
            {
                case ActivityColor.Green:
                    icon = IconEnum.Green;
                    break;
                case ActivityColor.Yellow:
                    icon = IconEnum.Yellow;
                    break;
                case ActivityColor.Red:
                    icon = IconEnum.Red;
                    break;
                case ActivityColor.Default:
                default:
                    icon = IconEnum.White;
                    break;
            }

            _notifyIcon.Icon = GetIcon(icon);
            _mainWindow.Icon = GetBitmap(icon);
        }

        private Icon GetIcon(IconEnum icon) => _icons[icon].Item1;
        private BitmapFrame GetBitmap(IconEnum icon) => _icons[icon].Item2;

        private void LoadIcon(IconEnum ico, string assetsName)
        {
            Uri iconUri = new Uri($"pack://application:,,,/TeamsPresencePublisher;component/Assets/{assetsName}");

            using Stream iconStream = System.Windows.Application.GetResourceStream(iconUri).Stream;
            Icon icon = new Icon(iconStream);

            BitmapFrame bitmap = BitmapFrame.Create(iconUri);

            _icons.Add(ico, new Tuple<Icon, BitmapFrame>(icon, bitmap));
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
