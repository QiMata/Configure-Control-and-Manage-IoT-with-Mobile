using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlIot.Bluetooth.Helpers
{
    class RotatingString
    {
        private readonly ConcurrentDictionary<int,string> _stringsList;

        private int _currentRecord;

        public RotatingString(IEnumerable<string> stringsList)
        {
            _stringsList = new ConcurrentDictionary<int,string>(stringsList.Select((x,y) => new KeyValuePair<int, string>(y,x)));
        }

        public string GetNext()
        {
            if (_currentRecord >= _stringsList.Count)
            {
                _currentRecord = 0;
            }

            var str = _stringsList[_currentRecord];

            _currentRecord++;

            return str;
        }
    }
}
