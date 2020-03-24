using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;

namespace Exceptions
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                var date = DateTime.Now;
                Console.WriteLine(date.ToString());
                Console.WriteLine(date.ToString(CultureInfo.InvariantCulture));
                Console.WriteLine(date.ToString("yyyyMMdd hh:mm"));
                Console.ReadLine();
                
                var exceptions = new List<Exception>();
                // do some stuff here ........
                // we have an exception with an innerexception, so add it to the list
                exceptions.Add(new TimeoutException("It timed out", new ArgumentException("ID missing")));
                // do more stuff .....            
                // Another exception, add to list
                exceptions.Add(new NotImplementedException("Somethings not implemented"));
                // all done, now create the AggregateException and throw it
                var aggEx = new AggregateException(exceptions);
                //throw aggEx;
                
                throw new IOException().SetCode(0x70);
            }

            catch (IOException ex) when ((ex.HResult & 0xFFFF) == 0x27 || (ex.HResult & 0xFFFF) == 0x70) // (vcetne Malo mista na disku )
            {
                var str = ex.GetType();
                Console.WriteLine(str);
                //throw new InvalidOperationException();  rethrownuti zabije program a do dalsiho catche se nedostane
            }
            catch (Exception exc)
            {
                var ex = exc;
                Console.WriteLine(ex.Message);

                if (ex is AggregateException ae)
                {
                    foreach (var e in ae.Flatten().InnerExceptions) // vypis inner exceptions z aggregate exc
                    {
                        Console.WriteLine(e.Message);
                    }
                }
            }

            Console.ReadLine();
        }
    }

    public static class ExceptionHelper
    {
        public static Exception SetCode(this Exception e, int value)
        {
            var flags = BindingFlags.Instance | BindingFlags.NonPublic;
            var fieldInfo = typeof(Exception).GetField("_HResult", flags);

            if (fieldInfo != null)
            {
                fieldInfo.SetValue(e, value);
            }

            return e;
        }
    }
}