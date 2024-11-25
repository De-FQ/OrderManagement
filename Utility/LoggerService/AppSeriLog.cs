using Microsoft.AspNetCore.Builder;
using Serilog;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility.LoggerService
{
    public static class AppSeriLog
    {

        /// <summary>
        /// <b>custom - "Serilog section should be defined in Admin or Api or Web, where are calling Log function</b>
        /// <br></br><b>Note: it should be added in Program.cs file </b>
        /// <para>
        ///  "Serilog": {
        ///    "Using": [ "Serilog.Sinks.Console", "Serilog.Sinks.File" ],
        ///    "WriteTo": [
        ///      { "Name": "Console" },
        ///      {
        ///        "Name": "File",
        ///        "Args": {
        ///          "path": "logs/mylogs-.txt",
        ///          "rollingInterval": "Day"
        ///        }
        ///      }
        ///    ]
        ///  },
        /// </para>
        /// <para>Dependency Injection
        /// Capture all error logs in "logs/?.txt" throughout the api
        /// The Log class object is available in api and its used inside ConfigureExceptionHandler method</para>
        /// </summary>
        /// <param name="app">dd</param>
        public static void Configure(this WebApplicationBuilder builder)
        {
            #region All Exceptions configuration

            Log.Logger = new Serilog.LoggerConfiguration().ReadFrom.Configuration(builder.Configuration).CreateLogger();

            //    .MinimumLevel.Information()
            //    .WriteTo.Console()
            //    .WriteTo.File("logs/applog.txt", rollingInterval: RollingInterval.Day)
            //    .CreateLogger();
            //var logger = app.Services.GetRequiredService<ILoggerManager>();
            //app.ConfigureExceptionHandler(logger);

            #endregion
        }
        /// <summary>
        /// <b>LogInfo static method can be called from any Admin,API and Web project directly</b>
        /// </summary>
        /// <param name="message"></param>
        /// <param name="From"></param>
        /// <param name="level"></param>
        public static void LogInfo(string message, string From, Serilog.Events.LogEventLevel level = Serilog.Events.LogEventLevel.Information)
        {
            string data = From + ":" + message;
            switch (level)
            {
                case LogEventLevel.Information:
                    Log.Information(data);
                    break;
                case LogEventLevel.Warning:
                    Log.Warning(data);
                    break;
                case LogEventLevel.Debug:
                    Log.Debug(data);
                    break;
                case LogEventLevel.Fatal:
                    Log.Fatal(data);
                    break;
                case LogEventLevel.Error:
                    Log.Error(data);
                    break;
                default:
                    break;
            }

        }
    }
}
