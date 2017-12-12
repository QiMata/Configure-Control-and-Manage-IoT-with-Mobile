using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Plugin.BluetoothLE;
using Xamarin.Forms;

namespace QiMata.ConfigureControlManage.ViewModels.Bluetooth
{
    class CharacteristicReadWriteViewModel : ViewModelBase
    {
        private IGattCharacteristic _characteristic;
        private IDisposable _characteristicConnectionDisposable;

        public CharacteristicReadWriteViewModel(IGattCharacteristic characteristic)
        {
            _characteristic = characteristic;

            WriteCommand = new Command(async () => await Write());
        }

        public void OnAppearing()
        {
            if (_characteristic.CanNotify())
            {
                _characteristic.EnableNotifications();
                _characteristicConnectionDisposable = _characteristic?.WhenNotificationReceived().Subscribe(x =>
                {
                    ReadValue = Encoding.UTF8.GetString(x.Data,0,x.Data.Length);
                });
            }
        }

        public void OnDisappearing()
        {
            _characteristicConnectionDisposable?.Dispose();
        }

        public ICommand WriteCommand { get; }

        private string _title;

        public string Title
        {
            get => _title;
            set
            {
                if (_title == value)
                {
                    return;
                }
                _title = value;
                OnPropertyChanged();
            }
        }

        private string _writeValue;

        public string WriteValue
        {
            get => _writeValue;
            set
            {
                if (_writeValue == value)
                {
                    return;
                }
                _writeValue = value;
                OnPropertyChanged();
            }
        }

        private string _readValue;

        public string ReadValue
        {
            get => _readValue;
            set
            {
                if (_readValue == value)
                {
                    return;
                }
                _readValue = value;
                OnPropertyChanged();
            }
        }

        public async Task Write()
        {
            var result = await _characteristic.Write(new []{byte.Parse(WriteValue)}).SingleAsync();

            ReadValue = Encoding.UTF8.GetString(result.Data, 0, result.Data.Length);
        }
    }
}
