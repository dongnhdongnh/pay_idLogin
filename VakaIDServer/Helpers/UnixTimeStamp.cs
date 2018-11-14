using System;

namespace VakaxaIDServer.Helpers
{
    public static class UnixTimestamp
    {
        public static long ToUnixTimestamp(DateTime date)
        {
            return ((DateTimeOffset) date).ToUnixTimeSeconds();
        }

        public static DateTime FromUnixTimestamp(long unixTimestamp)
        {
            return DateTimeOffset.FromUnixTimeSeconds(unixTimestamp).DateTime;
        }

        public static long ConvertToMilliseconds(long unixTimestamp)
        {
            return DateTimeOffset.FromUnixTimeSeconds(unixTimestamp).ToUnixTimeMilliseconds();
        }


        public static string Iso8061DateFromUnixTimestamp(long unixTimestamp)
        {
            return DateTimeOffset.FromUnixTimeSeconds(unixTimestamp).DateTime.ToString("yyyy-MM-dd HH:mm:ss");
        }

        public static string DateFromUnixTimestamp(long unixTimestamp)
        {
            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified);
            dtDateTime = dtDateTime.AddSeconds(unixTimestamp).ToLocalTime();
            return dtDateTime.ToString("MM/dd/yyyy");
        }

        public static long GetCurrentEpoch()
        {
            return ToUnixTimestamp(DateTime.UtcNow);
        }

        public static long GetChartDataBeginTime(long from, long interval)
        {
            return (long) Math.Floor((double) from / interval) * interval;
        }

        public static string ConvertTime(long unixTimestamp)
        {
            const int SECOND = 1;
            const int MINUTE = 60 * SECOND;
            const int HOUR = 60 * MINUTE;
            const int DAY = 24 * HOUR;
            const int MONTH = 30 * DAY;

            var yourDate = DateTimeOffset.FromUnixTimeSeconds(unixTimestamp).DateTime;
            
            var ts = new TimeSpan(DateTime.UtcNow.Ticks - yourDate.Ticks);
            double delta = Math.Abs(ts.TotalSeconds);

            if (delta < 1 * MINUTE)
                return ts.Seconds == 1 ? "one second ago" : ts.Seconds + " seconds ago";

            if (delta < 2 * MINUTE)
                return "a minute ago";

            if (delta < 45 * MINUTE)
                return ts.Minutes + " minutes ago";

            if (delta < 90 * MINUTE)
                return "an hour ago";

            if (delta < 24 * HOUR)
                return ts.Hours + " hours ago";

            if (delta < 48 * HOUR)
                return "yesterday";

            if (delta < 30 * DAY)
                return ts.Days + " days ago";

            if (delta < 12 * MONTH)
            {
                int months = Convert.ToInt32(Math.Floor((double)ts.Days / 30));
                return months <= 1 ? "one month ago" : months + " months ago";
            }
            else
            {
                int years = Convert.ToInt32(Math.Floor((double)ts.Days / 365));
                return years <= 1 ? "one year ago" : years + " years ago";
            }
        }
    }
}