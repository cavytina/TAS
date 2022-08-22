using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.Models
{
    /// <summary>
    /// 嵌套字典设置类型
    /// </summary>
    public struct NestKind : INestKind
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public string Description { get; set; }
        public string SubCode { get; set; }
        public string SubName { get; set; }
        public int Rank { get; set; }
        public bool Flag { get; set; }

        public NestKind(string codeArgs, string nameArgs, string contentArgs, string descriptionArgs,
                          string subCodeArgs, string subNameArgs, int rankArgs, bool flagArgs)
        {
            Code = codeArgs;
            Name = nameArgs;
            Content = contentArgs;
            Description = descriptionArgs;
            SubCode = subCodeArgs;
            SubName = subNameArgs;
            Rank = rankArgs;
            Flag = flagArgs;
        }
    }
}