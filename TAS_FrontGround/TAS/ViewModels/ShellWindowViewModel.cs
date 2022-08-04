using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Commands;
using Prism.Regions;
using MaterialDesignThemes.Wpf;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using TAS.Models;
using TAS.Views;
using TAS.Services;

namespace TAS.ViewModels
{
    public class ShellWindowViewModel : BindableBase
    {
        ISnackbarMessageQueue messageQueue;
        public ISnackbarMessageQueue MessageQueue
        {
            get => messageQueue;
            set => SetProperty(ref messageQueue, value);
        }

        ShellWindowModel shellWindowModel;
        public ShellWindowModel ShellWindowModel
        {
            get => shellWindowModel;
            set => SetProperty(ref shellWindowModel, value);
        }

        IContainerProvider containerProvider;
        IRegionManager regionManager;
        IApplictionController applictionController;

        public DelegateCommand WindowLoadedCommand { get; private set; }

        public ShellWindowViewModel(IContainerProvider containerProviderArgs)
        {
            containerProvider = containerProviderArgs;
            regionManager = containerProvider.Resolve<IRegionManager>();
            MessageQueue = containerProvider.Resolve<ISnackbarMessageQueue>();
            WindowLoadedCommand = new DelegateCommand(OnWindowLoaded);
        }

        private void OnWindowLoaded()
        {
            ShellWindowModel = new ShellWindowModel(containerProvider);

            ShellWindowModel.LoadMenuData();

            regionManager.RegisterViewWithRegion("ContentRegion", typeof(TransmissionView));
        }
    }
}