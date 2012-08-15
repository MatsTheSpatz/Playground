using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;

namespace RecipeWebRole
{
    public class WebRole : RoleEntryPoint
    {
        private readonly string wadConnectionSettingName = "Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString";

        public override bool OnStart()
        {
            // For information on handling configuration changes
            // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.

            DiagnosticMonitorConfiguration config = DiagnosticMonitor.GetDefaultInitialConfiguration();

            //var perfCounterConfig = new PerformanceCounterConfiguration()
            //                            {
            //                                CounterSpecifier = @"\Memory\Available MBytes",
            //                                SampleRate = TimeSpan.FromSeconds(5.0)
            //                            };
            //config.PerformanceCounters.DataSources.Add(perfCounterConfig);
            //config.PerformanceCounters.ScheduledTransferPeriod = TimeSpan.FromMinutes(1.0);

            config.Logs.ScheduledTransferPeriod = TimeSpan.FromMinutes(1.0);  // this is the minimum!
            config.Logs.ScheduledTransferLogLevelFilter = LogLevel.Error;

            //Trace.AutoFlush = true;
            //Trace.Listeners.Add(new DiagnosticMonitorTraceListener());

            DiagnosticMonitor.Start(wadConnectionSettingName, config);
            return base.OnStart();
        }
    }
}
