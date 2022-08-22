using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Ioc;
using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;
using TAS.Models;

namespace TAS.Services
{
    public abstract class TemperatureBase
    {
        IDataBaseController dataBaseController;
        ISerialPortController serialPortController;
        string sqlString;
        object retDataBaseObj;

        byte slaveID;

        ushort masterRequestCoilAddress, slaveResponseCoilAddress;
        ushort masterCommandRegisterAddress, timerRegisterAddress, temperatureRegisterAddress, pageStutasRegisterAddress, frequencyRegisterAddress;

        int retryCount, currentRetryCount;
        TimeSpan retryIntervalTimeSpan;
        bool slaveResponseCoilBit;

        internal TemperatureBase(IContainerProvider containerProviderArgs)
        {
            dataBaseController = containerProviderArgs.Resolve<IDataBaseController>();
            serialPortController = containerProviderArgs.Resolve<ISerialPortController>();
        }

        public virtual void Initialize()
        {
            sqlString = "SELECT Content FROM TAS_DefaultSetting WHERE Name = 'SlaveID'";
            dataBaseController.ExecuteScalar(sqlString, out retDataBaseObj);
            slaveID = Convert.ToByte(retDataBaseObj);

            sqlString = "SELECT Content FROM TAS_DictionarySetting WHERE CategoryCode = '07' AND Name = 'MA_REQ'";
            dataBaseController.ExecuteScalar(sqlString, out retDataBaseObj);
            masterRequestCoilAddress = Convert.ToUInt16(retDataBaseObj);

            sqlString = "SELECT Content FROM TAS_DictionarySetting WHERE CategoryCode = '07' AND Name = 'SLV_RESP'";
            dataBaseController.ExecuteScalar(sqlString, out retDataBaseObj);
            slaveResponseCoilAddress = Convert.ToUInt16(retDataBaseObj);

            sqlString = "SELECT Content FROM TAS_DictionarySetting WHERE CategoryCode = '09' AND Name = 'MA_CMD'";
            dataBaseController.ExecuteScalar(sqlString, out retDataBaseObj);
            masterCommandRegisterAddress = Convert.ToUInt16(retDataBaseObj);

            sqlString = "SELECT Content FROM TAS_DictionarySetting WHERE CategoryCode = '09' AND Name = 'CLK_SEC'";
            dataBaseController.ExecuteScalar(sqlString, out retDataBaseObj);
            timerRegisterAddress = Convert.ToUInt16(retDataBaseObj);

            sqlString = "SELECT Content FROM TAS_DictionarySetting WHERE CategoryCode = '09' AND Name = 'SLV_TEMP'";
            dataBaseController.ExecuteScalar(sqlString, out retDataBaseObj);
            temperatureRegisterAddress = Convert.ToUInt16(retDataBaseObj);

            sqlString = "SELECT Content FROM TAS_DictionarySetting WHERE CategoryCode = '09' AND Name = 'SLV_PSTAT'";
            dataBaseController.ExecuteScalar(sqlString, out retDataBaseObj);
            pageStutasRegisterAddress = Convert.ToUInt16(retDataBaseObj);

            sqlString = "SELECT Content FROM TAS_DictionarySetting WHERE CategoryCode = '09' AND Name = 'MA_FRQ'";
            dataBaseController.ExecuteScalar(sqlString, out retDataBaseObj);
            frequencyRegisterAddress = Convert.ToUInt16(retDataBaseObj);
        }

        internal virtual void ExecuteStatusWordAction(StatusWordPart statusWordPartArgs)
        {
            switch (statusWordPartArgs)
            {
                case StatusWordPart.MasterRequest:
                    serialPortController.WriteSingleCoil(slaveID, masterRequestCoilAddress, true);
                    break;
                case StatusWordPart.SlaveResponse:
                    serialPortController.WriteSingleCoil(slaveID, slaveResponseCoilAddress, false);
                    break;
            }
        }

