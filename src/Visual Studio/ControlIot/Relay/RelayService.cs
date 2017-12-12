using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Gpio;

namespace ControlIot.Relay
{
    class RelayService
    {
        private GpioPin _pinOne;
        private GpioPin _pinTwo;

        public RelayService(int pinOne,int pinTwo)
        {
            var controller = GpioController.GetDefault();

            _pinOne = controller.OpenPin(pinOne, GpioSharingMode.Exclusive);
            _pinTwo = controller.OpenPin(pinTwo, GpioSharingMode.Exclusive);

            _pinOne.SetDriveMode(GpioPinDriveMode.Output);
            _pinTwo.SetDriveMode(GpioPinDriveMode.Output);
        }

        public void SetConfiguration(RelayConfiguration configuration)
        {
            switch (configuration)
            {
                case RelayConfiguration.Off:
                    _pinOne.Write(GpioPinValue.Low);
                    _pinTwo.Write(GpioPinValue.Low);
                    break;
                case RelayConfiguration.FirstOn:
                    _pinOne.Write(GpioPinValue.High);
                    _pinTwo.Write(GpioPinValue.Low);
                    break;
                case RelayConfiguration.SecondOn:
                    _pinOne.Write(GpioPinValue.Low);
                    _pinTwo.Write(GpioPinValue.High);
                    break;
                case RelayConfiguration.FirstAndSecondOn:
                    _pinOne.Write(GpioPinValue.High);
                    _pinTwo.Write(GpioPinValue.High);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(configuration), configuration, null);
            }
        }
    }
}
