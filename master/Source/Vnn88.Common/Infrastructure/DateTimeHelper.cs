using System;

namespace Vnn88.Common.Infrastructure
{
    public static class DateTimeHelper
    {
        private static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0,
            DateTimeKind.Utc);
        /// <summary>
        /// Common DateTime Methods.
        /// </summary>
        ///

        public enum Quarter
        {
            First = 1,
            Second = 2,
            Third = 3,
            Fourth = 4
        }

        public enum Month
        {
            January = 1,
            February = 2,
            March = 3,
            April = 4,
            May = 5,
            June = 6,
            July = 7,
            August = 8,
            September = 9,
            October = 10,
            November = 11,
            December = 12
        }

        #region Public Methods

        public static DateTime AddQuarter(int quarters, DateTime origin)
        {
            var months = quarters * 3;
            return FirstDayOfQuarter(origin.AddMonths(months));
        }

        public static DateTime AddWorkDays(int days, DateTime theDate)
        {
            var direction = 1;
            if (days < 0)
            {
                direction = -direction;
            }
            // INSTANT C# NOTE: The ending condition of VB 'For' loops is tested only on entry to the loop. Instant C# has created a temporary variable in order to use the initial value of days for every iteration:
            var tempFor1 = days;
            for (var dayToAdd = 1; dayToAdd <= tempFor1; dayToAdd++)
            {
                theDate = theDate.AddDays(direction);
                while (theDate.DayOfWeek == DayOfWeek.Saturday | theDate.DayOfWeek == DayOfWeek.Sunday)
                {
                    theDate = theDate.AddDays(direction);
                }
            }
            return theDate;
        }

        public static DateTime BeginningOfFiscalYearDate(DateTime inputDate, DateTime currentFiscalBeginDate)
        {
            DateTime newDate;
            DateTime dteTemp;
            if (inputDate.DayOfYear < currentFiscalBeginDate.DayOfYear)
            {
                dteTemp = inputDate;
                dteTemp = dteTemp.AddYears(-1);
                newDate = new DateTime(dteTemp.Year, currentFiscalBeginDate.Month, currentFiscalBeginDate.Day);
            }
            else
            {
                dteTemp = inputDate;
                newDate = new DateTime(dteTemp.Year, currentFiscalBeginDate.Month, currentFiscalBeginDate.Day);
            }
            return newDate;
        }

        public static DateTime DateTimeFromString(string year, string month, string day)
        {
            var validDate = new DateTime();
            IsDate(month + ("/" + (day + ("/" + year))), ref validDate);
            return validDate;
        }

        public static DateTime FirstDayOfQuarter(DateTime day)
        {
            return new DateTime(day.Year, FirstMonthOfQuarter(QuarterOfMonth(day)), 1);
        }

        public static int FirstMonthOfQuarter(int qtr)
        {
            if (qtr == 1)
            {
                return 1;
            }
            if (qtr == 2)
            {
                return 4;
            }
            if (qtr == 3)
            {
                return 7;
            }
            if (qtr == 4)
            {
                return 10;
            }
            return 0;
        }

        public static DateTime GetEndOfCurrentMonth()
        {
            return GetEndOfMonth(DateTime.Now.Month, DateTime.Now.Year);
        }

        public static DateTime GetEndOfCurrentWeek()
        {
            var dt = GetStartOfCurrentWeek().AddDays(6);
            return GetEndOfDay(dt);
        }

        public static DateTime GetEndOfCurrentWorkWeek()
        {
            var dt = GetEndOfCurrentWeek().AddDays(-1);
            return dt;
        }

        public static DateTime GetEndOfCurrentYear()
        {
            return GetEndOfYear(DateTime.Now.Year);
        }

        public static DateTime GetEndOfDay(DateTime theDate)
        {
            return new DateTime(theDate.Year, theDate.Month, theDate.Day, 23, 59, 59, 999);
        }

        public static DateTime GetEndOfLastMonth()
        {
            if (DateTime.Now.Month == 1)
            {
                return GetEndOfMonth(12, DateTime.Now.Year - 1);
            }
            else
            {
                return GetEndOfMonth(DateTime.Now.Month - 1, DateTime.Now.Year);
            }
        }

        public static DateTime GetEndOfLastWeek()
        {
            var dt = GetStartOfLastWeek().AddDays(6);
            return new DateTime(dt.Year, dt.Month, dt.Day, 23, 59, 59, 999);
        }

        public static DateTime GetEndOfLastYear()
        {
            return GetEndOfYear(DateTime.Now.Year - 1);
        }

        public static DateTime GetEndOfMonth(int month, int year)
        {
            if (month < 1)
            {
                throw new ArgumentException("Month < 1", nameof(month));
            }
            if (month > 12)
            {
                throw new ArgumentException("Month > 12", nameof(month));
            }
            if (year < 1)
            {
                throw new ArgumentException("Year < 1", nameof(year));
            }
            if (year > 9999)
            {
                throw new ArgumentException("Year > 12", nameof(year));
            }
            return new DateTime(year, month, DateTime.DaysInMonth(year, month), 23, 59, 59, 999);
        }

        public static DateTime GetEndOfWeek(DateTime dayInWeek)
        {
            return GetEndOfDay(GetStartOfWeek(dayInWeek).AddDays(6));
        }

