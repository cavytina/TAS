using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.Services
{
    public interface IModBusController
    {
        void WriteSingleCoil(byte slaveAddressArgs, ushort coilAddressArgs, bool valueArgs);
        void WriteSingleRegister(byte slaveAddressArgs, ushort registerAddressArgs, ushort valueArgs);
        void WriteMultipleRegisters(byte slaveAddressArgs, ushort registerAddressArgs, ushort[] valueArgs);

        bool[] ReadCoils(byte slaveAddressArgs, ushort startAddressArgs, ushort numberOfPointsArgs);
        ushort[] ReadHoldingRegisters(byte slaveAddressArgs, ushort startAddressArgs, ushort numberOfPointsArgs);
    }
}
