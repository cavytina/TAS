using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using Prism.Ioc;
using Prism.Mvvm;
using TAS.Services;
using System.Diagnostics;

namespace TAS.Models
{
    public class TransmissionModel : BindableBase
    {
        IContainerProvider containerProvider;
        IDataBaseController dataBaseController;
        ISerialPortController serialPortController;
        ITemperatureController temperatureController;

        string sqlString;
        List<CategoryKind> dictionaryDataHub;
        List<BaseKind> defaultDataHub;

        ObservableCollection<CategoryKind> connectionKinds;
        public ObservableCollection<CategoryKind> ConnectionKinds
        {
            get => connectionKinds;
            set => SetProperty(ref connectionKinds, value);
        }

        CategoryKind currentConnectionKind;
        public CategoryKind CurrentConnectionKind
        {
            get => currentConnectionKind;
            set => SetProperty(ref currentConnectionKind, value);
        }

        ObservableCollection<string> serialPortNameKinds;
        public ObservableCollection<string> SerialPortNameKinds
        {
            get => serialPortNameKinds;
            set => SetProperty(ref serialPortNameKinds, value);
        }

        string currentSerialPortNameKind;
        public string CurrentSerialPortNameKind
        {
            get => currentSerialPortNameKind;
            set => SetProperty(ref currentSerialPortNameKind, value);
        }

        ObservableCollection<CategoryKind> baudRateKinds;
        public ObservableCollection<CategoryKind> BaudRateKinds
        {
            get => baudRateKinds;
            set => SetProperty(ref baudRateKinds, value);
        }

        CategoryKind currentBaudRateKind;
        public CategoryKind CurrentBaudRateKind
        {
            get => currentBaudRateKind;
            set => SetProperty(ref currentBaudRateKind, value);
        }

        ObservableCollection<CategoryKind> dataBitKinds;
        public ObservableCollection<CategoryKind> DataBitKinds
        {
            get => dataBitKinds;
            set => SetProperty(ref dataBitKinds, value);
        }

        CategoryKind currentDataBitKind;
        public CategoryKind CurrentDataBitKind
        {
            get => currentDataBitKind;
            set => SetProperty(ref currentDataBitKind, value);
        }

        ObservableCollection<CategoryKind> stopBitsKinds;
        public ObservableCollection<CategoryKind> StopBitsKinds
        {
            get => stopBitsKinds;
            set => SetProperty(ref stopBitsKinds, value);
        }

        CategoryKind currentStopBitsKind;
        public CategoryKind CurrentStopBitsKind
        {
            get => currentStopBitsKind;
            set => SetProperty(ref currentStopBitsKind, value);
        }

        ObservableCollection<CategoryKind> parityKinds;
        public ObservableCollection<CategoryKind> ParityKinds
        {
            get => parityKinds;
            set => SetProperty(ref parityKinds, value);
        }

        CategoryKind currentParityKind;
        public CategoryKind CurrentParityKind
        {
            get => currentParityKind;
            set => SetProperty(ref currentParityKind, value);
        }

        bool isOpen;
        public bool IsOpen
        {
            get => isOpen;
            set => SetProperty(ref isOpen, value);
        }

        string slaveDateTime;
        public string SlaveDateTime
        {
            get => slaveDateTime;
            set => SetProperty(ref slaveDateTime, value);
        }

        string slaveTemperature;
        public string SlaveTemperature
        {
            get => slaveTemperature;
            set => SetProperty(ref slaveTemperature, value);
        }

        public TransmissionModel(IContainerProvider containerProviderArgs)
        {
            containerProvider = containerProviderArgs;
        }

        public void LoadData()
        {
            LoadDataBaseData();
            SearchSerialPort();

            LoadConnectionData();
            LoadBaudRateData();
            LoadDataBitData();
            LoadStopBitsData();
            LoadParityData();
        }

        internal void LoadDataBaseData()
        {
            dataBaseController = containerProvider.Resolve<IDataBaseController>();

            sqlString = "SELECT Code,Name,Content,Description,Rank,Flag,CategoryCode,CategoryName FROM TAS_DictionarySetting";
            dataBaseController.Query<CategoryKind>(sqlString, out dictionaryDataHub);

            sqlString = "SELECT Code,Name,Content,Description,Rank,Flag FROM TAS_DefaultSetting";
            dataBaseController.Query<BaseKind>(sqlString, out defaultDataHub);
        }

