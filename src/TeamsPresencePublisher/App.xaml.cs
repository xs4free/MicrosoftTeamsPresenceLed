using Microsoft.Extensions.DependencyInjection;
using Microsoft.Graph;
using MQTTnet;
using System;
using System.Collections.Generic;
using System.Windows;
using TeamsPresencePublisher.Controls;
using TeamsPresencePublisher.Models;
using TeamsPresencePublisher.Options;
using TeamsPresencePublisher.Publishers;
using TeamsPresencePublisher.Services;

namespace TeamsPresencePublisher
{
    public partial class App : System.Windows.Application
    {
        private IServiceProvider _serviceProvider;

        public App()
        {
            ServiceCollection serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            _serviceProvider = serviceCollection.BuildServiceProvider();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            TraybarIcon traybarIcon = _serviceProvider.GetRequiredService<TraybarIcon>();
            traybarIcon.Show();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpClient();

            services.AddSingleton<TraybarIcon>();
            services.AddSingleton<MainWindow>();
            services.AddSingleton<IMqttFactory, MqttFactory>();
            services.AddSingleton<PresenceViewModel>();
            services.AddSingleton<IMicrosoftAuthentication, MicrosoftAuthentication>();
            services.AddSingleton<IAuthenticationProvider>(
                serviceProvider => serviceProvider.GetRequiredService<IMicrosoftAuthentication>().AuthProvider);
            services.AddSingleton<IPresenceService, PresenceService>();
            services.AddSingleton<ESPHomeAPIPublisher>();
            services.AddSingleton<MQTTPublisher>();
            services.AddSingleton<TeamsPresencePublisherOptions>();
            services.AddScoped<IOptionsService, OptionsService>();
            services.AddSingleton<MQTTOptions>(serviceProvider => serviceProvider.GetRequiredService<IOptionsService>().ReadSettingsAsync<MQTTOptions>().Result);
            services.AddSingleton<ESPHomeAPIOptions>(serviceProvider => serviceProvider.GetRequiredService<IOptionsService>().ReadSettingsAsync<ESPHomeAPIOptions>().Result);

            services.AddSingleton<IEnumerable<IPublisher>>(
                serviceProvider => new List<IPublisher>
                {
                    serviceProvider.GetRequiredService<MQTTPublisher>(),
                    serviceProvider.GetRequiredService<ESPHomeAPIPublisher>()
                });
        }
    }
}
