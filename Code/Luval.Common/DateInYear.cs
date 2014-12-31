using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luval.Common
{
    public class DateInYear
    {


        private readonly DayOfWeek _firstDayOfWeek;
        private readonly double _productionDayOffSetInMinutes;
        
        #region Property Implementation

        public DateTime DateValue { get; set; }
        public string DateCode { get; set; }
        public ushort Year { get; set; }
        public ushort Semester { get; set; }
        public ushort Quater { get; set; }
        public ushort Month { get; set; }
        public string MonthName { get; set; }
        public ushort Week { get; set; }
        public ushort Day { get; set; }
        public ushort DayOfYear { get; set; }
        public ushort DayOfWeek { get; set; }
        public string DayOfWeekName { get; set; }
        public ushort Hour { get; set; }
        public uint HourOfYear { get; set; }
        public ushort Minute { get; set; }
        public uint MinuteOfYear { get; set; }
        public ushort Second { get; set; }
        public uint SecondOfYear { get; set; }
        public uint Chunk30MinOfYear { get; set; }
        public uint Chunk15MinOfYear { get; set; }
        public uint Chunk10MinOfYear { get; set; }
        public uint Chunk5MinOfYear { get; set; }

        #endregion

        #region Constructors

        public DateInYear()
            : this(DateTime.Now)
        {

        }

        public DateInYear(DateTime date)
            : this(date, 0)
        {
        }


        public DateInYear(DateTime date, double productionDayOffSetInMinutes)
            : this(date, productionDayOffSetInMinutes, GetDefaultCulture().DateTimeFormat.FirstDayOfWeek)
        {

        }

        public DateInYear(DateTime date, double productionDayOffSetInMinutes, DayOfWeek firstDayOfWeek)
        {
            _productionDayOffSetInMinutes = productionDayOffSetInMinutes;
            _firstDayOfWeek = firstDayOfWeek;
            Initialize(date);
        }

        #endregion

        #region Methods

        private static CultureInfo GetDefaultCulture()
        {
            return new CultureInfo("es-ES");
        }

        public void Initialize(DateTime date)
        {
            Initialize(date, GetDefaultCulture());
        }

        public void Initialize(DateTime date, CultureInfo culture)
        {
            DateValue = date.AddMinutes(_productionDayOffSetInMinutes);
            DateCode = date.ToString("yyyyMMddHHmmss");
            Year = (ushort)DateValue.Year;
            Semester = (ushort)(DateValue.Month > 6 ? 2 : 1);
            Quater = (ushort)((DateValue.Month - 1) / 3 + 1);
            Month = (ushort)DateValue.Month;
            MonthName = DateValue.ToString("MMMM", culture);
            Week = (ushort)culture.Calendar.GetWeekOfYear(DateValue, CalendarWeekRule.FirstDay, _firstDayOfWeek);
            DayOfWeek = (ushort)DateValue.DayOfWeek;
            DayOfWeekName = DateValue.ToString("dddd", culture);
            Day = (ushort)DateValue.Day;
            DayOfYear = (ushort)DateValue.DayOfYear;
            Hour = (ushort) DateValue.Hour;
            HourOfYear = (uint)(((DayOfYear - 1) * 24) + DateValue.Hour);
            Minute = (ushort) DateValue.Minute;
            MinuteOfYear = (uint)((HourOfYear * 60) + DateValue.Minute);
            Second = (ushort) DateValue.Second;
            SecondOfYear = (uint)((MinuteOfYear * 60) + DateValue.Second);
            Chunk30MinOfYear = (uint)((HourOfYear * 2) + (int)(DateValue.Minute / 30));
            Chunk15MinOfYear = (uint)((HourOfYear * 4) + (int)(DateValue.Minute / 15));
            Chunk10MinOfYear = (uint)((HourOfYear * 12) + (int)(DateValue.Minute / 10));
            Chunk5MinOfYear = (uint)((HourOfYear * 12) + (int)(DateValue.Minute / 5));
        }

        #region Sql Methods

        public static string ToSqlInsertHeader()
        {
            return
                "INSERT INTO Date ({0})".Fi(GetSqlColumnNames());
        }

        public string ToSqlInsertValues()
        {
            return "({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18},{19},{20},{21})".FormatSql(DateValue, DateCode,Year, Semester, Quater, Month, MonthName, Week, DayOfWeek, DayOfWeekName, Day, DayOfYear, Hour, HourOfYear, Minute, MinuteOfYear, Second, SecondOfYear, Chunk30MinOfYear, Chunk15MinOfYear, Chunk10MinOfYear, Chunk5MinOfYear);
        }

        public string ToSqlIsert()
        {
            return "{0} VALUES {1}".Fi(ToSqlInsertHeader(), ToSqlInsertValues());
        }

        public string ToSqlSelect()
        {
            return "SELECT {0} FROM Date".Fi(GetSqlColumnNames());
        }

        private static string GetSqlColumnNames()
        {
            return "DateValue, DateCode, Year, Semester, Quater, Month, MonthName, Week, DayOfWeek, DayOfWeekName, Day, DayOfYear, Hour, HourOfYear, Minute, MinuteOfYear, Second, SecondOfYear, Chunk30MinOfYear, Chunk15MinOfYear, Chunk10MinOfYear, Chunk5MinOfYear";
        } 

        #endregion

        public static DateInYear FromDateTime(DateTime dateTime)
        {
            return new DateInYear(dateTime);
        }

        #endregion
    }
}

