using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace TeamsPresencePublisher.Converters
{
    public class PresenceToVisiblityConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            // possible values for availability are: Available, AvailableIdle, Away, BeRightBack, Busy, BusyIdle, DoNotDisturb, Offline, PresenceUnknown
            // possible values for activity are: Available, Away, BeRightBack, Busy, DoNotDisturb, InACall, InAConferenceCall, Inactive, InAMeeting, Offline, OffWork, OutOfOffice, PresenceUnknown, Presenting, UrgentInterruptionsOnly

            string currentAvailability = (string)values[0];
            string availability = (string)values[1];
            string currentActivity = (string)values[2];
            string activity = (string)values[3];
            bool refreshing = (bool)values[4];

            if (refreshing)
            {
                return Visibility.Collapsed;
            }

            string[] availabilities = availability?.Split('|') ?? new string[0];
            string[] activities = activity?.Split('|') ?? new string[0];

            if (availabilities.Contains(currentAvailability) && activities.Contains(currentActivity))
            {
                return Visibility.Visible;
            }
            else if (availabilities.Contains(currentAvailability) && activities.Length == 0)
            {
                return Visibility.Visible;
            }
            else if (availabilities.Length == 0 && activities.Contains(currentActivity))
            {
                return Visibility.Visible;
            }

            return Visibility.Collapsed;

        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
