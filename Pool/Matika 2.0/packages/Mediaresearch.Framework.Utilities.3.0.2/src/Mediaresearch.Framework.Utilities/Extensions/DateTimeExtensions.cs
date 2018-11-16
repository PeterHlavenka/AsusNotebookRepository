using System;

namespace Mediaresearch.Framework.Utilities.Extensions
{
    public static class DateTimeExtensions
    {
        public static DateTime RemoveMiliseconds(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second);
        }
    }
}