using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QiMata.ConfigureControlManage.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace QiMata.ConfigureControlManage.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NfcPage : BaseContentPage
    {
        public NfcPage()
        {
            InitializeComponent();
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            if (BindingContext is NfcViewModel vm)
            {
                vm._errorDisplayFunc = async x => await DisplayAlert("Error", x, "Ok");
            }
        }
    }
}