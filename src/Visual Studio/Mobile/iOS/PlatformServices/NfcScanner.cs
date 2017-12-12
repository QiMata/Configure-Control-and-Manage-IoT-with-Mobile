using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using QiMata.ConfigureControlManage.iOS.PlatformServices;
using QiMata.ConfigureControlManage.Services;

[assembly: Xamarin.Forms.Dependency(typeof(NfcScanner))]
namespace QiMata.ConfigureControlManage.iOS.PlatformServices
{
    public class NfcScanner : INfcScanner
    {
        public Task<byte[]> Scan(TimeSpan timeout)
        {
            throw new PlatformNotSupportedException("Nfc not supported on iOS");
        }
    }
}
