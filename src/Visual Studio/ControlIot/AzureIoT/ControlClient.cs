using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ControlIot.Bluetooth.Helpers;
using ControlIot.Relay;
using Microsoft.Azure.Devices.Client;

namespace ControlIot.AzureIoT
{
    class ControlClient
    {
        private readonly RelayService _relayService;
        private readonly DeviceClient _deviceClient;
        private readonly RotatingString _rotatingString;

        private Task _backgroundTask;

        public ControlClient(string connectionString, RelayService relayService,IEnumerable<string> stringList)
        {
            _relayService = relayService;
            _deviceClient = DeviceClient.CreateFromConnectionString(connectionString);
            _rotatingString = new RotatingString(stringList);

            _backgroundTask = ReadAndWriteInBackground();
        }

        private async Task ReadAndWriteInBackground()
        {
            while (true)
            {
                try
                {
                    var messageWrapper = await _deviceClient.ReceiveAsync();

                    if (messageWrapper != null)
                    {
                        var message = messageWrapper.GetBytes();
                        await _deviceClient.CompleteAsync(messageWrapper);
                        _relayService.SetConfiguration((RelayConfiguration)message[0]);
                    }

                    await _deviceClient.SendEventAsync(new Message(Encoding.UTF8.GetBytes(_rotatingString.GetNext())));

                    await Task.Delay(TimeSpan.FromSeconds(1));
                }
                catch (Exception e)
                {
                }
            }
        }
    }
}
