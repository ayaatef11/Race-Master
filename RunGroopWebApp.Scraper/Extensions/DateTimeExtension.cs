
namespace RunGroopWebApp.Scraper.Extensions
{
    public static class DateTimeExtension
    {
        public static DateTime? ToDate(this string dateTimeStr, params string[] dateFmt)
        {
            const DateTimeStyles style = DateTimeStyles.AllowWhiteSpaces;
            if (dateFmt == null)
            {
                var dateInfo = System.Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat;
                dateFmt = dateInfo.GetAllDateTimePatterns();
            }
            var result = DateTime.TryParseExact(dateTimeStr, dateFmt, CultureInfo.InvariantCulture,
                           style, out var dt) ? dt : null as DateTime?;
            return result;
        }
    }
}
