using Microsoft.Graph;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;
using System;
using System.Threading;
using System.Threading.Tasks;
using TeamsPresencePublisher.Options;

namespace TeamsPresencePublisher.Publishers
{
    class MQTTPublisher : IPublisher
    {
        private readonly MQTTOptions _options;
        private IMqttClient _mqttClient;
        private IMqttClientOptions _mqttClientOptions;

        public MQTTPublisher(IMqttFactory factory, MQTTOptions options)
        {
            _mqttClient = factory.CreateMqttClient();
            _options = options;

            var builder = new MqttClientOptionsBuilder().WithTcpServer(_options.Host, _options.Port);

            if (!string.IsNullOrEmpty(_options.Username) ||
                !string.IsNullOrEmpty(_options.Password.ToString()))
            {
                builder.WithCredentials(_options.Username, _options.Password.ToString());
            }

            _mqttClientOptions = builder.Build();
        }

        public bool Enabled => _options.Enabled;

        public async Task PublishAsync(Presence presence)
        {
            try
            {
                if (!_mqttClient.IsConnected)
                {
                    await _mqttClient.ConnectAsync(_mqttClientOptions, CancellationToken.None);
                }

                string topic = _options.Topic;

                await _mqttClient.PublishAsync(topic + "/activity", presence.Activity, true);
                await _mqttClient.PublishAsync(topic + "/availability", presence.Availability, true);
            }
            catch (Exception)
            {
                //TODO: add logging
            }
        }
    }
}
