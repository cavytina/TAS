using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.Models
{
    /// <summary>
    /// 命令字
    /// </summary>
    public enum CommandWordPart
    {
        /// <summary>
        /// 读从机信息 Hex:0x01
        /// </summary>
        ReadInfo,

        /// <summary>
        /// 读从机数据 Hex:0x02
        /// </summary>
        ReadData,

        /// <summary>
        /// 写从机时间 Hex:0x11
        /// </summary>
        WriteDateTime,

        /// <summary>
        /// 写从机温度频率 Hex:0x12
        /// </summary>
        WriteFrequency
    }
}
