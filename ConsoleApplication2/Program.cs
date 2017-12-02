using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication2
{
    public class User
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public DateTime MyDate { get; set; }
    }
    class Program
    {
        static void Main(string[] args)
        {
            List<User> users = new List<User>
            {
                new User {Name = "Max", Age = 20, MyDate = new DateTime(2017, 02, 01, 12, 15, 17).ToUniversalTime()},
                new User {Name = "Max", Age = 21, MyDate = new DateTime(2017, 02, 01, 12, 15, 17).ToUniversalTime()},
                new User {Name = "Max", Age = 22, MyDate = new DateTime(2017, 02, 02, 12, 15, 17).ToUniversalTime()},
                new User {Name = "Max", Age = 30, MyDate = new DateTime(2017, 02, 16, 12, 15, 17).ToUniversalTime()},
                new User {Name = "Igor", Age = 20, MyDate = new DateTime(2017, 02, 15, 12, 15, 17).ToUniversalTime()},
                new User {Name = "Alex", Age = 20, MyDate = new DateTime(2017, 02, 15, 12, 15, 17).ToUniversalTime()},
                new User {Name = "Alex", Age = 21, MyDate = new DateTime(2017, 02, 15, 12, 15, 17).ToUniversalTime()},
                new User {Name = "Oleg", Age = 20, MyDate = new DateTime(2017, 02, 15, 12, 15, 17).ToUniversalTime()},
                new User {Name = "Oleg", Age = 25, MyDate = new DateTime(2017, 02, 15, 12, 15, 17).ToUniversalTime()}
            };

            int result = users
                .GroupBy(x => x.Name)
                .Max(x => x.Count());
            double resultAverage = users
                .GroupBy(x => x.Name)
                .Average(x => x.Count());
            int dtresult = users
                .GroupBy(x => x.MyDate.Date)
                .Max(x => x.Count());
            Console.WriteLine(result);
            Console.WriteLine(resultAverage);
            Console.WriteLine(dtresult);
            string name = "Konstantin";
            Console.WriteLine(name.Substring(0,10));
        }
    }
}