        internal void LoadConnectionData()
        {
            IOrderedEnumerable<CategoryKind> connetions = from connectionHub in dictionaryDataHub
                                                          where connectionHub.Flag == true && connectionHub.CategoryCode == "01"
                                                          orderby connectionHub.Rank
                                                          select connectionHub;
            ConnectionKinds = new ObservableCollection<CategoryKind>(connetions.ToList());

            IEnumerable<BaseKind> defaultConnetion = from defaultConnetionHub in defaultDataHub
                                                     where defaultConnetionHub.Flag == true && defaultConnetionHub.Code == "01"
                                                     select defaultConnetionHub;
            CurrentConnectionKind = ConnectionKinds.FirstOrDefault(x => x.Content == defaultConnetion.FirstOrDefault().Content);
        }

        public void SearchSerialPort()
        {
            serialPortController = containerProvider.Resolve<ISerialPortController>();
            SerialPortNameKinds = new ObservableCollection<string>(serialPortController.GetSerialPortName());

            IEnumerable<BaseKind> defaultSerialPort = from defaultSerialPortHub in defaultDataHub
                                                      where defaultSerialPortHub.Flag == true && defaultSerialPortHub.Code == "02"
                                                      select defaultSerialPortHub;
            CurrentSerialPortNameKind = SerialPortNameKinds.FirstOrDefault(x => x == defaultSerialPort.FirstOrDefault().Content);
        }

        internal void LoadBaudRateData()
        {
            IOrderedEnumerable<CategoryKind> baudRates = from baudRateHub in dictionaryDataHub
                                                         where baudRateHub.Flag == true && baudRateHub.CategoryCode == "03"
                                                         orderby baudRateHub.Rank
                                                         select baudRateHub;
            BaudRateKinds = new ObservableCollection<CategoryKind>(baudRates.ToList());

            IEnumerable<BaseKind> defaultBaudRate = from defaultBaudRateHub in defaultDataHub
                                                    where defaultBaudRateHub.Flag == true && defaultBaudRateHub.Code == "03"
                                                    select defaultBaudRateHub;
            CurrentBaudRateKind = BaudRateKinds.FirstOrDefault(x => x.Content == defaultBaudRate.FirstOrDefault().Content);
        }

        internal void LoadDataBitData()
        {
            IOrderedEnumerable<CategoryKind> dataBits = from dataBitHub in dictionaryDataHub
                                                        where dataBitHub.Flag == true && dataBitHub.CategoryCode == "04"
                                                        orderby dataBitHub.Rank
                                                        select dataBitHub;
            DataBitKinds = new ObservableCollection<CategoryKind>(dataBits.ToList());

            IEnumerable<BaseKind> defaultDataBit = from defaultDataBitHub in defaultDataHub
                                                   where defaultDataBitHub.Flag == true && defaultDataBitHub.Code == "04"
                                                   select defaultDataBitHub;
            CurrentDataBitKind = DataBitKinds.FirstOrDefault(x => x.Content == defaultDataBit.FirstOrDefault().Content);
        }

        internal void LoadStopBitsData()
        {
            IOrderedEnumerable<CategoryKind> stopBits = from stopBitsHub in dictionaryDataHub
                                                        where stopBitsHub.Flag == true && stopBitsHub.CategoryCode == "05"
                                                        orderby stopBitsHub.Rank
                                                        select stopBitsHub;
            StopBitsKinds = new ObservableCollection<CategoryKind>(stopBits.ToList());

            IEnumerable<BaseKind> defaultStopBits = from defaultStopBitsHub in defaultDataHub
                                                    where defaultStopBitsHub.Flag == true && defaultStopBitsHub.Code == "05"
                                                    select defaultStopBitsHub;
            CurrentStopBitsKind = StopBitsKinds.FirstOrDefault(x => x.Content == defaultStopBits.FirstOrDefault().Content);
        }

        internal void LoadParityData()
        {
            IOrderedEnumerable<CategoryKind> paritys = from parityHub in dictionaryDataHub
                                                       where parityHub.Flag == true && parityHub.CategoryCode == "06"
                                                       orderby parityHub.Rank
                                                       select parityHub;
            ParityKinds = new ObservableCollection<CategoryKind>(paritys.ToList());

            IEnumerable<BaseKind> defaultParity = from defaultParityHub in defaultDataHub
                                                  where defaultParityHub.Flag == true && defaultParityHub.Code == "06"
                                                  select defaultParityHub;
            CurrentParityKind = ParityKinds.FirstOrDefault(x => x.Content == defaultParity.FirstOrDefault().Content);
        }

