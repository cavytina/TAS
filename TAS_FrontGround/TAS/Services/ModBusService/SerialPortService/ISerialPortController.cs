using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;

namespace TAS.Services
{
    public interface ISerialPortController : IModBusController
    {
        List<string> GetSerialPortName();

        string PortName { get; set; }
        int BaudRate { get; set; }
        int DataBits { get; set; }
        Parity Parity { get; set; }
        StopBits StopBits { get; set; }

        bool IsRtu { get; set; }

        void Initialize();

        void Open();
        void Close();

        bool IsOpen { get; }
    }
}