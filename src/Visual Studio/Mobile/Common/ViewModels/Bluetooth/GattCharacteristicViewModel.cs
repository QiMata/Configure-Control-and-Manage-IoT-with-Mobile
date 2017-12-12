using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Plugin.BluetoothLE;
using QiMata.ConfigureControlManage.Views.Bluetooth;
using Xamarin.Forms;

namespace QiMata.ConfigureControlManage.ViewModels.Bluetooth
{
    class GattCharacteristicViewModel : ViewModelBase
    {
        private readonly IScanResult _result;
        private IDisposable _characteristicDiscoveredDisposable;

        public GattCharacteristicViewModel(IScanResult result)
        {
            _result = result;

            GattTappedCommand = new Command(async x => await NavigateToCharacteristic((IGattCharacteristic)x));
        }

        public ICommand GattTappedCommand { get; }

        internal INavigation Navigation { private get; set; }

        public ObservableCollection<IGattCharacteristic> GattCharacteristics { get; } =
            new ObservableCollection<IGattCharacteristic>();

        public void OnAppearing()
        {
            Task.Run(async () =>
            {
                await _result.Device.Connect();
                _characteristicDiscoveredDisposable = _result.Device.WhenAnyCharacteristicDiscovered().Subscribe(characteristic =>
                {
                    int previousIndex = -1;

                    for (int i = 0; i < GattCharacteristics.Count; i++)
                    {
                        if (GattCharacteristics[i].Uuid == characteristic.Uuid)
                        {
                            previousIndex = i;
                            break;
                        }
                    }

                    if (previousIndex == -1)
                    {
                        GattCharacteristics.Add(characteristic);
                    }
                    else
                    {
                        GattCharacteristics[previousIndex] = characteristic;
                    }
                });
            });
        }

        public void OnDisappearing()
        {
            _characteristicDiscoveredDisposable?.Dispose();
        }

        public string Title => _result.AdvertisementData.LocalName ?? _result?.Device?.Uuid.ToString();

        private Task NavigateToCharacteristic(IGattCharacteristic gattCharacteristic)
        {
            return Navigation.PushAsync(new CharacteristicReadWritePage
            {
                BindingContext = new CharacteristicReadWriteViewModel(gattCharacteristic)
            });
        }
    }
}
