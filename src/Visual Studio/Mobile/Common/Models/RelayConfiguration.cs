using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QiMata.ConfigureControlManage.Models
{
    public enum RelayConfiguration : byte
    {
        Off = 0,
        FirstOn = 1,
        SecondOn = 2,
        FirstAndSecondOn = 3
    }
}
