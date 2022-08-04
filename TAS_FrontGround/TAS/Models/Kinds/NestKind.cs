using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.Models
{
    public class NestKind : BaseKind
    {
        /// <summary>
        /// 嵌套基础设置类
        /// </summary>
        public string SubCode { get; set; }
        public string SubName { get; set; }
    }
}