using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.Content;
using Android.Nfc;
using Android.Nfc.Tech;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using QiMata.ConfigureControlManage.Droid.PlatformServices;
using QiMata.ConfigureControlManage.Services;
using Xamarin.Forms;
using Application = Android.App.Application;

[assembly: Xamarin.Forms.Dependency(typeof(NfcScanner))]
namespace QiMata.ConfigureControlManage.Droid.PlatformServices
{
    
    public class NfcScanner : INfcScanner
    {
        private readonly NfcAdapter _nfcAdapter;

        public NfcScanner()
        {
            _nfcAdapter =  NfcAdapter.GetDefaultAdapter(Application.Context);
        }

        public Task<byte[]> Scan(TimeSpan timeout)
        {
            Callback callback = new Callback(timeout);
            _nfcAdapter.EnableReaderMode((MainActivity)Forms.Context, callback,
                NfcReaderFlags.NfcB | NfcReaderFlags.NfcA | NfcReaderFlags.NfcBarcode | NfcReaderFlags.NfcF | NfcReaderFlags.NfcV,null);
            return callback.Task;
        }

        class Callback : Java.Lang.Object ,NfcAdapter.IReaderCallback
        {
            private readonly TaskCompletionSource<byte[]> _completionSource;

            public Callback(TimeSpan timeout)
            {
                _completionSource = new TaskCompletionSource<byte[]>(timeout);
            }

            public void OnTagDiscovered(Tag tag)
            {
                var strs = tag.GetTechList();
                var nfc = Ndef.Get(tag);
                nfc.Connect();
                _completionSource.SetResult(nfc.NdefMessage.ToByteArray());
            }

            public Task<byte[]> Task => _completionSource.Task;
        }
    }
}