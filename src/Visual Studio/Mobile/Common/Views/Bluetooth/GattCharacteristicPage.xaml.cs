using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QiMata.ConfigureControlManage.ViewModels.Bluetooth;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace QiMata.ConfigureControlManage.Views.Bluetooth
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GattCharacteristicPage : ContentPage
    {
        public GattCharacteristicPage()
        {
            InitializeComponent();
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            if (BindingContext is GattCharacteristicViewModel vm)
            {
                vm.Navigation = Navigation;
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (BindingContext is GattCharacteristicViewModel vm)
            {
                vm.OnAppearing();
            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            if (BindingContext is GattCharacteristicViewModel vm)
            {
                vm.OnDisappearing();
            }
        }
    }
}