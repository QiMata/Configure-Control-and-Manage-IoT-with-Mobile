using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QiMata.ConfigureControlManage.Services
{
    public interface INfcScanner
    {
        Task<byte[]> Scan(TimeSpan timeout);
    }
}
