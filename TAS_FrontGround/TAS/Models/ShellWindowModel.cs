using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Ioc;
using Prism.Mvvm;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using TAS.Models;
using TAS.Services;

namespace TAS.Models
{
    public class ShellWindowModel:BindableBase
    {
        ObservableCollection<MenuKind> menuKinds;
        public ObservableCollection<MenuKind> MenuKinds
        {
            get => menuKinds;
            set => SetProperty(ref menuKinds, value);
        }

        MenuKind currentMenuKind;
        public MenuKind CurrentMenuKind
        {
            get => currentMenuKind;
            set => SetProperty(ref currentMenuKind, value);
        }

        IContainerProvider containerProvider;
        IDataBaseController dataBaseController;

        string queryText;

        public ShellWindowModel(IContainerProvider containerProviderArgs)
        {
            containerProvider = containerProviderArgs;
            dataBaseController = containerProviderArgs.Resolve<IDataBaseController>();
        }

        public void LoadMenuData()
        {
            queryText = "SELECT MenuCode,MenuName FROM MenuKind";

            List<MenuKind> outMenuKinds = new List<MenuKind>();
            if (dataBaseController.Query<MenuKind>(queryText, out outMenuKinds))
                MenuKinds = new ObservableCollection<MenuKind>(outMenuKinds);
        }
    }
}
