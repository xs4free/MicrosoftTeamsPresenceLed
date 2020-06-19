using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace TeamsPresencePublisher.Controls
{
    public partial class UserPresenceControl : UserControl
    {
        public string Availability
        {
            get { return (string)GetValue(AvailabilityProperty); }
            set { SetValue(AvailabilityProperty, value); }
        }

        public static readonly DependencyProperty AvailabilityProperty =
            DependencyProperty.Register(nameof(Availability), typeof(string), typeof(UserPresenceControl), new PropertyMetadata("PresenceUnknown"));

        public string Activity
        {
            get { return (string)GetValue(ActivityProperty); }
            set { SetValue(ActivityProperty, value); }
        }

        public static readonly DependencyProperty ActivityProperty =
            DependencyProperty.Register(nameof(Activity), typeof(string), typeof(UserPresenceControl), new PropertyMetadata("PresenceUnknown"));

        public bool Refreshing
        {
            get { return (bool)GetValue(RefreshingProperty); }
            set { SetValue(RefreshingProperty, value); }
        }

        public static readonly DependencyProperty RefreshingProperty =
            DependencyProperty.Register(nameof(Refreshing), typeof(bool), typeof(UserPresenceControl), new PropertyMetadata(false));

        public DateTime LastUpdate
        {
            get { return (DateTime)GetValue(LastUpdateProperty); }
            set { SetValue(LastUpdateProperty, value); }
        }

        public static readonly DependencyProperty LastUpdateProperty =
            DependencyProperty.Register(nameof(LastUpdate), typeof(DateTime), typeof(UserPresenceControl), new PropertyMetadata(DateTime.Now));

        public BitmapImage ProfileImage
        {
            get { return (BitmapImage)GetValue(ProfileImageProperty); }
            set { SetValue(ProfileImageProperty, value); }
        }

        public static readonly DependencyProperty ProfileImageProperty =
            DependencyProperty.Register(nameof(ProfileImage), typeof(BitmapImage), typeof(UserPresenceControl), new PropertyMetadata(null));

        public Visibility RefreshingToVisibility(bool refreshing)
        {
            return refreshing ? Visibility.Visible : Visibility.Collapsed;
        }

        public string GetToolTip(DateTime lastUpdate, string availability, string activity)
        {
            return $"Last update: {lastUpdate}{Environment.NewLine}Availability: {availability}{Environment.NewLine}Activity: {activity}";
        }
    }
}
