using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Microsoft.Graph;
using TeamsPresencePublisher.Models;
using TeamsPresencePublisher.Publishers;
using TeamsPresencePublisher.Services;

namespace TeamsPresencePublisher
{
    public partial class MainWindow : Window
    {
        private readonly IMicrosoftAuthentication _microsoftAuthentication;
        private readonly IPresenceService _presenceService;
        private readonly IEnumerable<IPublisher> _publishers;
        private readonly PresenceViewModel _presenceViewModel;
        private readonly IOptionsService _optionsService;
        private readonly DispatcherTimer _timer = new DispatcherTimer();

        public MainWindow(
            IMicrosoftAuthentication microsoftAuthentication,
            IPresenceService presenceService,
            IOptionsService optionsService,
            IEnumerable<IPublisher> publishers,
            PresenceViewModel presenceViewModel)
        {
            InitializeComponent();

            _microsoftAuthentication = microsoftAuthentication;
            _presenceService = presenceService;
            _optionsService = optionsService;
            _publishers = publishers;

            _presenceViewModel = presenceViewModel;
            _presenceViewModel.MQTTOptions.PropertyChanged += Options_Changed;
            _presenceViewModel.ESPHomeAPIOptions.PropertyChanged += Options_Changed;

            _timer.Interval = TimeSpan.FromSeconds(15);
            _timer.Tick += _timer_Tick;

            DataContext = _presenceViewModel;

            Loaded += MainPage_Loaded;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            Hide();

            e.Cancel = true;
            base.OnClosing(e);
        }

        private void Options_Changed(object sender, PropertyChangedEventArgs e)
        {
            _optionsService.SaveSettingsAsync(sender);
        }

        private async void _timer_Tick(object sender, object e)
        {
            try
            {
                _presenceViewModel.RefreshingPresence = true;
                Presence update = await _presenceService.GetPresenceAsync();
                _presenceViewModel.RefreshingPresence = false;

                _presenceViewModel.Activity = update.Activity;
                _presenceViewModel.Availability = update.Availability;
                _presenceViewModel.LastUpdate = DateTime.Now;

                _presenceViewModel.PublishingPresence = true;
                foreach (IPublisher publisher in _publishers.Where(publisher => publisher.Enabled))
                {
                    await publisher.PublishAsync(update);
                }
            }
            catch (Exception)
            {

            }

            _presenceViewModel.RefreshingPresence = false;
            _presenceViewModel.PublishingPresence = false;
        }

        private async void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            await UpdateIsSignedIn();
        }

        private async Task UpdateIsSignedIn()
        {
            _presenceViewModel.IsSignedIn = await _microsoftAuthentication.IsSignedInAsync();

            if (_presenceViewModel.IsSignedIn)
            {
                _timer.Start();

                _presenceViewModel.UserName = await _presenceService.GetUsernameAsync();
                _presenceViewModel.ProfileImage = await _presenceService.GetPhotoAsync();
            }
            else
            {
                _timer.Stop();

                _presenceViewModel.UserName = null;
                _presenceViewModel.ProfileImage = _presenceService.GetDefaultPhoto();
            }
        }

        private async void ButtonSignin_Click(object sender, RoutedEventArgs e)
        {
            await _microsoftAuthentication.SigninAsync();
            await UpdateIsSignedIn();
        }

        private async void ButtonSignout_Click(object sender, RoutedEventArgs e)
        {
            await _microsoftAuthentication.Signout();
            await UpdateIsSignedIn();
        }
    }
}
