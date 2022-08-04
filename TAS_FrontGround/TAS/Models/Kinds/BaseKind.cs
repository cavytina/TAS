using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.Models
{
    /// <summary>
    /// 基础字典设置类
    /// </summary>
    public class BaseKind
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public string Description { get; set; }
        public int Rank { get; set; }
        public bool Flag { get; set; }
    }
}
