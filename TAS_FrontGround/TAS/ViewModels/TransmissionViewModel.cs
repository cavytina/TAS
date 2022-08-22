using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Commands;
using MaterialDesignThemes.Wpf;
using TAS.Models;

namespace TAS.ViewModels
{
    public class TransmissionViewModel : BindableBase
    {
        IContainerProvider containerProvider;
        ISnackbarMessageQueue messageQueue;

        TransmissionModel transmissionModel;
        public TransmissionModel TransmissionModel
        {
            get => transmissionModel;
            set => SetProperty(ref transmissionModel, value);
        }

        public DelegateCommand WindowLoadedCommand { get; private set; }
        public DelegateCommand SearchSerialPortCommand { get; private set; }
        public DelegateCommand ConnectionCommand { get; private set; }
        public DelegateCommand FetchSlaveInfoCommand { get; private set; }
        public DelegateCommand FetchSlaveDataCommand { get; private set; }
        public DelegateCommand SyncSlaveDateTimeCommand { get; private set; }

        public TransmissionViewModel(IContainerProvider containerProviderArgs)
        {
            containerProvider = containerProviderArgs;
            messageQueue = containerProviderArgs.Resolve<ISnackbarMessageQueue>();

            TransmissionModel = new TransmissionModel(containerProviderArgs);
            WindowLoadedCommand = new DelegateCommand(OnWindowLoaded);
            SearchSerialPortCommand = new DelegateCommand(OnSearchSerialPort);
            ConnectionCommand = new DelegateCommand(OnConnection);

            FetchSlaveInfoCommand= new DelegateCommand(OnFetchSlaveInfo);
            FetchSlaveDataCommand = new DelegateCommand(OnFetchSlaveData);
            SyncSlaveDateTimeCommand= new DelegateCommand(OnSyncSlaveDateTime);
        }

        private void OnWindowLoaded()
        {
            TransmissionModel.LoadData();
        }

        private void OnSyncSlaveDateTime()
        {
            if (TransmissionModel.SyncSlaveDateTime())
                messageQueue.Enqueue("同步从机时间成功!");
        }

        private void OnFetchSlaveData()
        {
            if (TransmissionModel.FetchSlaveData())
                messageQueue.Enqueue("获取从机数据成功!");
        }

        private void OnFetchSlaveInfo()
        {
            if (TransmissionModel.FetchSlaveInfo())
                messageQueue.Enqueue("获取从机信息成功!");
        }

        private void OnSearchSerialPort()
        {
            TransmissionModel.SearchSerialPort();
        }

        private void OnConnection()
        {
            TransmissionModel.Connection();
        }
    }
}