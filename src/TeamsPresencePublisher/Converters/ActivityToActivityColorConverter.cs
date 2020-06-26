using TeamsPresencePublisher.Controls;

namespace TeamsPresencePublisher.Converters
{
    static class ActivityToActivityColorConverter
    {
        public static ActivityColor Convert(string activity)
        {
            switch (activity)
            {
                case "Available":
                case "Busy":
                case "Inactive":
                case "UrgentInterruptionsOnly":
                    return ActivityColor.Green;
                case "Away":
                case "BeRightBack":
                    return ActivityColor.Yellow;
                case "InACall":
                case "InAConferenceCall":
                case "InAMeeting":
                case "Presenting":
                case "DoNotDisturb":
                    return ActivityColor.Red;
                case "PresenceUnknown":
                case "Offline":
                case "OffWork":
                case "OutOfOffice":
                default:
                    return ActivityColor.Default;
            }
        }
    }
}
