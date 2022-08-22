using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.Models
{
    /// <summary>
    /// 状态字
    /// </summary>
    public enum StatusWordPart
    {
        /// <summary>
        /// 主机请求位:主机写1,从机写0
        /// </summary>
        MasterRequest,

        /// <summary>
        /// 从机应答位:从机写1,主机写0
        /// </summary>
        SlaveResponse
    }
}
