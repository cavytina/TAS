using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;

namespace TAS.Services
{
    public class SlaveTransientErrorDetectionStrategy : ITransientErrorDetectionStrategy
    {
        public bool IsTransient(Exception ex)
        {
            if (ex != null)
            {
                SlaveNotResponseException slaveNotResponseException;
                if ((slaveNotResponseException = ex as SlaveNotResponseException) != null)
                    return true;
            }

            return false;
        }
    }
}