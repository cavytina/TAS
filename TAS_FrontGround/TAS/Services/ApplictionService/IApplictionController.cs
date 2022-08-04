using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using TAS.Models;

namespace TAS.Services
{
    public interface IApplictionController
    {
        ValidationResults ApplictionValidationResults { get; set; }

        void Validate(ApplictionServicePart applictionServicePartArgs);
        void Initialize(ApplictionServicePart applictionServicePartArgs);
    }
}
