using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Events;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Utility.API;
using Utility.Enum;
using Utility.Helpers;

namespace API.Helpers
{
    public class CommonHelper : ICommonHelper
    {
        private readonly AppSettingsModel _appSettings;
        private readonly IEncryptionServices _encryptionServices;
        public CommonHelper(AppSettingsModel options,
            IEncryptionServices encryptionServices)
        {
            _appSettings = options;
            _encryptionServices = encryptionServices;
        }

        #region Utilities
        public string ConvertDecimalToString(decimal value, bool isEnglish, bool? includeZero = false)
        {
            string formattedValue = string.Empty;

            var currencyEn = "KWD";
            var currencyAr = "د.ك";
            var currencyFormat = "{0:" + "0.000" + "}";

            if (includeZero == true)
            {
                if (isEnglish)
                    formattedValue = currencyEn + " " + string.Format(currencyFormat, value);
                else
                    formattedValue = string.Format(currencyFormat, value) + " " + currencyAr;
            }
            else
            {
                if (value > 0)
                {
                    if (isEnglish)
                        formattedValue = currencyEn + " " + string.Format(currencyFormat, value);
                    else
                        formattedValue = string.Format(currencyFormat, value) + " " + currencyAr;
                }
            }

            return formattedValue;
        }
        public string GetTimeAgo(DateTime dateTime, bool isEnglish)
        {
            string result;
            var timeSpan = DateTime.Now.Subtract(dateTime);

            if (timeSpan <= TimeSpan.FromSeconds(60))
            {
                result = string.Format(ResourceHelper.GetResource("SecondsAgo", isEnglish), timeSpan.Seconds);
            }
            else if (timeSpan <= TimeSpan.FromMinutes(60))
            {
                result = timeSpan.Minutes > 1 ? string.Format(ResourceHelper.GetResource("MinutesAgo", isEnglish), timeSpan.Minutes) : ResourceHelper.GetResource("AMinuteAgo", isEnglish);
            }
            else if (timeSpan <= TimeSpan.FromHours(24))
            {
                result = timeSpan.Hours > 1 ? string.Format(ResourceHelper.GetResource("HoursAgo", isEnglish), timeSpan.Hours) : ResourceHelper.GetResource("AnHourAgo", isEnglish);
            }
            else if (timeSpan <= TimeSpan.FromDays(30))
            {
                result = timeSpan.Days > 1 ? string.Format(ResourceHelper.GetResource("DaysAgo", isEnglish), timeSpan.Days) : ResourceHelper.GetResource("Yesterday", isEnglish);
            }
            else if (timeSpan <= TimeSpan.FromDays(365))
            {
                result = timeSpan.Days > 30 ? string.Format(ResourceHelper.GetResource("MonthsAgo", isEnglish), timeSpan.Days / 30) : ResourceHelper.GetResource("AMonthAgo", isEnglish);
            }
            else
            {
                result = timeSpan.Days > 365 ? string.Format(ResourceHelper.GetResource("YearsAgo", isEnglish), timeSpan.Days / 365) : ResourceHelper.GetResource("AYearAgo", isEnglish);
            }

            return result;
        }
        public string ConvertIntegerToHours(long WaitTime, bool isEnglish)
        {
            TimeSpan result = TimeSpan.FromMinutes(WaitTime);
            string fromTimeString = result.ToString("hh':'mm");
            string[] ftime = fromTimeString.Split(':');
            string WTime = ftime[0] + ' ' + ResourceHelper.GetResource("hour", isEnglish) + ':' + ftime[1] + ' ' + ResourceHelper.GetResource("minute", isEnglish);

            return WTime;
        }
        public string ConvertTimeSpanToHours(TimeSpan timeSpan, bool isEnglish)
        {
            double WaitTime = (DateTime.Now.TimeOfDay - timeSpan).TotalHours;
            // If your integer is greater than 12, then use the modulo approach, otherwise output the value (padded)
            return (WaitTime > 12) ? ((WaitTime % 12).ToString("00") + " " + (ResourceHelper.GetResource("hour", isEnglish))) : (WaitTime.ToString("00") + " " + (ResourceHelper.GetResource("minute", isEnglish)));
        }
        #endregion

        #region Logs
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
        #endregion

       
    }
}
