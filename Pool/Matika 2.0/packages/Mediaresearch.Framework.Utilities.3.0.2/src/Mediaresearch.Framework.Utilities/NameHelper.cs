using System.Text;
using System.Text.RegularExpressions;

namespace Mediaresearch.Framework.Utilities
{
    public static class NameHelper
    {
        public static string GetFullName(string title, string name, string surname, string title2)
        {
            return string.Format("{0}{1} {2}{3}",
                                 string.IsNullOrEmpty(title) ? "" : string.Format("{0} ", title),
                                 name,
                                 surname,
                                 string.IsNullOrEmpty(title2) ? "" : string.Format(", {0}", title2));
        }

        public static string GetFullAddress(string street, string zipCode, string town)
        {
            var buf = new StringBuilder();

            string before = "";
            if (!string.IsNullOrEmpty(street))
            {
                buf.Append(street);
                before = ", ";
            }

            if (!string.IsNullOrEmpty(zipCode))
            {
                buf.Append(before);
                buf.Append(zipCode);
                before = " ";
            }

            if (!string.IsNullOrEmpty(town))
            {
                buf.Append(before);
                buf.Append(town);
            }

            return buf.ToString();
        }

        /// <summary>
        /// Metoda nahrazuje nazev ulice v textu, ktery tvori nazev ulice a cislo popisne.
        /// </summary>
        /// <param name="streetAndNumber">nazev ulice s cislem</param>
        /// <param name="newStreet">novy nazev ulice</param>
        /// <returns></returns>
        public static string ReplaceStreetName(string streetAndNumber, string newStreet)
        {
            if (newStreet == null)
            {
                newStreet = string.Empty;
            }

            if (!string.IsNullOrEmpty(streetAndNumber))
            {
                Regex regexNumberDot = new Regex("[\\s]*[0-9]+[.]+", RegexOptions.Singleline | RegexOptions.Compiled);
                Match matchND = regexNumberDot.Match(streetAndNumber);
                if (matchND.Success)
                {
                    int pos = streetAndNumber.IndexOf(matchND.Value);
                    if (pos == 0)
                        streetAndNumber = streetAndNumber.Substring(matchND.Value.Length); //skip street as "1. května XXX"
                }

                Regex regexNumber = new Regex("[\\s]*[0-9]+", RegexOptions.Singleline | RegexOptions.Compiled);
                Match matchN = regexNumber.Match(streetAndNumber);
                if (matchN.Success)
                {
                    int pos = streetAndNumber.IndexOf(matchN.Value);
                    if (pos < streetAndNumber.Length)
                    {
                        if (matchN.Value.StartsWith(" "))
                            return newStreet + streetAndNumber.Substring(pos);

                        return newStreet + " " + streetAndNumber.Substring(pos);
                    }
                }
            }

            return newStreet;
        }

    }
}
