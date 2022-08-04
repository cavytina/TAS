using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAS.Models;

namespace TAS.Services
{
    public interface ITemperatureController
    {
        void Initialize();
        bool ReadSlaveDateTime(out ushort[] slaveDateTimeArgs);
        bool WriteSlaveDateTime(ushort[] slaveDateTimeArgs);
        bool ReadSlaveTemperature(out ushort[] slaveTemperatureArgs);
    }
}
