using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using QiMata.ConfigureControlManage.Services;
using Xamarin.Forms;

namespace QiMata.ConfigureControlManage.ViewModels
{
    class NfcViewModel : ViewModelBase
    {
        private readonly INfcScanner _nfcScanner;

        public NfcViewModel()
        {
            _nfcScanner = DependencyService.Get<INfcScanner>();

            ScanCommand = new Command(async () => await Scan());
        }

        public ICommand ScanCommand { get; }

        internal Func<string,Task> _errorDisplayFunc { private get; set; }

        private async Task Scan()
        {
            try
            {
                var bytes = await _nfcScanner.Scan(TimeSpan.FromMinutes(1));
                var nfcStr = Encoding.UTF8.GetString(bytes,0,bytes.Length);
                var message = nfcStr.Substring(nfcStr.IndexOf("pkg") + 3);
                
                if (message.Contains("/qimata"))
                {
                    using (HttpClient client = new HttpClient())
                    {
                        client.BaseAddress = new Uri("http://cmcmobileforward.azurewebsites.net/");

                        var result = await client.PostAsync("api/UpdateRelayApi/",
                            new ByteArrayContent(new[] { (byte)0 }));

                        if (!result.IsSuccessStatusCode)
                        {
                            await _errorDisplayFunc("Unable to update relay");
                        }
                    }
                }
                else if (message.Contains("com.qimata"))
                {
                    using (HttpClient client = new HttpClient())
                    {
                        client.BaseAddress = new Uri("http://cmcmobileforward.azurewebsites.net/");

                        var result = await client.PostAsync("api/UpdateRelayApi/",
                            new ByteArrayContent(new[] { (byte)1 }));

                        if (!result.IsSuccessStatusCode)
                        {
                            await _errorDisplayFunc("Unable to update relay");
                        }
                    }
                }

                await _errorDisplayFunc(message);
            }
            catch (PlatformNotSupportedException platformNotSupportedException)
            {
                await _errorDisplayFunc(platformNotSupportedException.Message);
            }
            catch (TimeoutException)
            {
                await _errorDisplayFunc("Scan timed out");
            }
        }
    }
}
