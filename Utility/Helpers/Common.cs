using System.Security.Cryptography;

namespace Utility.Helpers
{
    public static class Common
    {
        public class TokenInfo
        {
            public string FullName { get; set; }
            public int RoleId { get; set; }
            public int UserId { get; set; }
            public string ErrorMessage { get; set; }
        }
        public static string GenerateRandomText()
        {
            // Creating a short version of the provided url
            var random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            var randomStr = new string(Enumerable.Repeat(chars, 8).Select(x => x[random.Next(x.Length)]).ToArray());
            return randomStr;
        }

        public static string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        public static Guid GenrateActivationKey()
        {
            return Guid.NewGuid();
        }
        public static string GenerateRandomNo(int minVal = 10000000, int maxVal = 99999999)
        {
            Random random = new();
            return random.Next(minVal, maxVal).ToString();
        }
        public static string RandomString(int size, bool isLowerCase = false, bool isAlphaNumeric = false)
        {
            Random random = new();
            string chars = Constants.Common.AlphaChars;

            if (isAlphaNumeric)
            {
                chars = Constants.Common.AlphaNumericChars;
            }

            string randomString = new string(Enumerable.Repeat(chars, size)
                                 .Select(s => s[random.Next(s.Length)]).ToArray());

            randomString = isLowerCase ? randomString.ToLower() : randomString;

            return randomString;
        }
        
