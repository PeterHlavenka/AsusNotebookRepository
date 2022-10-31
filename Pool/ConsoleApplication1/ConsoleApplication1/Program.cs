using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApplication1
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var list = new List<CodebookItem>(){
                new CodebookItem(1, "avon"),
                new CodebookItem(2, "sencor"),
                new CodebookItem(2, "sencor od"),
                new CodebookItem(1, "avon"),
                new CodebookItem(1, "avon do"),
                new CodebookItem(1, "avon od"),
                new CodebookItem(1, "avon")
            };


            var result = (from ci in list
                group ci by ci.IdZnacky
                into gr
                where gr.Count() > 1
                select new
                {
                    gr.Key,
                    Names = gr.Select(d => d.Name).OrderBy(d => d.Length).Distinct()
                }).Where(d => d.Names.Count() > 1).ToDictionary(d => d.Key, d => d.Names);

            if (!result.Any()) return;
        
            var sb = new StringBuilder();
            sb.AppendLine("Existuje více platností pro tyto značky:");
            
            foreach (var str in result.SelectMany(keyValuePair => keyValuePair.Value))
            {
                sb.Append("\t");
                sb.AppendLine(str);
            }
            
            Console.WriteLine(sb.ToString());
            Console.ReadLine();
        }
    }
    
    public class CodebookItem {

        public CodebookItem(int id, string name){
            IdZnacky = id;
            Name = name;
        }

        public int IdZnacky {get;set;}
        public string Name {get;set;}
    }
}