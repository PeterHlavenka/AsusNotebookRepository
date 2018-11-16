using System;
using System.Linq;

namespace Mediaresearch.Framework.Utilities.Calendar
{
    public class HolidaysHelper
    {
        private static readonly DayOfWeek[] m_daysOfWeekend =
        {
            DayOfWeek.Sunday, DayOfWeek.Saturday
        };

        private static readonly DateTime[] m_holidays =
        {
            new DateTime(2000, 1, 1), //"Nový rok"
            new DateTime(2000, 5, 1), //"Svátek práce"
            new DateTime(2000, 5, 8),//"Den vítězství"
            new DateTime(2000, 7, 5),//"Den slovanských věrozvěstů Cyrila a Metoděje"
            new DateTime(2000, 7, 6),//"Den upálení mistra Jana Husa"
            new DateTime(2000, 9, 28),//"Den české státnosti"
            new DateTime(2000, 10, 28),//"Den vzniku samostatného československého státu"
            new DateTime(2000, 11, 17),//"Den boje za svobodu a demokracii"
            new DateTime(2000, 12, 24),//""Štědrý den""
            new DateTime(2000, 12, 25),//"1. svátek vánoční"
            new DateTime(2000, 12, 26)//"2. svátek vánoční"
        };

        /// <summary>
        /// Vraci velikonocni nedeli pro dany rok
        /// </summary>
        /// <param name="year">Rok</param>
        /// <returns>Pulnoc velikonocni nedele</returns>
        private static DateTime EasterDay(int year)
        {
            int g = year%19;
            int c = year/100;
            int h = ((c - (c/4) - ((8*c + 13)/25) + (19*g) + 15)%30);
            int i = h - ((h/28)*(1 - (h/28)*(29/(h + 1))*((21 - g)/11)));
            int j = ((year + (year/4) + i + 2 - c + (c/4))%7);
            int l = i - j;
            int eMonth = 3 + ((l + 40)/44);
            int eDay = l + 28 - (31*(eMonth/4));
            DateTime eDateTime = new DateTime(year, eMonth, eDay, 0, 0, 0, 0);
            return eDateTime;
        }

        public static bool IsEasterDay(DateTime dateTime)
        {
            var isEasterDay = EasterDay(dateTime.Year);

            return isEasterDay == dateTime.Date || isEasterDay.AddDays(-2) == dateTime.Date || isEasterDay.AddDays(1) == dateTime;
        }

        public static bool IsWeekend(DateTime dateTime)
        {
            return m_daysOfWeekend.Any(d => d == dateTime.DayOfWeek);
        }

        public static bool IsHoliday(DateTime dateTime)
        {
            return m_holidays.Any(d => d.Month == dateTime.Month && d.Day == dateTime.Day);
        }

        public static bool IsFreeDay(DateTime dateTime)
        {
            return IsWeekend(dateTime) || IsHoliday(dateTime) || IsEasterDay(dateTime);
        }
    }
}