        public static string ConvertHtmlToPlainText(string source)
        {
            if (source != null)
            {
                char[] array = new char[source.Length];
                int arrayIndex = 0;
                bool inside = false;

                for (int i = 0; i < source.Length; i++)
                {
                    char let = source[i];
                    if (let == '<')
                    {
                        inside = true;
                        continue;
                    }
                    if (let == '>')
                    {
                        inside = false;
                        continue;
                    }
                    if (!inside)
                    {
                        array[arrayIndex] = let;
                        arrayIndex++;
                    }
                }
                return new string(array, 0, arrayIndex);
            }
            return string.Empty;
        }
        public static string StringLimit(string inputText, int limit, bool includeDots = true)
        {
            string newText = string.Empty;

            if (limit > inputText.Length)
                newText = inputText;
            else
            {
                newText = inputText.Substring(0, limit);

                if (includeDots)
                    newText += "...";
            }

            return newText;
        }
        public static int? ConvertTextToIntOptional(string text)
        {

            if (int.TryParse(text, out int result))
            {
                return result;
            }
            else
            {
                return null;
            }
        }
        public static long? ConvertTextToLongOptional(string text)
        {

            if (long.TryParse(text, out long result))
            {
                return result;
            }
            else
            {
                return null;
            }
        }
        public static int ConvertTextToInt(string text)
        {

            if (int.TryParse(text, out int result))
            {
                return result;
            }
            else
            {
                return 0;
            }

        }
        public static bool? ConvertTextToBoolean(string text)
        {

            if (bool.TryParse(text, out bool result))
            {
                return result;
            }
            else
            {
                return null;
            }
        }
        public static bool  ConvertTextToBooleanNew(string text)
        {

            if (bool.TryParse(text, out bool result))
            {
                return result;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Convert calendar date [day/mon/year] to System DateTime format
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static DateTime? ConvertTextToDate(string text)
        {
            try
            {
                if (string.IsNullOrEmpty(text))
                {
                    return null;
                }

                var arrDate = text.Split("/");
                int.TryParse(arrDate[0], out int day);
                int.TryParse(arrDate[1], out int month);
                int.TryParse(arrDate[2], out int year);
                var newDate = new DateTime(year, month, day);
                return newDate;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Convert calendar date [year/mon/day] to System DateTime format
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static DateTime? ConvertYYYYMMDDTextToDate(string text)
        {
            try
            {
                if (string.IsNullOrEmpty(text))
                {
                    return null;
                }

                var arrDate = text.Split("/");
                int.TryParse(arrDate[0], out int year);
                int.TryParse(arrDate[1], out int month);
                int.TryParse(arrDate[2], out int day);
                var newDate = new DateTime(year, month, day);
                return newDate;
            }
            catch
            {
                return null;
            }
        }
        public static DateTime? ConvertTextToDateYYMMDD(string text)
        {
            try
            {
                if (string.IsNullOrEmpty(text))
                {
                    return null;
                }

                var arrDate = text.Split("/");
                int.TryParse(arrDate[0], out int year);
                int.TryParse(arrDate[1], out int month);
                int.TryParse(arrDate[2], out int day);
                var newDate = new DateTime(year, month, day);
                return newDate;
            }
            catch
            {
                return null;
            }
        }

        public static DateOnly? ConvertTextToDateOnlyYYMMDD(string text)
        {
            try
            {
                if (string.IsNullOrEmpty(text))
                {
                    return null;
                }

                var arrDate = text.Split("/");
                int.TryParse(arrDate[0], out int year);
                int.TryParse(arrDate[1], out int month);
                int.TryParse(arrDate[2], out int day);
                var newDate = new DateOnly(year, month, day);
                return newDate;
            }
            catch
            {
                return null;
            }
        }
        public static Guid? ConvertTextToGuidOptional(string text)
        {

            if (Guid.TryParse(text, out Guid result))
            {
                return result;
            }
            else
            {
                return null;
            }
        }
        public static string ArabicNumeraltoEnglish(string Arabictext)
        {
            string Englishtext = "";
            for (int i = 0; i < Arabictext.Length; i++)
            {
                if (Char.IsDigit(Arabictext[i]))
                {
                    Englishtext += char.GetNumericValue(Arabictext, i);
                }
                else
                {
                    Englishtext += Arabictext[i].ToString();
                }
            }
            return Englishtext;
        }
        public static string GetRandomNumber()
        {
            Random ran = new Random();

            String b = "abcdefghijklmnopqrstuvwxyz0123456789";
            String sc = "!@#$%^&*~";

            int length = 32;

            String random = "";

            for (int i = 0; i < length; i++)
            {
                int a = ran.Next(b.Length); //string.Lenght gets the size of string
                random = random + b.ElementAt(a);
            }
            for (int j = 0; j < 2; j++)
            {
                int sz = ran.Next(sc.Length);
                random = random + sc.ElementAt(sz);
            }

            return random;
        }     
        public static void SaveExceptionError(string errorMessage)
        {
            try
            {
                var line = Environment.NewLine;
                var filepath = Path.Combine(Directory.GetCurrentDirectory(), "Logs\\Exception");

                if (!Directory.Exists(filepath))
                {
                    Directory.CreateDirectory(filepath);
                }

                filepath = filepath + @"\Log_" + DateTime.Now.Year + DateTime.Now.Month + DateTime.Now.Day + ".txt";
                if (!File.Exists(filepath))
                {
                    File.Create(filepath).Dispose();
                }

                using (StreamWriter sw = File.AppendText(filepath))
                {
                    sw.WriteLine("-----------Exception Details on " + " " + DateTime.Now.ToString() + "-----------------");
                    sw.WriteLine("-------------------------------------------------------------------------------------");
                    sw.WriteLine(errorMessage);
                    sw.WriteLine("--------------------------------*End*------------------------------------------");
                    sw.WriteLine(line);
                    sw.Flush();
                    sw.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
        public static void SaveExceptionError(Exception ex)
        {
            try
            {
                var line = Environment.NewLine;
                var filepath = Path.Combine(Directory.GetCurrentDirectory(), "Logs\\Exception");

                if (!Directory.Exists(filepath))
                {
                    Directory.CreateDirectory(filepath);
                }

                filepath = filepath + @"\Log_" + DateTime.Now.Year + DateTime.Now.Month + DateTime.Now.Day + ".txt";
                if (!File.Exists(filepath))
                {
                    File.Create(filepath).Dispose();
                }

                string errorMessage = ex.Message;

                var innerException = ex.InnerException;
                if (innerException != null)
                {
                    do
                    {
                        errorMessage = errorMessage + ", " + innerException.Message;
                        innerException = innerException.InnerException;
                    }
                    while (innerException != null);
                }

                using (StreamWriter sw = File.AppendText(filepath))
                {
                    sw.WriteLine("-----------Exception Details on " + " " + DateTime.Now.ToString() + "-----------------");
                    sw.WriteLine("-------------------------------------------------------------------------------------");
                    sw.WriteLine(errorMessage);
                    sw.WriteLine("--------------------------------*End*------------------------------------------");
                    sw.WriteLine(line);
                    sw.Flush();
                    sw.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
        public static void SaveIPDetails(string message)
        {
            try
            {
                var line = Environment.NewLine;
                var filepath = Path.Combine(Directory.GetCurrentDirectory(), "Logs\\IP");

                if (!Directory.Exists(filepath))
                {
                    Directory.CreateDirectory(filepath);
                }

                filepath = filepath + @"\Log_" + DateTime.Now.Year + DateTime.Now.Month + DateTime.Now.Day + ".txt";
                if (!File.Exists(filepath))
                {
                    File.Create(filepath).Dispose();
                }

                using (StreamWriter sw = File.AppendText(filepath))
                {
                    sw.WriteLine("-----------IP Details on " + " " + DateTime.Now.ToString() + "-----------------");
                    sw.WriteLine(message);
                    sw.WriteLine("--------------------------------*End*------------------------------------------");
                    sw.WriteLine(line);
                    sw.Flush();
                    sw.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
        public static void SaveMasterCardRequestResponseLog(string log)
        {
            try
            {
                var line = Environment.NewLine;
                var filepath = Path.Combine(Directory.GetCurrentDirectory(), "Logs\\MasterCardRequestResponse");

                if (!Directory.Exists(filepath))
                {
                    Directory.CreateDirectory(filepath);
                }

                filepath = filepath + @"\Log_" + DateTime.Now.Year + DateTime.Now.Month + DateTime.Now.Day + ".txt";
                if (!File.Exists(filepath))
                {
                    File.Create(filepath).Dispose();
                }

                using (StreamWriter sw = File.AppendText(filepath))
                {
                    sw.WriteLine("-----------Log Details on " + " " + DateTime.Now.ToString() + "-----------------");
                    sw.WriteLine("-------------------------------------------------------------------------------------");
                    sw.WriteLine(log);
                    sw.WriteLine("--------------------------------*End*------------------------------------------");
                    sw.WriteLine(line);
                    sw.Flush();
                    sw.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
