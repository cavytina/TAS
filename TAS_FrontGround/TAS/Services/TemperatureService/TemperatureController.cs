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
        /// 读从机当前信息命令序列
        /// 1.置位命令寄存器
        /// 2.时间寄存器数据清零
        /// 3.温度寄存器数据清零
        /// 4.置位主机请求位
        /// 4.延迟等待从机置位应答位
        /// 5.读取时间寄存器数据
        /// 6.读取温度寄存器数据
        /// 7.复位从机应答位
        /// </summary>
        public bool ReadSlaveInfoSequence(out ushort[] slaveInfoArgs)
        {
            bool ret = false;
            ushort[] slaveDateTime, slaveTemperature;

            ExecuteCommandWordAction(CommandWordPart.ReadInfo);
            ExecuteDataWordAction(DataWordPart.Timer, true, new ushort[] { 0, 0, 0, 0, 0, 0 }, out slaveDateTime);
            ExecuteDataWordAction(DataWordPart.Temperature, true, new ushort[] { 0 }, out slaveTemperature);
            ExecuteStatusWordAction(StatusWordPart.MasterRequest);

            if (ExecuteSlaveResponseWithRetryAction())
            {
                ExecuteDataWordAction(DataWordPart.Timer, false, new ushort[] { 0, 0, 0, 0, 0, 0 }, out slaveDateTime);
                ExecuteDataWordAction(DataWordPart.Temperature, false, new ushort[] { 0 }, out slaveTemperature);
                ExecuteStatusWordAction(StatusWordPart.SlaveResponse);
                ret = true;
            }

            slaveInfoArgs = slaveDateTime.Append<ushort>(slaveTemperature.FirstOrDefault()).ToArray<ushort>();

            return ret;
        }

        /// <summary>
        /// 读从机数据命令序列
        /// 1.置位命令寄存器
        /// 2.时间寄存器数据清零
        /// 3.温度寄存器数据清零
        /// 4.页状态寄存器数据清零
        /// 5.置位主机请求位
        /// 6.延迟等待从机置位应答位
        /// 7.读取时间寄存器数据
        /// 8.读取温度寄存器数据
        /// 9.读取页状态寄存器数据
        /// 10.复位从机应答位
        /// </summary>
        public bool ReadSlaveDataSequence(out ushort[] slaveDataArgs)
        {
            bool ret = false;
            ushort[] slaveDateTime, slaveTemperature, slavePageStutas;

            ExecuteCommandWordAction(CommandWordPart.ReadData);
            ExecuteDataWordAction(DataWordPart.Timer, true, new ushort[] { 0, 0, 0, 0, 0, 0 }, out slaveDateTime);
            ExecuteDataWordAction(DataWordPart.Temperature, true, new ushort[] { 0 }, out slaveTemperature);
            ExecuteDataWordAction(DataWordPart.PageStutas, true, new ushort[] { 0 }, out slavePageStutas);
            ExecuteStatusWordAction(StatusWordPart.MasterRequest);

            if (ExecuteSlaveResponseWithRetryAction())
            {
                ExecuteDataWordAction(DataWordPart.Timer, false, new ushort[] { 0, 0, 0, 0, 0, 0 }, out slaveDateTime);
                ExecuteDataWordAction(DataWordPart.Temperature, false, new ushort[] { 0 }, out slaveTemperature);
                ExecuteDataWordAction(DataWordPart.PageStutas, false, new ushort[] { 0 }, out slavePageStutas);
                ExecuteStatusWordAction(StatusWordPart.SlaveResponse);
                ret = true;
            }

            ushort[] slaveInfoArgs = slaveDateTime.Append<ushort>(slaveTemperature.FirstOrDefault()).ToArray<ushort>();
            slaveDataArgs = slaveInfoArgs.Append<ushort>(slavePageStutas.FirstOrDefault()).ToArray<ushort>();

            return ret;
        }

        /// <summary>
        /// 写从机时间命令序列
        /// 1.置位命令寄存器
        /// 2.设置时间寄存器数据
        /// 3.置位主机请求位
        /// 4.延迟等待从机置位应答位
        /// 5.复位从机应答位
        /// </summary>
        public bool WriteSlaveDateTimeSequence(ushort[] slaveDateTimeArgs)
        {
            bool ret = false;

            ExecuteCommandWordAction(CommandWordPart.WriteDateTime);
            ExecuteDataWordAction(DataWordPart.Timer, true, slaveDateTimeArgs, out slaveDateTimeArgs);
            ExecuteStatusWordAction(StatusWordPart.MasterRequest);

            if (ExecuteSlaveResponseWithRetryAction())
            {
                ExecuteStatusWordAction(StatusWordPart.SlaveResponse);
                ret = true;
            }

            return ret;
        }

        /// <summary>
        /// 写从机频率命令序列
        /// 1.置位命令寄存器
        /// 2.设置频率寄存器数据
        /// 3.置位主机请求位
        /// 4.延迟等待从机置位应答位
        /// 5.复位从机应答位
        /// </summary>
        public bool WriteSlaveFrequencySequence(ushort[] slaveFrequencyArgs)
        {
            bool ret = false;
            ushort[] slaveFrequency;

            ExecuteCommandWordAction(CommandWordPart.WriteFrequency);
            ExecuteDataWordAction(DataWordPart.Frequency, true, slaveFrequencyArgs, out slaveFrequency);
            ExecuteStatusWordAction(StatusWordPart.MasterRequest);

            if (ExecuteSlaveResponseWithRetryAction())
            {
                ExecuteStatusWordAction(StatusWordPart.SlaveResponse);
                ret = true;
            }

            return ret;
        }
    }
}
