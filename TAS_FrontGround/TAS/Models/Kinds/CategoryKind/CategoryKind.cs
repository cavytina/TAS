using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.Models
{
    /// <summary>
    /// 分类字典设置类型
    /// </summary>
    public struct CategoryKind : ICategoryKind
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public string Description { get; set; }
        public string CategoryCode { get; set; }
        public string CategoryName { get; set; }
        public int Rank { get; set; }
        public bool Flag { get; set; }

        public CategoryKind(string codeArgs, string nameArgs, string contentArgs, string descriptionArgs,
                                string categoryCodeArgs, string categoryNameArgs, int rankArgs, bool flagArgs)
        {
            Code = codeArgs;
            Name = nameArgs;
            Content = contentArgs;
            Description = descriptionArgs;
            CategoryCode = categoryCodeArgs;
            CategoryName = categoryNameArgs;
            Rank = rankArgs;
            Flag = flagArgs;
        }
    }
}