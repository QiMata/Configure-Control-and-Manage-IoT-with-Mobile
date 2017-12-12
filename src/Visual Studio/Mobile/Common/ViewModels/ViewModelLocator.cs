using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QiMata.ConfigureControlManage.ViewModels.Azure;
using QiMata.ConfigureControlManage.ViewModels.Bluetooth;

namespace QiMata.ConfigureControlManage.ViewModels
{
    class ViewModelLocator
    {
        private BluetoothScanViewModel _bluetoothScanViewModel;
        private NfcViewModel _nfcViewModel;
        private AzureViewModel _azureViewModel;
        private VoiceViewModel _voiceViewModel;

        public BluetoothScanViewModel BluetoothScanViewModel =>
            _bluetoothScanViewModel ?? (_bluetoothScanViewModel = new BluetoothScanViewModel());

        public NfcViewModel NfcViewModel =>
            _nfcViewModel ?? (_nfcViewModel = new NfcViewModel());

        public AzureViewModel AzureViewModel =>
            _azureViewModel ?? (_azureViewModel = new AzureViewModel());

        public VoiceViewModel VoiceViewModel =>
            _voiceViewModel ?? (_voiceViewModel = new VoiceViewModel());
    }
}
