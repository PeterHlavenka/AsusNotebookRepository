using System;
using System.Text;

namespace Mediaresearch.Framework.Utilities
{
    public static class ExceptionExtension  
    {
        /// <summary>
        /// Vrati exception message vcetne vsech innerException
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public static string GetFullExceptionMessage(this Exception ex)
        {
            return GetMessage(new StringBuilder(), ex);
        }

        private static string GetMessage(StringBuilder message, Exception ex)
        {
            message.AppendLine(ex.Message);

            if (ex.InnerException != null)
            {
                message.AppendLine();
                return GetMessage(message, ex.InnerException);
            }

            return message.ToString();
        }
    }
}