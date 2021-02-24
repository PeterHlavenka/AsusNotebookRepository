using System;
using System.Text;

namespace StringBuilderAppendNullTest
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            string necoNull = null;

            var toolTip = new StringBuilder()
                .Append("mediumId: ")
                .Append(1234)
                .Append(" ").Append("name")
                .Append(": ")
                .Append(necoNull)
                .ToString();

            Console.WriteLine(toolTip);
            Console.ReadLine();
        }
    }
}