using System;
using Mediaresearch.Framework.Domain.History;
using Mediaresearch.Framework.Utilities.Extensions;

namespace Mediaresearch.Framework.Utilities
{
	public static class DateTimeHelper
	{
		public static DateTime GetTodayStamp(TimeSpan span)
		{
		    return DateTime.Now.Date.Add(span);
		}

		public static DateTime SetTime(DateTime date, TimeSpan span)
		{
		    return date.Date.Add(span);
		}

		public static DateTime RemoveMiliseconds(DateTime date)
		{
		    return date.RemoveMiliseconds();
		}

		/// <summary>
		/// Vraci produkcni den pro dany cas
		/// </summary>
		/// <param name="date">datum, ktery chci zmenit na produkcni den</param>
		/// <param name="productionTime">cas produkce</param>
		public static DateTime GetProductionDate(this DateTime date, TimeSpan productionTime)
		{
			if (date.TimeOfDay >= productionTime)
				return date.Date.Add(productionTime);

			return date.Subtract(TimeSpan.FromDays(1)).Date.Add(productionTime);
		}

		public static string FormatActivity(DateTime activeFrom, DateTime activeTo, string dateFormat)
		{
			return string.Format("{0} - {1}", activeFrom.ToString(dateFormat), activeTo.ToString(dateFormat));
		}


		public static DateTime ProductionAfterDeparture(DateTime date, TimeSpan dayStart)
		{
		    return date.TimeOfDay <= dayStart ? date.Date.Add(dayStart) : date.Date.Add(dayStart).AddDays(1);
		}

	    public static DateTime ProductionBeforeArrival(DateTime? date, TimeSpan dayStart)
		{
			if (date.HasValue)
			{
				return date.Value.TimeOfDay >= dayStart ? date.Value.Date.Add(dayStart) : date.Value.Date.Add(dayStart).AddDays(-1);
			}

			return HistoryConstants.DefaultValidTo;
		}


		public static int ComputeAge(DateTime birthday, DateTime date)
		{
			if (date < birthday)
				return 0;

			int age = date.Year - birthday.Year;

			if (date.Month < birthday.Month)
				age--;

			if (date.Month == birthday.Month && date.Day < birthday.Day)
				age--;

			return age;
		}

		public static int? ComputeAge(DateTime? birthday, DateTime? date)
		{
			if (birthday.HasValue && date.HasValue)
			{
				return ComputeAge(birthday.Value, date.Value);
			}

			return null;
		}

	}

}
