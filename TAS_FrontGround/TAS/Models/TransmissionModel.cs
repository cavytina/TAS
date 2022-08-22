using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.ComponentModel;
using System.IO.Ports;
using Prism.Ioc;
using Prism.Mvvm;
using TAS.Services;


namespace TAS.Models
{
    public class TransmissionModel : BindableBase
    {
        IContainerProvider containerProvider;
        IDataBaseController dataBaseController;
        ISerialPortController serialPortController;
        ITemperatureController temperatureController;
        ILogController logController;

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
            set
            {
                SetProperty(ref currentConnectionKind, value);

                if (value.Content == "SerialPort")
                    IsSerialPortSelected = true;
                else
                    IsSerialPortSelected = false;
            }
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

        ObservableCollection<CategoryKind> modeKinds;
        public ObservableCollection<CategoryKind> ModeKinds
        {
            get => modeKinds;
            set => SetProperty(ref modeKinds, value);
        }

        CategoryKind currentModeKind;
        public CategoryKind CurrentModeKind
        {
            get => currentModeKind;
            set => SetProperty(ref currentModeKind, value);
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

        bool isSerialPortSelected;
        public bool IsSerialPortSelected
        {
            get => isSerialPortSelected;
            set => SetProperty(ref isSerialPortSelected, value);
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

        double slaveFetchValue;
        public double SlaveFetchValue
        {
            get => slaveFetchValue;
            set => SetProperty(ref slaveFetchValue, value);
        }

        public TransmissionModel(IContainerProvider containerProviderArgs)
        {
            containerProvider = containerProviderArgs;
            logController = containerProviderArgs.Resolve<ILogController>();
        }

        public void LoadData()
        {
            LoadDataBaseData();
            SearchSerialPort();

            LoadConnectionData();
            LoadModeData();
            LoadBaudRateData();
            LoadDataBitData();
            LoadStopBitsData();
            LoadParityData();

            SlaveDateTime = "0000-00-00 00:00:00";
            SlaveTemperature = "0";
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
            var connetions = from connectionHub in dictionaryDataHub
                             where connectionHub.Flag == true && connectionHub.CategoryCode == "01"
                             orderby connectionHub.Rank
                             select connectionHub;
            ConnectionKinds = new ObservableCollection<CategoryKind>(connetions.ToList());

            var defaultConnetion = from defaultConnetionHub in defaultDataHub
                                   where defaultConnetionHub.Flag == true && defaultConnetionHub.Code == "01"
                                   select defaultConnetionHub;
            CurrentConnectionKind = ConnectionKinds.FirstOrDefault(x => x.Content == defaultConnetion.FirstOrDefault().Content);
        }

        internal void LoadModeData()
        {
            var modes = from modeHub in dictionaryDataHub
                        where modeHub.Flag && modeHub.CategoryCode == "02"
                        orderby modeHub.Rank
                        select modeHub;
            ModeKinds = new ObservableCollection<CategoryKind>(modes.ToList());

            var defaultMode = from defaultModeHub in defaultDataHub
                              where defaultModeHub.Flag && defaultModeHub.Code == "02"
                              select defaultModeHub;
            CurrentModeKind = ModeKinds.FirstOrDefault(x => x.Content == defaultMode.FirstOrDefault().Content);
        }


        public void SearchSerialPort()
        {
            serialPortController = containerProvider.Resolve<ISerialPortController>();
            SerialPortNameKinds = new ObservableCollection<string>(serialPortController.GetSerialPortName());

            var defaultSerialPortName = from defaultSerialPortNameHub in defaultDataHub
                                        where defaultSerialPortNameHub.Flag && defaultSerialPortNameHub.Code == "03"
                                        select defaultSerialPortNameHub;
            CurrentSerialPortNameKind = SerialPortNameKinds.FirstOrDefault(x => x == defaultSerialPortName.FirstOrDefault().Content);
        }

        internal void LoadBaudRateData()
        {
            var baudRates = from baudRateHub in dictionaryDataHub
                            where baudRateHub.Flag && baudRateHub.CategoryCode == "03"
                            orderby baudRateHub.Rank
                            select baudRateHub;
            BaudRateKinds = new ObservableCollection<CategoryKind>(baudRates.ToList());

            var defaultBaudRate = from defaultBaudRateHub in defaultDataHub
                                  where defaultBaudRateHub.Flag && defaultBaudRateHub.Code == "04"
                                  select defaultBaudRateHub;
            CurrentBaudRateKind = BaudRateKinds.FirstOrDefault(x => x.Content == defaultBaudRate.FirstOrDefault().Content);
        }

        internal void LoadDataBitData()
        {
            var dataBits = from dataBitHub in dictionaryDataHub
                           where dataBitHub.Flag && dataBitHub.CategoryCode == "04"
                           orderby dataBitHub.Rank
                           select dataBitHub;
            DataBitKinds = new ObservableCollection<CategoryKind>(dataBits.ToList());

            var defaultDataBit = from defaultDataBitHub in defaultDataHub
                                 where defaultDataBitHub.Flag && defaultDataBitHub.Code == "05"
                                 select defaultDataBitHub;
            CurrentDataBitKind = DataBitKinds.FirstOrDefault(x => x.Content == defaultDataBit.FirstOrDefault().Content);
        }

        internal void LoadStopBitsData()
        {
            var stopBits = from stopBitsHub in dictionaryDataHub
                           where stopBitsHub.Flag && stopBitsHub.CategoryCode == "05"
                           orderby stopBitsHub.Rank
                           select stopBitsHub;
            StopBitsKinds = new ObservableCollection<CategoryKind>(stopBits.ToList());

            var defaultStopBits = from defaultStopBitsHub in defaultDataHub
                                  where defaultStopBitsHub.Flag && defaultStopBitsHub.Code == "06"
                                  select defaultStopBitsHub;
            CurrentStopBitsKind = StopBitsKinds.FirstOrDefault(x => x.Content == defaultStopBits.FirstOrDefault().Content);
        }

        internal void LoadParityData()
        {
            var paritys = from parityHub in dictionaryDataHub
                          where parityHub.Flag == true && parityHub.CategoryCode == "06"
                          orderby parityHub.Rank
                          select parityHub;
            ParityKinds = new ObservableCollection<CategoryKind>(paritys.ToList());

            var defaultParity = from defaultParityHub in defaultDataHub
                                where defaultParityHub.Flag == true && defaultParityHub.Code == "07"
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
            serialPortController.IsRtu = CurrentModeKind.Content == "RTU";

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

            sqlString = "UPDATE TAS_DefaultSetting SET Content = '" + CurrentModeKind.Content + "' WHERE Code = '02'";
            dataBaseController.Execute(sqlString);

            sqlString = "UPDATE TAS_DefaultSetting SET Content = '" + CurrentSerialPortNameKind + "' WHERE Code = '03'";
            dataBaseController.Execute(sqlString);

            sqlString = "UPDATE TAS_DefaultSetting SET Content = '" + CurrentBaudRateKind.Content + "' WHERE Code = '04'";
            dataBaseController.Execute(sqlString);

            sqlString = "UPDATE TAS_DefaultSetting SET Content = '" + CurrentDataBitKind.Content + "' WHERE Code = '05'";
            dataBaseController.Execute(sqlString);

            sqlString = "UPDATE TAS_DefaultSetting SET Content = '" + CurrentStopBitsKind.Content + "' WHERE Code = '06'";
            dataBaseController.Execute(sqlString);

            sqlString = "UPDATE TAS_DefaultSetting SET Content = '" + CurrentParityKind.Content + "' WHERE Code = '07'";
            dataBaseController.Execute(sqlString);
        }

        public bool FetchSlaveInfo()
        {
            bool ret = false;
            TemperatureKind temperatureKind;

            if (ConvertSlaveInfo(out temperatureKind))
            {
                SlaveDateTime = temperatureKind.SlaveCurrentDataTime.ToString("G");
                SlaveTemperature = temperatureKind.SlaveCurrentTemperature.ToString();

                ret = true;
            }

            return ret;
        }

        public bool FetchSlaveData()
        {
            bool ret = false;
            TemperatureKind temperatureKind = default;
            temperatureKind.SlaveCurrentPageStatus = 0x7FFF;

            while (CalculateHighBitsAmount(temperatureKind.SlaveCurrentPageStatus) != 0)
            {
                ConvertSlaveData(out temperatureKind);
                int slaveCurrentPageStatusHighBitsAmount = CalculateHighBitsAmount(temperatureKind.SlaveCurrentPageStatus);
                SlaveFetchValue = slaveCurrentPageStatusHighBitsAmount / 15.0 * 100;
            }

            ret = true;

            return ret;
        }

        internal int CalculateHighBitsAmount(ushort slavePageStatusBitsArgs)
        {
            BitArray slavePageStatusBits = new BitArray(BitConverter.GetBytes(slavePageStatusBitsArgs));
            bool[] slavePageStatusBooleans = new bool[slavePageStatusBits.Length];

            slavePageStatusBits.CopyTo(slavePageStatusBooleans, 0);
            return slavePageStatusBooleans.Count(x => x == true);
        }

        internal bool ConvertSlaveInfo(out TemperatureKind temperatureKindArgs)
        {
            bool ret = false;
            temperatureKindArgs = default;

            ushort[] slaveInfo;
            if (temperatureController.ReadSlaveInfoSequence(out slaveInfo))
            {
                string slaveDateTime = "20" + slaveInfo[5].ToString() + "-" + slaveInfo[4].ToString().PadLeft(2, '0') + "-" + slaveInfo[3].ToString().PadLeft(2, '0') +
                    " " + slaveInfo[2].ToString() + ":" + slaveInfo[1].ToString() + ":" + slaveInfo[0].ToString();

                temperatureKindArgs.SlaveCurrentDataTime = DateTime.Parse(slaveDateTime);

                ushort slaveTemperature = slaveInfo[6];
                ushort slaveTemperatureValue;

                byte slaveTemperatureSignBit = (byte)(slaveTemperature >> 15);
                if (Convert.ToBoolean(slaveTemperatureSignBit))
                {
                    ushort slaveTemperatureTurnValue = (ushort)((ushort)~slaveTemperature + 0x01);
                    temperatureKindArgs.SlaveCurrentTemperature = -Convert.ToSingle(slaveTemperatureTurnValue * 0.0625);
                }
                else
                {
                    slaveTemperatureValue = slaveTemperature;
                    temperatureKindArgs.SlaveCurrentTemperature = Convert.ToSingle(slaveTemperatureValue * 0.0625);
                }

                ret = true;
            }

            return ret;
        }

        internal bool ConvertSlaveData(out TemperatureKind temperatureKindArgs)
        {
            bool ret = false;
            temperatureKindArgs = default;

            ushort[] slaveData;
            if (temperatureController.ReadSlaveDataSequence(out slaveData))
            {
                string slaveDateTime = "20" + slaveData[5].ToString() + "-" + slaveData[4].ToString().PadLeft(2, '0') + "-" + slaveData[3].ToString().PadLeft(2, '0') +
                    " " + slaveData[2].ToString() + ":" + slaveData[1].ToString() + ":" + slaveData[0].ToString();

                temperatureKindArgs.SlaveCurrentDataTime = DateTime.Parse(slaveDateTime);

                ushort slaveTemperature = slaveData[6];
                ushort slaveTemperatureValue;

                byte slaveTemperatureSignBit = (byte)(slaveTemperature >> 15);
                if (Convert.ToBoolean(slaveTemperatureSignBit))
                {
                    ushort slaveTemperatureTurnValue = (ushort)((ushort)~slaveTemperature + 0x01);
                    temperatureKindArgs.SlaveCurrentTemperature = -Convert.ToSingle(slaveTemperatureTurnValue * 0.0625);
                }
                else
                {
                    slaveTemperatureValue = slaveTemperature;
                    temperatureKindArgs.SlaveCurrentTemperature = Convert.ToSingle(slaveTemperatureValue * 0.0625);
                }

                ushort slavePageStatus= slaveData[7];
                temperatureKindArgs.SlaveCurrentPageStatus = slavePageStatus;

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

            if (temperatureController.WriteSlaveDateTimeSequence(currentDateTime))
                ret = true;

            return ret;
        }
    }
}