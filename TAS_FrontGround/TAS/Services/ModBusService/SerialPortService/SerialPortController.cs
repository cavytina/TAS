using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using Prism.Ioc;
using Modbus.Data;
using Modbus.Device;
using Modbus.Utility;
using TAS.Models;

namespace TAS.Services
{
    public class SerialPortController : ISerialPortController
    {
        SerialPort serialPort;
        ModbusSerialMaster  modbusSerialMaster;
        ILogController logController;
        IContainerProvider containerProvider;

        public string PortName { get; set; }
        public int BaudRate { get; set; }
        public int DataBits { get; set; }
        public Parity Parity { get; set; }
        public StopBits StopBits { get; set; }
        public bool IsOpen => serialPort.IsOpen;

        public List<string> GetSerialPortName()
        {
            return SerialPort.GetPortNames().ToList();
        }

        public SerialPortController(IContainerProvider containerProviderArgs)
        {
            containerProvider = containerProviderArgs;
            serialPort = new SerialPort();
        }

        public void Initialize()
        {
            serialPort.PortName = PortName;
            serialPort.BaudRate = BaudRate;
            serialPort.DataBits = DataBits;
            serialPort.Parity = Parity;
            serialPort.StopBits = StopBits;

            logController = containerProvider.Resolve<ILogController>();
            modbusSerialMaster = ModbusSerialMaster.CreateRtu(serialPort);
        }

        public void Open()
        {
            serialPort.Open();
        }

        public void Close()
        {
            serialPort.Close();
        }

        public void WriteSingleCoil(byte slaveAddressArgs, ushort coilAddressArgs, bool valueArgs)
        {
            modbusSerialMaster.WriteSingleCoil(slaveAddressArgs, coilAddressArgs, valueArgs);

            logController.WriteLog("调用写单线圈:从机地址" +
                         slaveAddressArgs.ToString() + ",线圈地址:" +
                         coilAddressArgs.ToString() + ",值:" +
                         valueArgs.ToString());
        }

        public void WriteSingleRegister(byte slaveAddressArgs, ushort registerAddressArgs, ushort valueArgs)
        {
            modbusSerialMaster.WriteSingleRegister(slaveAddressArgs, registerAddressArgs, valueArgs);

            logController.WriteLog("调用写单寄存器:从机地址" +
                         slaveAddressArgs.ToString() + ",寄存器地址:" +
                         registerAddressArgs.ToString() + ",值:" +
                         valueArgs.ToString());
        }

        public void WriteMultipleRegisters(byte slaveAddressArgs, ushort registerAddressArgs, ushort[] valueArgs)
        {
            modbusSerialMaster.WriteMultipleRegisters(slaveAddressArgs, registerAddressArgs, valueArgs);

            string str = string.Empty;
            valueArgs.ToList().ForEach(x => { str += x.ToString() + " "; });
            logController.WriteLog("调用写多寄存器:从机地址" +
                         slaveAddressArgs.ToString() + ",寄存器地址:" +
                         registerAddressArgs.ToString() + ",值:" +
                         str);
        }

        public bool[] ReadCoils(byte slaveAddressArgs, ushort startAddressArgs, ushort numberOfPointsArgs)
        {
            logController.WriteLog("调用读线圈:从机地址" +
                         slaveAddressArgs.ToString() + ",起始地址:" +
                         startAddressArgs.ToString() + ",数量:" +
                         numberOfPointsArgs.ToString());

            return modbusSerialMaster.ReadCoils(slaveAddressArgs, startAddressArgs, numberOfPointsArgs);
        }

        public ushort[] ReadHoldingRegisters(byte slaveAddressArgs, ushort startAddressArgs, ushort numberOfPointsArgs)
        {
            logController.WriteLog("调用读寄存器:从机地址" +
                         slaveAddressArgs.ToString() + ",起始地址:" +
                         startAddressArgs.ToString() + ",数量:" +
                         numberOfPointsArgs.ToString());

            return modbusSerialMaster.ReadHoldingRegisters(slaveAddressArgs, startAddressArgs, numberOfPointsArgs);
        }
    }
}