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
    public partial class VoicePage : BaseContentPage
    {
        public VoicePage()
        {
            InitializeComponent();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            if (BindingContext is VoiceViewModel vm)
            {
                vm.OnDisappearing();
            }
        }
    }
}