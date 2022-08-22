using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Ioc;
using Prism.Mvvm;
using TAS.Models;
using TAS.Services;

namespace TAS.Models
{
    public class ShellWindowModel : BindableBase
    {
        IDataBaseController dataBaseController;

        string sqlString;
        List<NestKind> menuDataHub;

        ObservableCollection<NestKind> menuKinds;
        public ObservableCollection<NestKind> MenuKinds
        {
            get => menuKinds;
            set => SetProperty(ref menuKinds, value);
        }

        NestKind currentMenuKind;
        public NestKind CurrentMenuKind
        {
            get => currentMenuKind;
            set => SetProperty(ref currentMenuKind, value);
        }

        public ShellWindowModel(IContainerProvider containerProviderArgs)
        {
            dataBaseController = containerProviderArgs.Resolve<IDataBaseController>();
        }

        public void LoadMenuData()
        {
            sqlString = "SELECT Code,Name,Content,Description,SubCode,SubName,Rank,Flag FROM System_MenuSetting";
            dataBaseController.Query<NestKind>(sqlString, out menuDataHub);

            var menus = from menuHub in menuDataHub
                        where menuHub.Flag && menuHub.SubCode == "01"
                        orderby menuHub.Rank
                        select menuHub;

            MenuKinds = new ObservableCollection<NestKind>(menus.ToList());
        }
    }
}
