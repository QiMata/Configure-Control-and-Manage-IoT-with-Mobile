using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Plugin.BluetoothLE;
using Plugin.Permissions;
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
                CrossBleAdapter.Current.ScanListen()
                    .Subscribe(HandleScanResult);
            }
            else
            {
                // discover some devices
                CrossBleAdapter.Current.Scan(new ScanConfig
                {
                    ScanType = BleScanType.Balanced
                }).Subscribe(HandleScanResult);
            }

            
        }

        private void HandleScanResult(IScanResult scanResult)
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
