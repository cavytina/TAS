using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.Services
{
    public interface IDataBaseController
    {
        void Initialize();

        bool Query<T>(string queryStingArgs, out List<T> tHub);
        bool ExecuteScalar(string execStingArgs, out object objArgs);
        bool Execute(string execStingArgs);
    }
}
