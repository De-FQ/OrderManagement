using Utility.Enum;

namespace API.Helpers
{
    public interface ICommonHelper
    {
        #region Utilities

        string ConvertDecimalToString(decimal value, bool isEnglish, bool? includeZero = false);
        string GetTimeAgo(DateTime dateTime, bool isEnglish);
        string ConvertIntegerToHours(long WaitTime, bool isEnglish);
        string ConvertTimeSpanToHours(TimeSpan timeSpan, bool isEnglish);
        #endregion

        #region logs
        //void LogInfo(string message, string From, Serilog.Events.LogEventLevel level = Serilog.Events.LogEventLevel.Information);
        #endregion
        // #region Token
        //string CreateAccessToken(dynamic customer, out string expiration);
        //int GetCustomerIdByToken(string token);
        //#endregion
    }
}
