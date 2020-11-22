using Microsoft.AspNetCore.Blazor.Hosting;
using Microsoft.Extensions.Primitives;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EncoderDecoder.Utilities
{

    public static class ExtentionMethods
    {
       public static ConcurrentDictionary<TKey, TValue> ToConcurrentDictionary<TKey, TValue>(this Dictionary<TKey, TValue>? dict)
        {
            if (dict == null)
                return new ConcurrentDictionary<TKey, TValue>();
            return new ConcurrentDictionary<TKey, TValue>(dict);

        } 
        public static Dictionary<string, string> ToReleventInfo(this Exception e)
        {
            Dictionary<string, string> error;
            try
            {
                error = new Dictionary<string, string> {
                                {"Type", e.GetType().ToString()},
                                {"Message", e.Message},
                                {"StackTrace", e.StackTrace??string.Empty},
                                {"InnerException", e.InnerException?.Message??string.Empty}
                };


                foreach (DictionaryEntry? data in e.Data)
                    error.Add(data?.Key?.ToString() ?? string.Empty, data?.Value?.ToString() ?? string.Empty);
                return error;
            }
            catch (Exception ex)
            {
                error = new Dictionary<string, string> {
                                {"Message", ex.Message},
                                {"InnerMessage", e.Message}};
            }
            return error;
        }
        private static readonly IContractResolver contractResolverCC = new CamelCasePropertyNamesContractResolver();
        public static string SerializeObject(this object myObject, bool isFormattingIntended = true, bool isCamelCase = false) => JsonConvert.SerializeObject(myObject, isFormattingIntended ? Formatting.Indented : Formatting.None, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore, NullValueHandling = NullValueHandling.Ignore, ContractResolver = isCamelCase ? contractResolverCC : default });


        public static decimal TruncateAfter(this decimal d, byte decimals)
        {
            decimal r = Math.Round(d, decimals);

            if (d > 0 && r > d)
            {
                return r - new decimal(1, 0, 0, false, decimals);
            }
            else if (d < 0 && r < d)
            {
                return r + new decimal(1, 0, 0, false, decimals);
            }
            return r;
        }

        public static long ToLong(this double d) => Convert.ToInt64(Math.Truncate(d));


        #region DateTime
        public static readonly DateTime MinDateTimeJS = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc); 

        public static long ToSec(this DateTime dt) => (dt - MinDateTimeJS).TotalSeconds.ToLong();

        public static long ToSecIgnoreTime(this DateTime dt) => (dt.Date - MinDateTimeJS).TotalSeconds.ToLong();

        public static long ToMilliSec(this DateTime dt) => (dt - MinDateTimeJS).TotalMilliseconds.ToLong();

        public static long ToMilliSecIgnoreTime(this DateTime dt) => (dt.Date - MinDateTimeJS).TotalMilliseconds.ToLong();

        public static long ToTicks(this DateTime dt) => (dt.Ticks - MinDateTimeJS.Ticks);

        public static long ToTicksIgnoreTime(this DateTime dt) => (dt.Date.Ticks - MinDateTimeJS.Ticks);


        public static string ToTicksHexString(this DateTime dt) => dt.ToTicks().ToString("X");
        public static string ToTicksHexStringIgnoreTime(this DateTime dt) => dt.Date.ToTicks().ToString("X");



        public static DateTime MilliSecToDateTime(this long millisec) => MinDateTimeJS.AddMilliseconds(millisec);
        public static DateTime SecToDateTime(this long sec) => MinDateTimeJS.AddSeconds(sec);
        public static DateTime TicksToDateTime(this long ticks) => MinDateTimeJS.AddTicks(ticks);

        public static DateTime TicksHexStringToDateTime(this string ticksHexString) => TicksHexStringToTicks(ticksHexString).TicksToDateTime();
        public static long TicksHexStringToTicks(string ticksHexString)
        {
            try
            {
                return Convert.ToInt64(ticksHexString, 16);
            }
            catch// (Exception ex)
            {
                return 0L;
            }
        }
        #endregion



        #region HMAC
        public static string HMACSHA512(this SortedList<string, string> parms, string secret)
        {
            string post_data = "";
            foreach (KeyValuePair<string, string> parm in parms)
            {
                if (post_data.Length > 0) { post_data += "&"; }
                post_data += parm.Key + "=" + Uri.EscapeDataString(parm.Value ?? "");
            }
            byte[] keyBytes = Encoding.UTF8.GetBytes(secret);
            byte[] postBytes = Encoding.UTF8.GetBytes(post_data);
            var hmacsha512 = new HMACSHA512(keyBytes);
            string hmac = BitConverter.ToString(hmacsha512.ComputeHash(postBytes)).Replace("-", string.Empty);
            return hmac;
        }

        public static string? ToHMACSHA512(this string? input, string? secret)
        {
            if (string.IsNullOrWhiteSpace(input) || string.IsNullOrWhiteSpace(secret))
                return input;

            byte[] keyBytes = Encoding.UTF8.GetBytes(secret);
            byte[] postBytes = Encoding.UTF8.GetBytes(input);
            var hmacsha512 = new HMACSHA512(keyBytes);
            string hmac = BitConverter.ToString(hmacsha512.ComputeHash(postBytes)).Replace("-", string.Empty);
            return hmac;
        }
        public static SortedList<string, string> ToDictionary(this Dictionary<string, StringValues> queriesDict)
        {
            var dict = new SortedList<string, string>();
            foreach (var k in queriesDict.Keys)
            {
                dict.Add(k, queriesDict[k].ToString());
            }
            return dict;
        }
        #endregion

        public static StringContent AsJson(this object o) => new StringContent(JsonConvert.SerializeObject(o), Encoding.UTF8, "application/json");
        //public static string? GetIP(this HttpContext httpContext) => httpContext.Request.HttpContext.Connection.RemoteIpAddress?.MapToIPv4()?.ToString();


        //public static string GetIP(this HttpContext httpContext)
        //{
        //    if (httpContext.Request.Headers.TryGetValue("CF-Connecting-IP", out var IP) && System.Net.IPAddress.TryParse(IP, out var IPAddress))
        //    {
        //        return IPAddress.MapToIPv4()?.ToString() ?? string.Empty;
        //    }
        //    else if (httpContext.Request.Headers.TryGetValue("X-Forwarded-For", out IP) && System.Net.IPAddress.TryParse(IP, out IPAddress))
        //    {
        //        return IPAddress.MapToIPv4()?.ToString() ?? string.Empty;
        //    }
        //    else
        //    {
        //        return httpContext.Request.HttpContext.Connection.RemoteIpAddress.MapToIPv4()?.ToString() ?? string.Empty;
        //    }
        //}

        public static Dictionary<int, string> ToDictionary(this Enum enumValue) => Enum.GetValues(enumValue.GetType())
                .Cast<Enum>()
                .ToDictionary(t => (int)(object)t, t => t.ToString());

        //public static Dictionary<T, string> ToDictionary<T>() where T : struct => Enum.GetValues(typeof(T)).Cast<T>().ToDictionary(e => e, e => e.ToString());


        #region Hashing && Encryption

        public static string ToSHA256(this string TextToBeEncrypted, bool trimLR = true)
        {
            if (string.IsNullOrWhiteSpace(TextToBeEncrypted))
                return TextToBeEncrypted;

            var crypt = new System.Security.Cryptography.SHA256Managed();
            var hash = new System.Text.StringBuilder();
            byte[] crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes(trimLR ? TextToBeEncrypted.Trim() : TextToBeEncrypted));
            foreach (byte theByte in crypto)
            {
                hash.Append(theByte.ToString("x2"));
            }
            return hash.ToString();
        }

        private const string EncryptionKey = "EncryptionKey";

        public static string EncryptText(this string TextToBeEncrypted, string EncryptionKey = EncryptionKey)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(TextToBeEncrypted) || string.IsNullOrWhiteSpace(EncryptionKey))
                    return TextToBeEncrypted;

                byte[] clearBytes = Encoding.Unicode.GetBytes(TextToBeEncrypted);
                using (Aes encryptor = Aes.Create())
                {
                    Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                    encryptor.Key = pdb.GetBytes(32);
                    encryptor.IV = pdb.GetBytes(16);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(clearBytes, 0, clearBytes.Length);
                        }
                        TextToBeEncrypted = Convert.ToBase64String(ms.ToArray());
                        return TextToBeEncrypted;
                    }
                }
            }
            catch //(Exception ex)
            {
                return TextToBeEncrypted;
            }
        }

        public static string DecryptText(this string TextToBeDecrypted, string EncryptionKey = EncryptionKey)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(TextToBeDecrypted) || string.IsNullOrWhiteSpace(EncryptionKey))
                    return TextToBeDecrypted;

                byte[] cipherBytes = Convert.FromBase64String(TextToBeDecrypted);
                using (Aes encryptor = Aes.Create())
                {
                    Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                    encryptor.Key = pdb.GetBytes(32);
                    encryptor.IV = pdb.GetBytes(16);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(cipherBytes, 0, cipherBytes.Length);
                        }
                        TextToBeDecrypted = Encoding.Unicode.GetString(ms.ToArray());
                        return TextToBeDecrypted;
                    }
                }

            }
            catch //(Exception ex)
            {
                return TextToBeDecrypted;
            }
        }


        /// <summary> 
        /// length >= 8
        /// Atleast 1 Number
        /// Atleast 1 UpperCase Alphabate
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static bool isPasswordStrongEnough(this string password) => !string.IsNullOrWhiteSpace(password) && password.Length >= 8 && password.ToCharArray().Any(x => x >= 65 && x <= 90) && password.ToCharArray().Any(x => x >= 48 && x <= 57);


        public static byte[] StringToByteArray(this string hex) => Enumerable.Range(0, hex.Length).Where(x => x % 2 == 0).Select(x => Convert.ToByte(hex.Substring(x, 2), 16)).ToArray();

        #endregion


        public static Task ParallelForEachAsync<T>(this IEnumerable<T> source, Func<T, Task> funcBody, int maxDoP = 4)
        {
            async Task AwaitPartition(IEnumerator<T> partition)
            {
                using (partition)
                {
                    while (partition.MoveNext())
                    { await funcBody(partition.Current); }
                }
            }

            return Task.WhenAll(
                System.Collections.Concurrent.Partitioner
                    .Create(source)
                    .GetPartitions(maxDoP)
                    .AsParallel()
                    .Select(p => AwaitPartition(p)));
        }
 
        public static string ToMD5Hash(this string? str)
        {
            if (string.IsNullOrWhiteSpace(str))
                return string.Empty;

            return Encoding.ASCII.GetBytes(str).ToMD5Hash();
        }

        public static string ToMD5Hash(this byte[]? bytes)
        {
            if (bytes == null || bytes.Length == 0)
                return string.Empty;

            using (var md5 = MD5.Create())
            {
                return string.Join(string.Empty, md5.ComputeHash(bytes).Select(x => x.ToString("X2")));
            }
        }
        public static decimal GetMedian(this IEnumerable<decimal> source)
        {

            if (source.Count() == 0)
            {
                return 0M;
            }
            // Create a copy of the input, and sort the copy
            decimal[] temp = source.ToArray();
            Array.Sort(temp);
            int count = temp.Length;

            if (count % 2 == 0)
            {
                // count is even, average two middle elements
                decimal a = temp[count / 2 - 1];
                decimal b = temp[count / 2];
                return (a + b) / 2m;
            }
            else
            {
                // count is odd, return the middle element
                return temp[count / 2];
            }
        }
        public static void InitializeBlazoredLocalization(this WebAssemblyHost app)
        {
            var jsRuntime = app.Services.GetService(typeof(IJSRuntime));
            var browserLocale = ((IJSInProcessRuntime)jsRuntime).Invoke<string>("blazoredLocalisation.getBrowserLocale");
            var culture = new CultureInfo(browserLocale);

            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;
        }

    }


}
