using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetDuplicates.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var duplicates = StringUtility.GetDuplicateElements("abcAAbEEe");

            Console.WriteLine(string.Join(", ", duplicates));

            Console.ReadKey();
        }
    }
}
