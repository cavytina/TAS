using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAS.Models;

namespace TAS.Services
{
    public interface IApplictionPathController
    {
        string ApplictionCatalogue { get; set; }
        string NativeDataBaseFilePath { get; set; }
        string TextLogFilePath { get; set; }

        void Initialize();
        void Save();
    }
}
