using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication3
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> names = new List<string>();
            names.AddRange(new string[] { "alex", "max", "igor", "boris", "kostya" });
            int pbc = names.Count - (names.IndexOf("max") + 1);
            double result = (double)pbc / (names.Count - 1) * 100;

            Console.WriteLine(result);
        }
    }
}
