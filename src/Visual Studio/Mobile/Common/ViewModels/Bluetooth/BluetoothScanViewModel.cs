using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Plugin.BluetoothLE;
using QiMata.ConfigureControlManage.Views.Bluetooth;
using Xamarin.Forms;

namespace QiMata.ConfigureControlManage.ViewModels.Bluetooth
{
    class BluetoothScanViewModel : ViewModelBase
    {
        public BluetoothScanViewModel()
        {
            ScanCommand = new Command(Scan);

            DeviceTappedCommand = new Command(async x => await NavigateToCharacteristic((IScanResult)x));
        }

        public ICommand ScanCommand { get; }

        internal INavigation Navigation { private get; set; }

        public ObservableCollection<IScanResult> Devices { get; } = new ObservableCollection<IScanResult>();

        public void Scan()
        {
            if (CrossBleAdapter.Current.IsScanning)
            {
                return;
            }

            // discover some devices
            CrossBleAdapter.Current.Scan(new ScanConfig
            {
                ScanType = BleScanType.Balanced,
                ServiceUuid = Guid.Parse("FE08FF91-8B9D-4D4D-9135-4A8F376D2F09")
            }).Subscribe(scanResult =>
            {
                int previousIndex = -1;

                for (int i = 0; i < Devices.Count; i++)
                {
                    if (Devices[i].Device.Uuid == scanResult.Device.Uuid)
                    {
                        previousIndex = i;
                        break;
                    }
                }

                if (previousIndex == -1)
                {
                    Devices.Add(scanResult);
                }
                else
                {
                    Devices[previousIndex] = scanResult;
                }

                // Once finding the device/scanresult you want
                //await scanResult.Device.Connect();

                //scanResult.Device.WhenAnyCharacteristicDiscovered().Subscribe(async characteristic =>
                //{
                //    // read, write, or subscribe to notifications here
                //    var charResult = await characteristic.Read().SingleAsync(); // use result.Data to see response
                //    //await characteristic.Write(bytes);

                //    await characteristic.EnableNotifications();
                //    characteristic.WhenNotificationReceived().Subscribe(result =>
                //    {
                //        //result.Data to get at response
                //    });
                //});
            });
        }

        public ICommand DeviceTappedCommand { get; }

        private async Task NavigateToCharacteristic(IScanResult scanResult)
        {
            await Navigation.PushAsync(new GattCharacteristicPage
            {
                BindingContext = new GattCharacteristicViewModel(scanResult)
            });
        }
    }
}
