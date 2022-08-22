using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.Models
{
    public interface INestKind : IBaseKind
    {
        string SubCode { get; set; }
        string SubName { get; set; }
    }
}
