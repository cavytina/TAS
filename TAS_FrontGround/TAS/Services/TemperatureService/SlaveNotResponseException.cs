using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.Services
{
    public class SlaveNotResponseException : ApplicationException
    {
        public SlaveNotResponseException()
        {

        }

        public SlaveNotResponseException(string message) : base(message)
        {

        }
    }
}