using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;

namespace TAS.Services
{
    /// <summary>
    /// 通过加载当前程序环境配置文件管理程序相关路径.
    /// </summary>
    [HasSelfValidation]
    public class ApplictionPathController : IApplictionPathController
    {
        string defaultApplictionConfigFilePath;
        Configuration configuration;
        Dictionary<string, string> applictionPathShub;

        /// <summary>
        /// 程序运行目录
        /// </summary>
        public string ApplictionCatalogue { get; set; }

        /// <summary>
        /// 本地数据库文件路径
        /// </summary>
        public string NativeDataBaseFilePath { get; set; }

        /// <summary>
        /// 日志文件路径
        /// </summary>
        public string TextLogFilePath { get; set; }

        public ApplictionPathController()
        {
            CreateDefaultPath();
        }

        internal void CreateDefaultPath()
        {
            applictionPathShub = new Dictionary<string, string>();

            defaultApplictionConfigFilePath = AppDomain.CurrentDomain.BaseDirectory + AppDomain.CurrentDomain.FriendlyName.Replace("exe", "config");
            ApplictionCatalogue = AppDomain.CurrentDomain.BaseDirectory;
            NativeDataBaseFilePath = AppDomain.CurrentDomain.BaseDirectory + AppDomain.CurrentDomain.FriendlyName.Replace("exe", "db");
            TextLogFilePath = AppDomain.CurrentDomain.BaseDirectory + AppDomain.CurrentDomain.FriendlyName.Replace("exe", "txt");

            applictionPathShub.Add("applictionCatalogue", ApplictionCatalogue);
            applictionPathShub.Add("nativeDataBaseFilePath", NativeDataBaseFilePath);
            applictionPathShub.Add("textLogFilePath", TextLogFilePath);
        }

        /// <summary>
        /// 加载配置数据
        /// </summary>
        public void Initialize()
        {
            //先查找config文件配置，替换默认值。
            ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap();
            fileMap.ExeConfigFilename = defaultApplictionConfigFilePath;
            configuration = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
            KeyValueConfigurationCollection keyValueConfigurationCollection = configuration.AppSettings.Settings;

            foreach (KeyValuePair<string, string> applictionPath in applictionPathShub)
            {
                if (keyValueConfigurationCollection.AllKeys.Contains(applictionPath.Key))
                    applictionPathShub[applictionPath.Key] = keyValueConfigurationCollection[applictionPath.Key].Value;
            }
        }

        [SelfValidation]
        public void Validate(ValidationResults results)
        {
            if (!File.Exists(defaultApplictionConfigFilePath))
                results.AddResult(new ValidationResult("当前程序环境缺少配置文件,请检查配置并重启程序!", this, "defaultApplictionConfigFilePath", "", null));
            else
            {
                if (!File.Exists(NativeDataBaseFilePath))
                    results.AddResult(new ValidationResult("当前程序环境缺少本地数据库文件,请检查配置并重启程序!", this, "NativeDataBaseFilePath", "", null));
            }
        }

        /// <summary>
        /// 将属性内容持久化
        /// </summary>
        public void Save()
        {
            applictionPathShub["applictionCatalogue"] = ApplictionCatalogue;
            applictionPathShub["nativeDataBaseFilePath"] = NativeDataBaseFilePath;
            applictionPathShub["textLogFilePath"] = TextLogFilePath;

            foreach (KeyValuePair<string, string> applictionPath in applictionPathShub)
            {
                if (configuration.AppSettings.Settings.AllKeys.Contains(applictionPath.Key))
                    configuration.AppSettings.Settings[applictionPath.Key].Value = applictionPath.Value;
                else
                    configuration.AppSettings.Settings.Add(applictionPath.Key, applictionPath.Value);
            }

            configuration.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }
    }
}