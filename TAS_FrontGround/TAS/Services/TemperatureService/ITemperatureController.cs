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

        bool ReadSlaveInfoSequence(out ushort[] slaveInfoArgs);
        bool ReadSlaveDataSequence(out ushort[] slaveDataArgs);

        bool WriteSlaveDateTimeSequence(ushort[] slaveDateTimeArgs);
        bool WriteSlaveFrequencySequence(ushort[] slaveFrequencyArgs);
    }
}
