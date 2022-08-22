using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.Models
{
    public interface IBaseKind
    {
        string Code { get; set; }
        string Name { get; set; }
        string Content { get; set; }
        string Description { get; set; }
        int Rank { get; set; }
        bool Flag { get; set; }
    }
}
