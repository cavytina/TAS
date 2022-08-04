using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Prism.Ioc;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;
using Microsoft.Practices.EnterpriseLibrary.Logging.Filters;

namespace TAS.Services
{
    [HasSelfValidation]
    public class TextLogController : ILogController
    {
        IApplictionController applictionController;
        IApplictionPathController applictionPathController;

        string textLogFilePath;  
        LogWriter logWriter;

        public TextLogController(IContainerProvider containerProviderArgs)
        {
            applictionController= containerProviderArgs.Resolve<IApplictionController>();
            applictionPathController = containerProviderArgs.Resolve<IApplictionPathController>();
            CreateDefaultTextLogPath();
        }

        internal void CreateDefaultTextLogPath()
        {
            textLogFilePath = applictionPathController.TextLogFilePath;
        }

        public void Initialize()
        {
            TextFormatter textFormatter = new TextFormatter("{timestamp(local)}{tab}{message}");
            FlatFileTraceListener flatFileTraceListener = new FlatFileTraceListener(textLogFilePath, null, null, textFormatter);
            List<TraceListener> traceListeners = new List<TraceListener>();
            traceListeners.Add(flatFileTraceListener);

            LogEnabledFilter logEnabledFilter = new LogEnabledFilter("enabledFilter", true);
            List<ILogFilter> logFilters = new List<ILogFilter>();
            logFilters.Add(logEnabledFilter);

            LogSource generalLogSource = new LogSource("General", traceListeners, SourceLevels.All);
            LogSource ErrorsLogSource = new LogSource("Errors", traceListeners, SourceLevels.Error);
            List<LogSource> logSources = new List<LogSource>();
            logSources.Add(generalLogSource);

            logWriter = new LogWriter(logFilters, logSources, ErrorsLogSource, "General");
        }

        [SelfValidation]
        public void Validate(ValidationResults results)
        {
            if (string.IsNullOrEmpty(textLogFilePath))
                results.AddResult(new ValidationResult("默认日志文件不能为空!", this, "defaultTextLogFilePath", "", null));
        }

        public void WriteLog(string MessageArgs)
        {
            if (applictionController.ApplictionValidationResults.IsValid)
                logWriter.Write(MessageArgs);
        }
    }
}