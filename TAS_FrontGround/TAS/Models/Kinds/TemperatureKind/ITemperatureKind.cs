using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.Models
{
    public interface ITemperatureKind
    {
        DateTime SlaveCurrentDataTime { get; set; }
        float SlaveCurrentTemperature { get; set; }
        ushort SlaveCurrentPageStatus { get; set; }
    }
}
