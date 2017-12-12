using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Plugin.SpeechRecognition;
using QiMata.ConfigureControlManage.Models;
using Xamarin.Forms;

namespace QiMata.ConfigureControlManage.ViewModels
{
    class VoiceViewModel : ViewModelBase
    {
        private IDisposable _listenerDisposable;

        public VoiceViewModel()
        {
            ListenCommand = new Command(async () => await HandleVoiceInput());
        }


        public ICommand ListenCommand { get; }

        private async Task HandleVoiceInput()
        {
            var granted = await CrossSpeechRecognition.Current.RequestPermission();
            if (granted != SpeechRecognizerStatus.Available)
            {
                return;
            }

            _listenerDisposable = CrossSpeechRecognition
                .Current
                .ListenForFirstKeyword(new []{"Off","On"})
                .Subscribe(async phrase => {
                    // will keep returning phrases as pause is observed
                    if (phrase.Contains("Off"))
                    {
                        await SendRelayConfiguration(RelayConfiguration.Off);
                    }
                    else if (phrase.Contains("Second") && phrase.Contains("On"))
                    {
                        await SendRelayConfiguration(RelayConfiguration.SecondOn);
                    }
                    else if (phrase.Contains("First") && phrase.Contains("On"))
                    {
                        await SendRelayConfiguration(RelayConfiguration.FirstOn);
                    }
                });
        }

        public void OnDisappearing()
        {
            _listenerDisposable?.Dispose();
        }

        private async Task SendRelayConfiguration(RelayConfiguration relayConfiguration)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://cmcmobileforward.azurewebsites.net/");

                var result = await client.PostAsync("api/UpdateRelayApi/",
                    new ByteArrayContent(new[] { (byte)relayConfiguration }));
            }
        }
    }
}
