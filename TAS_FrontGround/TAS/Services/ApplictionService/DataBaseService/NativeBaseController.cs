using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Prism.Ioc;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using System.Data.SQLite;
using Dapper;

namespace TAS.Services
{
    [HasSelfValidation]
    public class NativeBaseController : IDataBaseController
    {
        IApplictionController applictionController;
        IApplictionPathController applictionPathController;
        ILogController logController;

        SQLiteConnection SQLiteConnection;

        public NativeBaseController(IContainerProvider containerProviderArgs)
        {
            applictionController = containerProviderArgs.Resolve<IApplictionController>();
            applictionPathController = containerProviderArgs.Resolve<IApplictionPathController>();
            logController = containerProviderArgs.Resolve<ILogController>();
        }

        public void Initialize()
        {
            SQLiteConnection = new SQLiteConnection();
            string nativeDataBaseFilePath = applictionPathController.NativeDataBaseFilePath;
            SQLiteConnection.ConnectionString = "Data Source= " + nativeDataBaseFilePath;
        }

        [SelfValidation]
        public void Validate(ValidationResults results)
        {
            SQLiteConnection.Open();
            if (SQLiteConnection.State != ConnectionState.Open)
                results.AddResult(new ValidationResult("当前本地数据库文件访问失败!", this, "sqliteConnection", "", null));
            else
                SQLiteConnection.Close();
        }

        public bool Query<T>(string queryStingArgs, out List<T> tHub)
        {
            bool ret = false;
            tHub = default(List<T>);

            if (applictionController.ApplictionValidationResults.IsValid)
            {
                CommandDefinition commandDefinition = new CommandDefinition(queryStingArgs);
                tHub = SqlMapper.Query<T>(SQLiteConnection, commandDefinition).AsList();
                logController.WriteLog(queryStingArgs);
                ret = true;
            }

            return ret;
        }

        public bool Execute(string execStingArgs)
        {
            bool ret = false;

            if (applictionController.ApplictionValidationResults.IsValid)
            {
                CommandDefinition commandDefinition = new CommandDefinition(execStingArgs);
                int retVal = SqlMapper.Execute(SQLiteConnection, commandDefinition);
                if (retVal!=0)
                {
                    logController.WriteLog(execStingArgs);
                    ret = true;
                }
            }

            return ret;
        }

        public bool ExecuteScalar(string execStingArgs,out object objArgs)
        {
            bool ret = false;
            objArgs = default(object);

            if (applictionController.ApplictionValidationResults.IsValid)
            {
                CommandDefinition commandDefinition = new CommandDefinition(execStingArgs);
                objArgs = SqlMapper.ExecuteScalar(SQLiteConnection, commandDefinition);
                logController.WriteLog(execStingArgs);
                ret = true;
            }

            return ret;
        }
    }
}