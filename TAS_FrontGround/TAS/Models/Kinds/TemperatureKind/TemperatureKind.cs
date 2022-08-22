using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.Models
{
    public struct TemperatureKind : ITemperatureKind
    {
        public DateTime SlaveCurrentDataTime { get; set; }
        public float SlaveCurrentTemperature { get; set; }
        public ushort SlaveCurrentPageStatus { get; set; }

        public TemperatureKind(DateTime slaveCurrentDataTimeArgs,float slaveCurrentTemperatureArgs,ushort slaveCurrentPageStatusArgs)
        {
            SlaveCurrentDataTime = slaveCurrentDataTimeArgs;
            SlaveCurrentTemperature = slaveCurrentTemperatureArgs;
            SlaveCurrentPageStatus = slaveCurrentPageStatusArgs;
        }
    }
}
