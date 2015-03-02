using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luval.Common
{
    public static class DateTimeExtension
    {
        public static uint ToEpoch(this DateTime d)
        {
            return (uint)d.Subtract(MinDate).TotalMilliseconds;
        }

        public static uint ToInt(this DateTime d)
        {
            return ToEpoch(d);
        }

        public static DateTime TrimMilliseconds(this DateTime d)
        {
            return new DateTime(d.Year, d.Month, d.Day, d.Hour, d.Minute, d.Second, 0);
        }

        public static DateTime TrimSeconds(this DateTime d)
        {
            return new DateTime(d.Year, d.Month, d.Day, d.Hour, d.Minute, 0, 0);
        }

        public static DateTime TrimMinutes(this DateTime d)
        {
            return new DateTime(d.Year, d.Month, d.Day, d.Hour, 0, 0, 0);
        }

        public static DateTime TrimHours(this DateTime d)
        {
            return new DateTime(d.Year, d.Month, d.Day, 0, 0, 0, 0);
        }

        public static DateTime TrimDays(this DateTime d)
        {
            return new DateTime(d.Year, d.Month, 1, 0, 0, 0, 0);
        }

        public static DateTime TrimMonths(this DateTime d)
        {
            return new DateTime(d.Year, 1, 1, 0, 0, 0, 0);
        }

        public static string ToInternationalFormat(this DateTime d)
        {
            return d.ToString("yyyy-MM-dd hh:mm:ss");
        }

        public static string ToInternationalFormatShort(this DateTime d)
        {
            return d.ToString("yyyy-MM-dd");
        }

        public static DateTime MinDate
        {
            get { return new DateTime(1970, 1, 1); }
        }

        public static DateTime MaxDate
        {
            get { return DateTime.MaxValue; }
        }
    }
}
