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
	public partial class CharacteristicReadWritePage : ContentPage
	{
		public CharacteristicReadWritePage ()
		{
			InitializeComponent ();
		}

	    protected override void OnAppearing()
	    {
	        base.OnAppearing();

	        if (BindingContext is CharacteristicReadWriteViewModel vm)
	        {
	            vm.OnAppearing();
	        }
	    }

	    protected override void OnDisappearing()
	    {
	        base.OnDisappearing();

	        if (BindingContext is CharacteristicReadWriteViewModel vm)
	        {
	            vm.OnDisappearing();
	        }
        }
	}
}