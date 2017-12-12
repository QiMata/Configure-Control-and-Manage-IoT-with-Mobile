using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth;
using Windows.Devices.Bluetooth.GenericAttributeProfile;
using Windows.Storage.Streams;
using ControlIot.Bluetooth.Helpers;
using ControlIot.Relay;

namespace ControlIot.Bluetooth
{
    class ControlBluettoothService
    {
        private readonly RelayService _relayService;
        private readonly RotatingString _rotatingString;

        private static readonly Guid ServiceGuid = Guid.Parse("FE08FF91-8B9D-4D4D-9135-4A8F376D2F09");
        private static readonly Guid RelayCharactersticGuid = Guid.Parse("97B06D92-6D10-4A8A-88B4-822C6DFF9F4C");
        private static readonly Guid RotatingStringCharactersticGuid = Guid.Parse("BF5CAE55-1F37-4231-A389-DA4771C31E33");

        private readonly IList<GattSubscribedClient> _clients;
        private Task _subscriberTask;

        private GattLocalCharacteristic _relayCharacteristic;
        private GattLocalCharacteristic _rotatingStringCharacteristic;

        private GattServiceProvider _gattServiceProvider;

        public ControlBluettoothService(RelayService relayService,IEnumerable<string> rotatingStrings)
        {
            _relayService = relayService;
            _rotatingString = new RotatingString(rotatingStrings);
            _clients = new List<GattSubscribedClient>();
        }

        public async Task Start()
        {
            GattServiceProviderResult result = await GattServiceProvider.CreateAsync(ServiceGuid);

            if (result.Error == BluetoothError.Success)
            {
                _gattServiceProvider = result.ServiceProvider;

                GattLocalCharacteristicResult rotatingtStringCharacteristicResult =
                    await _gattServiceProvider.Service.CreateCharacteristicAsync(RotatingStringCharactersticGuid,
                        new GattLocalCharacteristicParameters
                        {
                            CharacteristicProperties = GattCharacteristicProperties.Notify,
                            ReadProtectionLevel = GattProtectionLevel.Plain
                        });
                if (rotatingtStringCharacteristicResult.Error != BluetoothError.Success)
                {
                    // An error occurred.
                    return;
                }
                _rotatingStringCharacteristic = rotatingtStringCharacteristicResult.Characteristic;
                _rotatingStringCharacteristic.SubscribedClientsChanged += RotatingStringCharacteristicOnSubscribedClientsChanged;

                var relayCharacteristicResult = await _gattServiceProvider.Service.CreateCharacteristicAsync(RelayCharactersticGuid, new GattLocalCharacteristicParameters
                {
                    CharacteristicProperties = GattCharacteristicProperties.Write,
                    WriteProtectionLevel = GattProtectionLevel.Plain
                });
                if (relayCharacteristicResult.Error != BluetoothError.Success)
                {
                    // An error occurred.
                    return;
                }
                _relayCharacteristic = relayCharacteristicResult.Characteristic;
                _relayCharacteristic.WriteRequested += RelayCharacteristicOnWriteRequested;

                GattServiceProviderAdvertisingParameters advParameters = new GattServiceProviderAdvertisingParameters
                {
                    IsDiscoverable = true,
                    IsConnectable = true
                };
                _gattServiceProvider.StartAdvertising(advParameters);

                _subscriberTask = BroadcastSubscribers();
            }
        }

        private async Task BroadcastSubscribers()
        {
            while (true)
            {
                var writer = new DataWriter();

            writer.WriteString(_rotatingString.GetNext());

            await _rotatingStringCharacteristic.NotifyValueAsync(writer.DetachBuffer());

            await Task.Delay(TimeSpan.FromSeconds(1));
            }
        }

        private void RotatingStringCharacteristicOnSubscribedClientsChanged(GattLocalCharacteristic sender, object args)
        {
            _clients.Clear();


            foreach (var gattSubscribedClient in sender.SubscribedClients)
            {
                _clients.Add(gattSubscribedClient);
            }
        }

        private async void RelayCharacteristicOnWriteRequested(GattLocalCharacteristic sender, GattWriteRequestedEventArgs args)
        {
            var deferral = args.GetDeferral();

            var request = await args.GetRequestAsync();
            var buf = request.Value.ToArray();

            _relayService.SetConfiguration((RelayConfiguration)buf[0]);

            if (request.Option == GattWriteOption.WriteWithResponse)
            {
                request.Respond();
            }

            deferral.Complete();
        }
    }
}
