using QiMata.ConfigureControlManage.ViewModels.Bluetooth;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace QiMata.ConfigureControlManage.Views.Bluetooth
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BluetoothScanPage : ContentPage
    {
        public BluetoothScanPage()
        {
            InitializeComponent();
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            if (BindingContext is BluetoothScanViewModel vm)
            {
                vm.Navigation = Navigation;
            }
        }
    }
}