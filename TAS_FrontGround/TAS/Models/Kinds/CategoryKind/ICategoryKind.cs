using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.Models
{
    public interface ICategoryKind : IBaseKind
    {
        string CategoryCode { get; set; }
        string CategoryName { get; set; }
    }
}
