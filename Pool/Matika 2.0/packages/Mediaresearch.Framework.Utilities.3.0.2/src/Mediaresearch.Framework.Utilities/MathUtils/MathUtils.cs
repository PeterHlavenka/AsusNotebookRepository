using System;

namespace Mediaresearch.Framework.Utilities.MathUtils
{
	public static class MathUtils
	{
		/// <summary>
		/// zaokrouhli cislo, pokud je o jedna mensi nebo o jedna vetsi nez nasobek peti a neni to 1 na nejblizsi nasobek peti
		/// </summary>
		/// <param name="number">jakykoliv integer</param>
		public static int RoundNumberToFiveMultiple(int number)
		{
			var modulo = number % 5;
			if (modulo == 1 && number != 1)
			{
				return --number;
			}

			if (modulo == 4)
				return ++number;

			return number;
		}

        /// <summary>
        /// Truncate decimalu (ne round).
        /// Truncate(-0.89, 1)      => -0.8
        /// Truncate(1234.89, -1)   => 1230
        /// </summary>
        /// <param name="precision">Kladné číslo určuje počet míst za desetinou čárkou, záporné počet míst před desetinou čárkou.</param>
        public static decimal Truncate(decimal value, int precision)
	    {
	        if (Math.Abs(precision) > 28)
	            throw new ArgumentException("Math.Abs(precision) > 28");

            decimal step = (decimal)Math.Pow(10, precision);
	        long tmp = (long)Math.Truncate(step * value);
            return tmp / step;
        }
    }
}