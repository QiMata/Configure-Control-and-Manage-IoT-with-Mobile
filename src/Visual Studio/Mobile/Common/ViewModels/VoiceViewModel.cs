using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
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
            ListenCommand = new Command(HandleVoiceInput);
        }


        public ICommand ListenCommand { get; }

        private void HandleVoiceInput()
        {
            if (!CrossSpeechRecognition.Current.IsSupported)
            {
                return;
            }

            var granted = CrossSpeechRecognition.Current.RequestPermission()
                .SubscribeOn(SynchronizationContext.Current)
                .Subscribe(x =>
            {
                if (x != SpeechRecognizerStatus.Available)
                {
                    return;
                }

                _listenerDisposable = CrossSpeechRecognition
                    .Current
                    .ContinuousDictation()
                    .Buffer(TimeSpan.FromSeconds(5))
                    .Subscribe(async phrase =>
                    {
                        try
                        {
                            if (phrase.Contains("Off") || phrase.Contains("off"))
                            {
                                await SendRelayConfiguration(RelayConfiguration.Off);
                            }
                            else if ((phrase.Contains("Second") || phrase.Contains("second")) &&
                                     (phrase.Contains("On") || phrase.Contains("on")))
                            {
                                await SendRelayConfiguration(RelayConfiguration.SecondOn);
                            }
                            else if ((phrase.Contains("First") || phrase.Contains("first")) &&
                                     (phrase.Contains("On") || phrase.Contains("on")))
                            {
                                await SendRelayConfiguration(RelayConfiguration.FirstOn);
                            }
                        }
                        catch (Exception)
                        {
                            //Eat exception its a demo
                        }
                        // will keep returning phrases as pause is observed
                    });
            });
           
        }

        private async Task VoiceInputMethod()
        {
            
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
