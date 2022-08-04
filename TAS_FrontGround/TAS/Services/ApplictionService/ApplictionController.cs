using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Ioc;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using TAS.Models;

namespace TAS.Services
{
    public class ApplictionController : IApplictionController
    {
        IContainerProvider containerProvider;

        public ValidationResults ApplictionValidationResults { get; set; }

        IApplictionPathController applictionPathController;
        ILogController logController;
        IDataBaseController dataBaseController;

        public ApplictionController(IContainerProvider containerProviderArgs)
        {
            containerProvider = containerProviderArgs;
            ApplictionValidationResults = new ValidationResults();
        }

        public void Validate(ApplictionServicePart applictionServicePartArgs)
        {
            switch (applictionServicePartArgs)
            {
                case ApplictionServicePart.ApplictionPath:
                    Validator<ApplictionPathController> applictionPathControllerValidator = ValidationFactory.CreateValidator<ApplictionPathController>();
                    ApplictionValidationResults.AddAllResults(applictionPathControllerValidator.Validate(applictionPathController));
                    break;
                case ApplictionServicePart.Log:
                    Validator<TextLogController> txtLogControllerValidator = ValidationFactory.CreateValidator<TextLogController>();
                    ApplictionValidationResults.AddAllResults(txtLogControllerValidator.Validate(logController));
                    break;
                case ApplictionServicePart.DataBase:
                    Validator<NativeBaseController> nativeBaseControllerValidator = ValidationFactory.CreateValidator<NativeBaseController>();
                    ApplictionValidationResults.AddAllResults(nativeBaseControllerValidator.Validate(dataBaseController));
                    break;
            }
        }

        public void Initialize(ApplictionServicePart applictionServicePartArgs)
        {
            if (ApplictionValidationResults.IsValid)
            {
                switch (applictionServicePartArgs)
                {
                    case ApplictionServicePart.ApplictionPath:
                        applictionPathController = containerProvider.Resolve<IApplictionPathController>();
                        applictionPathController.Initialize();
                        break;
                    case ApplictionServicePart.Log:
                        logController = containerProvider.Resolve<ILogController>();
                        logController.Initialize();
                        break;
                    case ApplictionServicePart.DataBase:
                        dataBaseController = containerProvider.Resolve<IDataBaseController>();
                        dataBaseController.Initialize();
                        break;
                }
            }
        }
    }
}