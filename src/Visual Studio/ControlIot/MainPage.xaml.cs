using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using ControlIot.AzureIoT;
using ControlIot.Bluetooth;
using ControlIot.Relay;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace ControlIot
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private ControlBluettoothService _controlBluettoothService;
        private ControlClient _controlClient;

        public MainPage()
        {
            this.InitializeComponent();

            var stringList = new[]
            {
                "https://qimata.com", "https://jaredrhodes.com", "https://twitter.com/QiMata",
                "https://www.linkedin.com/in/qimata/","https://github.com/QiMata"
            };

            var relayService = new RelayService(2,3);

            _controlClient =
                new ControlClient(
                    "{Enter your connection string}",
                    relayService, stringList);
            _controlBluettoothService = new ControlBluettoothService(relayService,stringList);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            Dispatcher.RunAsync(CoreDispatcherPriority.High,async () =>
            {
               await _controlBluettoothService.Start();
            });
        }
    }
}
