using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Matika_2._0
{
   public class Difference : Example
   {
       private static string Sign => " - ";

       public Difference()
       {
           var first = new Random().Next(Max);
           var second = new Random().Next(Max - first);

           Result = first - second;

           Task = string.Join(" ", first, Sign, second, EqualSign);
        }
   }
}