        public void Connection()
        {
            serialPortController.PortName = CurrentSerialPortNameKind;
            serialPortController.BaudRate = int.Parse(CurrentBaudRateKind.Content);
            serialPortController.DataBits = int.Parse(CurrentDataBitKind.Content);
            serialPortController.StopBits = (StopBits)Enum.Parse(typeof(StopBits), CurrentStopBitsKind.Name);
            serialPortController.Parity = (Parity)Enum.Parse(typeof(Parity), CurrentParityKind.Name);

            if (IsOpen == false)
            {
                serialPortController.Initialize();
                serialPortController.Open();
                IsOpen = serialPortController.IsOpen;
                if (IsOpen)
                {
                    SaveDefaultData();

                    temperatureController = containerProvider.Resolve<ITemperatureController>();
                    temperatureController.Initialize();
                }
            }
            else
            {
                serialPortController.Close();
                IsOpen = serialPortController.IsOpen;
            }
        }

        internal void SaveDefaultData()
        {
            sqlString = "UPDATE TAS_DefaultSetting SET Content = '" + CurrentConnectionKind.Content + "' WHERE Code = '01'";
            dataBaseController.Execute(sqlString);

            sqlString = "UPDATE TAS_DefaultSetting SET Content = '" + CurrentSerialPortNameKind + "' WHERE Code = '02'";
            dataBaseController.Execute(sqlString);

            sqlString = "UPDATE TAS_DefaultSetting SET Content = '" + CurrentBaudRateKind.Content + "' WHERE Code = '03'";
            dataBaseController.Execute(sqlString);

            sqlString = "UPDATE TAS_DefaultSetting SET Content = '" + CurrentDataBitKind.Content + "' WHERE Code = '04'";
            dataBaseController.Execute(sqlString);

            sqlString = "UPDATE TAS_DefaultSetting SET Content = '" + CurrentStopBitsKind.Content + "' WHERE Code = '05'";
            dataBaseController.Execute(sqlString);

            sqlString = "UPDATE TAS_DefaultSetting SET Content = '" + CurrentParityKind.Content + "' WHERE Code = '06'";
            dataBaseController.Execute(sqlString);
        }

        public bool FetchSlaveInfo()
        {
            bool ret = false;

            if (FetchSlaveDataTime())
                if (FetchSlaveTemperature())
                    ret = true;

            return ret;
        }

        internal bool FetchSlaveDataTime()
        {
            bool ret = false;

            ushort[] slaveDateTime;
            if (temperatureController.ReadSlaveDateTime(out slaveDateTime))
            {
                SlaveDateTime = "20" + slaveDateTime[5].ToString() + "-" + slaveDateTime[4].ToString().PadLeft(2,'0') + "-" + slaveDateTime[3].ToString() +
                    " " +
                    slaveDateTime[2].ToString() + ":" + slaveDateTime[1].ToString() + ":" + slaveDateTime[0].ToString();

                ret = true;
            }

            return ret;
        }

        internal bool FetchSlaveTemperature()
        {
            bool ret = false;

            ushort[] slaveTemperatureArgs;
            if (temperatureController.ReadSlaveTemperature(out slaveTemperatureArgs))
            {
                ushort slaveTemperature = slaveTemperatureArgs[0];
                ushort slaveTemperatureValue;

                byte slaveTemperatureSignBit = (byte)(slaveTemperature >> 15);
                if (Convert.ToBoolean(slaveTemperatureSignBit))
                {
                    ushort slaveTemperatureTurnValue = (ushort)((ushort)~slaveTemperature + 0x01);
                    SlaveTemperature = "-" + (slaveTemperatureTurnValue * 0.0625).ToString() + "℃";
                }
                else
                {
                    slaveTemperatureValue = slaveTemperature;
                    SlaveTemperature = (slaveTemperatureValue * 0.0625).ToString() + "℃";
                }

                ret = true;
            }

            return ret;
        }

        public bool SyncSlaveDateTime()
        {
            bool ret = false;

            ushort second = Convert.ToUInt16(DateTime.Now.Second);
            ushort minute = Convert.ToUInt16(DateTime.Now.Minute);
            ushort hour = Convert.ToUInt16(DateTime.Now.Hour);

            ushort day = Convert.ToUInt16(DateTime.Now.Day);
            ushort month = Convert.ToUInt16(DateTime.Now.Month);
            ushort year = Convert.ToUInt16(DateTime.Now.Year - 2000);

            ushort[] currentDateTime = new ushort[]
            {
                Convert.ToUInt16(DateTime.Now.Second) ,
                Convert.ToUInt16(DateTime.Now.Minute),
                Convert.ToUInt16(DateTime.Now.Hour),
                Convert.ToUInt16(DateTime.Now.Day),
                Convert.ToUInt16(DateTime.Now.Month),
                Convert.ToUInt16(DateTime.Now.Year - 2000)
            };

            if (temperatureController.WriteSlaveDateTime(currentDateTime))
                ret = true;

            return ret;
        }
    }
}