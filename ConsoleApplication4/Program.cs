using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication4
{
    public class Server
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string[] GameModes { get; set; }
        public string Endpoint { get; set; }
    }
    class Program
    {
        static void Main(string[] args)
        {
            Server firstServer = new Server { Name = "firstServer", GameModes = new string[] { "asf", "lk;l" } };
            Server secondServer = new Server { Name = "secondServer", GameModes = new string[] { "qqqq", "zzzz" } };
            AddServerInfo(firstServer, "first");
            AddServerInfo(secondServer, "second");
            secondServer = new Server { Name = "thirdServer", GameModes = new string[] { "qqqq", "zzzz", "xxxx" } };
            AddServerInfo(secondServer, "second");
        }
        public static void AddServerInfo(Server server, string endpoint)
        {

            server.Endpoint = endpoint;
            using (var database = new LiteDatabase(@"Storage.db"))
            {
                var userCollection = database.GetCollection<Server>("servers");
                //userCollection.EnsureIndex(x => x.Endpoint);
                //userCollection.Upsert(server);

                if (userCollection.FindOne(x => x.Endpoint == endpoint) == null)
                    userCollection.Insert(server);
                else
                {
                    server.Id = userCollection.FindOne(x => x.Endpoint == endpoint).Id;
                    userCollection.Update(server);
                }
                

                var servers = userCollection.FindAll();

                foreach (var item in servers)
                {
                    Console.WriteLine($"{item.Endpoint}, {item.Name}, {item.GameModes.Count()}");
                }
                Console.WriteLine();
            }
        }
    }
}
