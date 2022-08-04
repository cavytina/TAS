using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Prism.Unity;
using Prism.Ioc;
using MaterialDesignThemes.Wpf;
using TAS.Models;
using TAS.Views;
using TAS.Services;

namespace TAS
{
    public class TASBootstrapper : PrismBootstrapper
    {
        IApplictionController applictionController;

        protected override DependencyObject CreateShell()
        {
            return Container.Resolve<ShellWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<TransmissionView>("Transmission");
            containerRegistry.RegisterForNavigation<QueryView>("Query");
            containerRegistry.RegisterForNavigation<SetupView>("Setup");

            containerRegistry.RegisterSingleton<ISnackbarMessageQueue, SnackbarMessageQueue>();

            containerRegistry.RegisterSingleton<IApplictionController, ApplictionController>();
            containerRegistry.RegisterSingleton<IApplictionPathController, ApplictionPathController>();
            containerRegistry.RegisterSingleton<ILogController, TextLogController>();
            containerRegistry.RegisterSingleton<IDataBaseController, NativeBaseController>();

            //TODO:如何通过IModBusController接口获取SerialPortController?
            containerRegistry.RegisterSingleton<ISerialPortController, SerialPortController>();

            containerRegistry.RegisterSingleton<ITemperatureController, TemperatureController>();
        }

        protected override void Initialize()
        {
            base.Initialize();
            InitializeAndValidateService();
        }

        internal void InitializeAndValidateService()
        {
            applictionController= Container.Resolve<IApplictionController>();
            applictionController.Initialize(ApplictionServicePart.ApplictionPath);
            applictionController.Validate(ApplictionServicePart.ApplictionPath);

            if (applictionController.ApplictionValidationResults.IsValid)
            {
                applictionController.Initialize(ApplictionServicePart.Log);
                applictionController.Validate(ApplictionServicePart.Log);

                if (applictionController.ApplictionValidationResults.IsValid)
                {
                    applictionController.Initialize(ApplictionServicePart.DataBase);
                    applictionController.Validate(ApplictionServicePart.DataBase);
                }
            }
        }
    }
}