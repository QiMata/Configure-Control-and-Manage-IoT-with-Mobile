using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QiMata.ConfigureControlManage.ViewModels
{
    class ViewModelLocator
    {
        private BluetoothScanViewModel _bluetoothScanViewModel;

        public BluetoothScanViewModel BluetoothScanViewModel =>
            _bluetoothScanViewModel ?? (_bluetoothScanViewModel = new BluetoothScanViewModel());
    }
}