        internal virtual void ExecuteCommandWordAction(CommandWordPart commandWordPartArgs)
        {
            switch (commandWordPartArgs)
            {
                case CommandWordPart.ReadInfo:
                    serialPortController.WriteSingleRegister(slaveID, masterCommandRegisterAddress, 1);
                    break;
                case CommandWordPart.ReadData:
                    serialPortController.WriteSingleRegister(slaveID, masterCommandRegisterAddress, 2);
                    break;
                case CommandWordPart.WriteDateTime:
                    serialPortController.WriteSingleRegister(slaveID, masterCommandRegisterAddress, 11);
                    break;
                case CommandWordPart.WriteFrequency:
                    serialPortController.WriteSingleRegister(slaveID, masterCommandRegisterAddress, 12);
                    break;
            }
        }

        internal virtual void ExecuteDataWordAction(DataWordPart dataWordPartArgs,bool isWriteArgs,ushort[] writeValueArgs, out ushort[] readValueArgs)
        {
            readValueArgs = default;

            switch (dataWordPartArgs)
            {
                case DataWordPart.Timer:
                    {
                        if (isWriteArgs)
                            serialPortController.WriteMultipleRegisters(slaveID, timerRegisterAddress, writeValueArgs);
                        else
                            readValueArgs = serialPortController.ReadHoldingRegisters(slaveID, timerRegisterAddress, 6);
                        break;
                    }
                case DataWordPart.Temperature:
                    {
                        if (isWriteArgs)
                            serialPortController.WriteMultipleRegisters(slaveID, temperatureRegisterAddress, writeValueArgs);
                        else
                            readValueArgs = serialPortController.ReadHoldingRegisters(slaveID, temperatureRegisterAddress, 1);
                        break;
                    }
                case DataWordPart.PageStutas:
                    {
                        if (isWriteArgs)
                            serialPortController.WriteMultipleRegisters(slaveID, pageStutasRegisterAddress, writeValueArgs);
                        else
                            readValueArgs = serialPortController.ReadHoldingRegisters(slaveID, pageStutasRegisterAddress, 1);
                        break;
                    }
                case DataWordPart.Frequency:
                    {
                        if (isWriteArgs)
                            serialPortController.WriteMultipleRegisters(slaveID, frequencyRegisterAddress, writeValueArgs);
                        else
                            readValueArgs = serialPortController.ReadHoldingRegisters(slaveID, frequencyRegisterAddress, 1);
                        break;
                    }
            }
        }

        internal virtual bool ExecuteSlaveResponseWithRetryAction()
        {
            bool ret = false;
            currentRetryCount = 0;

            sqlString = "SELECT Content FROM TAS_DefaultSetting WHERE Name = 'RetryCount'";
            dataBaseController.ExecuteScalar(sqlString, out retDataBaseObj);
            retryCount = Convert.ToInt32(retDataBaseObj);

            sqlString = "SELECT Content FROM TAS_DefaultSetting WHERE Name = 'RetryInterval'";
            dataBaseController.ExecuteScalar(sqlString, out retDataBaseObj);
            retryIntervalTimeSpan = TimeSpan.FromSeconds(Convert.ToDouble(retDataBaseObj));

            FixedInterval retryStrategy = new FixedInterval("SlaveNotResponseStrategy", retryCount, retryIntervalTimeSpan, false);
            RetryPolicy retryPolicy = new RetryPolicy<SlaveTransientErrorDetectionStrategy>(retryStrategy);

            //TODO:同步操作中查询从机应答位会造成程序假死,需将查询操作修改为异步方式.
            retryPolicy.ExecuteAction(() =>
            {
                slaveResponseCoilBit = serialPortController.ReadCoils(slaveID, slaveResponseCoilAddress, 1).FirstOrDefault();

                if (slaveResponseCoilBit)
                    ret = true;
                else
                {
                    if (currentRetryCount < retryCount)
                    {
                        currentRetryCount++;
                        throw new SlaveNotResponseException();
                    }
                    else
                        try
                        {

                        }
                        catch (SlaveNotResponseException ex)
                        {

                        }
                }
            });

            return ret;
        }
    }
}
