using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using OfficeOpenXml;
using System.Globalization;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace MiniCrm.Core.Utility
{
    public static class Helpers
    {

        public static string ToSha512(this string input)
        {
            if (string.IsNullOrWhiteSpace(input)) throw new ArgumentNullException($"invalid value for {input}");

            var bytes = System.Text.Encoding.UTF8.GetBytes(input);

            using var hash = System.Security.Cryptography.SHA512.Create();
            var hashedInputBytes = hash.ComputeHash(bytes);

            // StringBuilder Capacity is 128, because 512 bits / 8 bits in byte * 2 symbols for byte 
            var hashedInputStringBuilder = new System.Text.StringBuilder(128);

            foreach (var b in hashedInputBytes)
                hashedInputStringBuilder.Append(b.ToString("X2"));

            return hashedInputStringBuilder.ToString().ToLower();
        }
        public static string ToJsonString(this object data)
        {
            try
            {
                if (data.GetType() == typeof(string))
                {
                    return (string)data;
                }
                // params JsonConverter[] converters
                var settings = new JsonSerializerSettings
                {
                    ContractResolver = new DefaultContractResolver { NamingStrategy = new LowercaseNamingStrategy() },
                };

                return JsonConvert.SerializeObject(data, settings);
            }
            catch { return ""; }
        }
        public static string RandomDigits(int length)
        {
            var s = string.Empty;
            for (var i = 0; i < length; i++)
                s = string.Concat(s, RandomNumberGenerator.GetInt32(10).ToString(CultureInfo.InvariantCulture));
            return s;
        }
        public static TimeSpan DateDiff(DateTimeOffset startDate, DateTimeOffset endDate) => endDate.Date.Subtract(startDate.Date);
        public static string RemoveEmptySpacesAndSpecialCharacters(string str)
        {
            str = str.Replace(" ", "", StringComparison.OrdinalIgnoreCase).Trim();
            return Regex.Replace(str, "[^a-zA-Z0-9_.]+", "", RegexOptions.Compiled);
        }
        public static (string, string) SplitBase64String(string base64)
        {
            var str = base64.Split("*|-||-|*");
            return (str[0], str[1]);
        }
        public static async Task<IEnumerable<T>> ConvertExcelData<T>(IFormFile excelFile) where T : new()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            if (excelFile == null || excelFile.Length == 0)
            {
                throw new ArgumentException("Invalid or empty Excel file.");
            }

            using (var stream = new MemoryStream())
            {
                await excelFile.CopyToAsync(stream);

                using (var package = new ExcelPackage(stream))
                {
                    var worksheet = package.Workbook.Worksheets.FirstOrDefault();

                    if (worksheet == null)
                    {
                        throw new InvalidOperationException("Excel file does not contain any worksheets.");
                    }

                    var headerRow = worksheet.Cells["A1:Z1"].Select(c => c.Text).ToList();
                    var properties = typeof(T).GetProperties();

                    int startRow = 2; // Assuming headers are in the first row
                    int endRow = worksheet.Dimension.Rows;

                    var result = new List<T>();

                    for (int rowNum = startRow; rowNum <= endRow; rowNum++)
                    {
                        var dataRow = new Dictionary<string, string>();

                        for (int col = 1; col <= headerRow.Count; col++)
                        {
                            string key = headerRow[col - 1];
                            string value = worksheet.Cells[rowNum, col].Text ?? string.Empty;

                            dataRow.Add(key, value);
                        }

                        var obj = new T();

                        foreach (var prop in properties)
                        {
                            string propName = prop.Name;

                            if (dataRow.TryGetValue(propName, out var propValue))
                            {
                                prop.SetValue(obj, value: Convert.ChangeType(propValue, prop.PropertyType));
                            }
                        }

                        result.Add(obj);
                    }

                    return result;
                }
            }
        }
    }
    public class LowercaseNamingStrategy : NamingStrategy
    {
        protected override string ResolvePropertyName(string name)
        {
            return name.ToLowerInvariant();
        }
    }
}
