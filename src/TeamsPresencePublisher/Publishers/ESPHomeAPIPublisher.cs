using Microsoft.Graph;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using TeamsPresencePublisher.Options;

namespace TeamsPresencePublisher.Publishers
{
    public class ESPHomeAPIPublisher : IPublisher
    {
        private readonly HttpClient _httpClient;
        private readonly ESPHomeAPIOptions _options;

        public ESPHomeAPIPublisher(HttpClient httpClient, ESPHomeAPIOptions options)
        {
            _httpClient = httpClient;
            _options = options;
        }

        public bool Enabled => _options.Enabled;

        public async Task PublishAsync(Presence presence)
        {
            try
            {
                Uri uri = CreateUrl(presence);

                await _httpClient.PostAsync(uri, null);
            }
            catch(Exception ex)
            {
                // todo: add logging
            }
        }

        private Uri CreateUrl(Presence presence)
        {
            (int r, int g, int b, int brightness) = GetDesiredLedSettings(presence);

            // https://esphome.io/web-api/index.html#light
            string relativeUri = $"light/{_options.LightId}/turn_on?brightness={brightness}&r={r}&g={g}&b={b}&transition={_options.Transition}";

            return new Uri(_options.BaseUri, relativeUri);
        }

        private (int R, int G, int B, int Brightness) GetDesiredLedSettings(Presence presence)
        {
            //switch (presence.Availability)
            //{
            //    case "Available":
            //        return (0, 63, 21, _options.Brightness);
            //    case "Busy":
            //        return (255, 51, 0, _options.Brightness);
            //    case "BeRightBack":
            //        return (255, 255, 0, _options.Brightness);
            //    case "Away":
            //        return (255, 255, 0, _options.Brightness);
            //    case "DoNotDisturb":
            //        return (128, 0, 0, _options.Brightness);
            //    case "Offline":
            //        return (255, 255, 255, _options.Brightness);
            //    case "Off":
            //        return (255, 255, 255, _options.Brightness);
            //    default:
            //        return (0, 0, 0, 0);
            //}

            switch (presence.Activity)
            {
                case "Available":
                case "Busy":
                case "Inactive":
                case "UrgentInterruptionsOnly":
                    return (0, 63, 21, _options.Brightness);
                case "Away":
                case "BeRightBack":
                    return (255, 255, 0, _options.Brightness);
                case "InACall":
                case "InAConferenceCall":
                case "InAMeeting":
                case "Presenting":
                case "DoNotDisturb":
                    return (128, 0, 0, _options.Brightness);
                case "PresenceUnknown":
                case "Offline":
                case "OffWork":
                case "OutOfOffice":
                default:
                    return (0, 0, 0, 0);
            }
        }
    }
}