        public static DateTime GetEndOfWorkWeek(DateTime dayInWeek)
        {
            return GetEndOfWeek(dayInWeek).AddDays(-1);
        }

        public static DateTime GetEndOfYear(int year)
        {
            return new DateTime(year, 12, 31, 23, 59, 59, 999);
        }

        public static string GetShortDateOrEmptyString(DateTime? nullableDateTime)
        {
            return nullableDateTime.HasValue ? nullableDateTime.Value.ToShortDateString() : string.Empty;
        }

        public static DateTime GetStartOfCurrentMonth()
        {
            return GetStartOfMonth(DateTime.Now.Month, DateTime.Now.Year);
        }

        public static DateTime GetStartOfCurrentWeek()
        {
            return GetStartOfWeek(DateTime.Now);
        }

        public static DateTime GetStartOfCurrentWorkWeek()
        {
            var dt = GetStartOfCurrentWeek().AddDays(1);
            return dt;
        }

        public static DateTime GetStartOfCurrentYear()
        {
            return GetStartOfYear(DateTime.Now.Year);
        }

        public static DateTime GetStartOfDay(DateTime theDate)
        {
            return new DateTime(theDate.Year, theDate.Month, theDate.Day, 0, 0, 0, 0);
        }

        public static DateTime GetStartOfLastMonth()
        {
            if (DateTime.Now.Month == 1)
            {
                return GetStartOfMonth(12, DateTime.Now.Year - 1);
            }
            else
            {
                return GetStartOfMonth(DateTime.Now.Month - 1, DateTime.Now.Year);
            }
        }

        public static DateTime GetStartOfLastWeek()
        {
            var daysToSubtract = Convert.ToInt32(DateTime.Now.DayOfWeek) + 7;
            var dt = DateTime.Now.Subtract(TimeSpan.FromDays(daysToSubtract));
            return new DateTime(dt.Year, dt.Month, dt.Day, 0, 0, 0, 0);
        }

        public static DateTime GetStartOfLastYear()
        {
            return GetStartOfYear(DateTime.Now.Year - 1);
        }

        public static DateTime GetStartOfMonth(int month, int year)
        {
            return new DateTime(year, month, 1, 0, 0, 0, 0);
        }

        public static DateTime GetStartOfWeek(DateTime dayInWeek)
        {
            var daysToSubtract = Convert.ToInt32(dayInWeek.DayOfWeek);
            var dt = dayInWeek.Subtract(TimeSpan.FromDays(daysToSubtract));
            return GetStartOfDay(dt);
        }

        public static DateTime GetStartOfWorkWeek(DateTime dayInWeek)
        {
            return GetStartOfWeek(dayInWeek).AddDays(1);
        }

        public static DateTime GetStartOfYear(int year)
        {
            return new DateTime(year, 1, 1, 0, 0, 0, 0);
        }

        public static bool IsDate(string dateAsString)
        {
            var pointlessDateTime = new DateTime();
            return IsDate(dateAsString, ref pointlessDateTime);
        }

        public static bool IsDate(string dateString, ref DateTime validDate)
        {
            var isValid = true;
            try
            {
                validDate = DateTime.Parse(dateString);
            }
            catch (Exception)
            {
                isValid = false;
                validDate = DateTime.Now;
            }
            return isValid;
        }

        public static string MonthAndYearStamp(int month, int year)
        {
            return string.Format("(0)/(1)", month, year);
        }

        public static DateTime? ParseNullableDateTimeString(string dateString)
        {
            DateTime? result = null;
            if (!String.IsNullOrEmpty(dateString))
            {
                if (DateTime.TryParse(dateString, out var tempDate))
                {
                    result = tempDate;
                }
            }
            return result;
        }

        public static int QuarterOfMonth(DateTime theDate)
        {
            var i = theDate.Month;
            if (i <= 3)
            {
                return 1;
            }

            if ((i >= 4) && (i <= 6))
            {
                return 2;
            }

            if ((i >= 7) && (i <= 9))
            {
                return 3;
            }

            if ((i >= 10) && (i <= 12))
            {
                return 4;
            }

            return 0;
        }

        public static DateTime FromMillisecondsSinceUnixEpoch(long milliseconds)
        {
            return UnixEpoch.AddMilliseconds(milliseconds);
        }

        public static string QuarterAndYearStamp(int quarter, int year)
        {
            return string.Format("Quarter (0) of the year (1)", quarter, year);
        }

        public static double TimeSpanToDouble(string timeSpan)
        {
            var hour = timeSpan.Split(':')[0];
            var minute = timeSpan.Split(':')[1];
            if (!string.IsNullOrEmpty(hour) && !string.IsNullOrEmpty(minute))
            {
                if (double.Parse(hour) >= 24)
                {
                    hour = Constants.Settings.MaxTimezoneTimeHour;
                }
                if (double.Parse(minute) >= 60)
                {
                    minute = Constants.Settings.MaxTimezoneTimeMinute;
                }
                timeSpan = $"{hour}:{minute}";
            }
            return Convert.ToDouble(TimeSpan.Parse(timeSpan).TotalHours);
        }

        public static string DoubleToTimeSpan(double dou)
        {
            if(dou >= 24)
            {
                dou = Constants.Settings.MaxTimezoneTimeHourMinute;
            }
            return TimeSpan.FromHours(dou).ToString("h\\.mm");
        }
        #endregion Public Methods
    }
}