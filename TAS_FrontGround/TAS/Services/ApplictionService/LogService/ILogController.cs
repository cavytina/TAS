using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.Services
{
    public interface ILogController
    {
        void Initialize();
        void WriteLog(string MessageArgs);
    }
}
