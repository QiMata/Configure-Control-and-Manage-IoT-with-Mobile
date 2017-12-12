using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.AspNet.SignalR.Client;
using QiMata.ConfigureControlManage.Models;
using Xamarin.Forms;

namespace QiMata.ConfigureControlManage.ViewModels.Azure
{
    class AzureViewModel : ViewModelBase
    {
        private HubConnection _hubConnection;

        public AzureViewModel()
        {
            OnSendCommand = new Command(async x => await SendRelayConfiguration((RelayConfiguration)x));
        }

        internal Func<string,Task> ShowErrorMessage { private get; set; }

        private string _value;
        public string Value
        {
            get => _value;
            set
            {
                if (_value == value)
                {
                    return;
                }
                _value = value;
                OnPropertyChanged();
            }
        }

        public ICommand OnSendCommand { get; }

        public void OnAppearing()
        {
            _hubConnection = new HubConnection("http://cmcmobileforward.azurewebsites.net/");
            IHubProxy stockTickerHubProxy = _hubConnection.CreateHubProxy("TopicHub");
            stockTickerHubProxy.On<string>("NewData", newData => Value = newData);
            _hubConnection.Start();
        }

        public void OnDisappearing()
        {
            _hubConnection?.Dispose();
        }

        private async Task SendRelayConfiguration(RelayConfiguration relayConfiguration)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://cmcmobileforward.azurewebsites.net/");

                var result = await client.PostAsync("api/UpdateRelayApi/",
                    new ByteArrayContent(new[] { (byte)relayConfiguration }));

                if (!result.IsSuccessStatusCode)
                {
                    await ShowErrorMessage("Unable to update relay");
                }
            }
        }
    }
}
