using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;
using Prism.Ioc;
using TAS.Models;

namespace TAS.Services
{
    public class TemperatureController : TemperatureBase, ITemperatureController
    {
        ILogController logController;

        public TemperatureController(IContainerProvider containerProviderArgs) : base(containerProviderArgs)
        {
            logController = containerProviderArgs.Resolve<ILogController>();
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        internal override void ExecuteStatusWordAction(StatusWordPart statusWordPartArgs)
        {
            base.ExecuteStatusWordAction(statusWordPartArgs);
        }

        internal override void ExecuteCommandWordAction(CommandWordPart commandWordPartArgs)
        {
            base.ExecuteCommandWordAction(commandWordPartArgs);
        }

        internal override void ExecuteDataWordAction(DataWordPart dataWordPartArgs, bool isWriteArgs, ushort[] writeValueArgs, out ushort[] readValueArgs)
        {
            base.ExecuteDataWordAction(dataWordPartArgs, isWriteArgs, writeValueArgs, out readValueArgs);
        }

        internal override bool ExecuteSlaveResponseWithRetryAction()
        {
            return base.ExecuteSlaveResponseWithRetryAction();
        }

        /// <summary>
        /// 读从机时间步骤
        /// 1.置位命令寄存器
        /// 2.时间寄存器数据清零
        /// 3.置位主机请求位
        /// 4.延迟等待从机置位应答位
        /// 5.读取时间寄存器数据
        /// 6.复位从机应答位
        /// </summary>
        public bool ReadSlaveDateTime(out ushort[] slaveDateTimeArgs)
        {
            bool ret = false;
            slaveDateTimeArgs = new ushort[] { 0 };

            ExecuteCommandWordAction(CommandWordPart.ReadDateTime);
            ExecuteDataWordAction(DataWordPart.DateTime, true,new ushort[] { 0, 0, 0, 0, 0, 0 },  out slaveDateTimeArgs);
            ExecuteStatusWordAction(StatusWordPart.MasterRequestBit);

            if (ExecuteSlaveResponseWithRetryAction())
            {
                ExecuteDataWordAction(DataWordPart.DateTime, false, new ushort[] { 0, 0, 0, 0, 0, 0 }, out slaveDateTimeArgs);
                ExecuteStatusWordAction(StatusWordPart.SlaveResponseBit);
                ret = true;
            }

            return ret;
        }

        /// <summary>
        /// 写从机时间步骤
        /// 1.置位命令寄存器
        /// 2.设置时间寄存器数据
        /// 3.置位主机请求位
        /// 4.延迟等待从机置位应答位
        /// 5.复位从机应答位
        /// </summary>
        public bool WriteSlaveDateTime(ushort[] slaveDateTimeArgs)
        {
            bool ret = false;
            ushort[] readSlaveDateTime;

            ExecuteCommandWordAction(CommandWordPart.WriteDateTime);
            ExecuteDataWordAction(DataWordPart.DateTime, true, slaveDateTimeArgs, out readSlaveDateTime);
            ExecuteStatusWordAction(StatusWordPart.MasterRequestBit);

            if (ExecuteSlaveResponseWithRetryAction())
            {
                ExecuteStatusWordAction(StatusWordPart.SlaveResponseBit);
                ret = true;
            }

            return ret;
        }

        /// <summary>
        /// 读从机温度步骤
        /// 1.置位命令寄存器
        /// 2.温度寄存器数据清零
        /// 3.置位主机请求位
        /// 4.延迟等待从机置位应答位
        /// 5.读取温度寄存器数据
        /// 6.复位从机应答位
        /// </summary>
        public bool ReadSlaveTemperature(out ushort[] slaveTemperatureArgs)
        {
            bool ret = false;
            slaveTemperatureArgs = new ushort[] { 0 };

            ExecuteCommandWordAction(CommandWordPart.ReadTemperature);
            ExecuteDataWordAction(DataWordPart.Temperature, true, new ushort[] { 0, 0 }, out slaveTemperatureArgs);
            ExecuteStatusWordAction(StatusWordPart.MasterRequestBit);

            if (ExecuteSlaveResponseWithRetryAction())
            {
                ExecuteDataWordAction(DataWordPart.Temperature, false, new ushort[] { 0, 0 }, out slaveTemperatureArgs);
                ExecuteStatusWordAction(StatusWordPart.SlaveResponseBit);
                ret = true;
            }

            return ret;
        }
    }
}
