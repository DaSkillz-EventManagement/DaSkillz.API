using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Helper
{
    public static class DateTimeHelper
    {
        public static DateTime dateTime = new DateTime(1970, 1, 1, 7, 0, 0, DateTimeKind.Utc);
        public static long epochTime = dateTime.Ticks;
        public static long ToJsDateType(DateTime dateTime)
        {
            return (dateTime.Ticks - epochTime) / 10000;
        }
        public static DateTime GetDateTimeNow()
        {
            // Lấy múi giờ hiện tại của server
            DateTime serverTime = DateTime.UtcNow;

            // Tìm thông tin múi giờ của Việt Nam
            TimeZoneInfo vietnamTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");

            // Chuyển múi giờ của server sang múi giờ của Việt Nam
            DateTime vietnamTime = TimeZoneInfo.ConvertTimeFromUtc(serverTime, vietnamTimeZone);

            return vietnamTime;
        }

        public static string GetDateNow()
        {
            // Lấy múi giờ hiện tại của server
            DateTime serverTime = DateTime.UtcNow;

            // Tìm thông tin múi giờ của Việt Nam
            TimeZoneInfo vietnamTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");

            // Chuyển múi giờ của server sang múi giờ của Việt Nam
            DateTime vietnamTime = TimeZoneInfo.ConvertTimeFromUtc(serverTime, vietnamTimeZone);

            return vietnamTime.ToString("dd/MM/yyyy");
        }
        public static bool ValidateStartTimeAndEndTime(long startTime, long endTime)
        {
            DateTime check = DateTime.Now.AddHours(12);
            long now = ToJsDateType(check);
            return startTime > now && endTime > startTime;
        }
        public static DateTime ToDateTime(long tick)
        {
            DateTimeOffset value = DateTimeOffset.FromUnixTimeMilliseconds(tick);
            return value.DateTime;
        }
        public static string GetCronExpression(DateTime dateTime)
        {
            // The Cron expression format is: "seconds minutes hours dayOfMonth month dayOfWeek year"
            // Example: "0 0 12 * * ? *" (every day at 12:00 PM)
            return $"{dateTime.Second} {dateTime.Minute} {dateTime.Hour} {dateTime.Day} {dateTime.Month} ? {dateTime.Year}";
        }

        public static string ToOnlyDate(this DateTime dateTime)
        {
            return dateTime.ToString("dd/MM/yyyy");
        }

        public static string GetTimeRange(DateTime startDate, DateTime endDate)
        {
            return startDate.ToString("HH:mm:tt") + " - " + endDate.ToString("HH:mm:tt");
        }

        public static bool IsTheSameDate(this DateTime dateTime, DateTime compare)
        {
            return dateTime.ToString("dd/MM/yyyy").Equals(compare.ToString("dd/MM/yyyy"));
        }

    }
}
