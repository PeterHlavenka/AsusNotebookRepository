using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Exceptions
{
    class Program
    {
        static void Main(string[] args)
        {

            try
            {

                throw new IOException().SetCode(0x70);               
            }

            catch (IOException ex) when ((ex.HResult & 0xFFFF) == 0x27 || (ex.HResult & 0xFFFF) == 0x70)
            {
                Console.WriteLine(ex.Message);
            }
            catch (Exception exc)
            {
                var ex = exc;
                Console.WriteLine(ex.Message);
            }

            Console.ReadLine();
        }


    }

    public static class ExceptionHelper
    {
        public static Exception SetCode(this Exception e, int value)
        {
            BindingFlags flags = BindingFlags.Instance | BindingFlags.NonPublic;
            FieldInfo fieldInfo = typeof(Exception).GetField("_HResult", flags);

            if (fieldInfo != null)
            {
                fieldInfo.SetValue(e, value);
            }

            return e;
        }
    }
}
