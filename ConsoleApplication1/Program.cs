using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    public class Server
    {
        public string Name { get; set; }
        public string[] GameModes { get; set; }
        public string Endpoint { get; set; }
    }

    public class ServerInfo
    {
        public string Endpoint { get; set; }
        public Server Info { get; set; }
    }

    class Service
    {
        public void AddServerInfo(Server server, string endpoint)
        {

            server.Endpoint = endpoint;
            using (var database = new LiteDatabase(@"Storage.db"))
            {
                var userCollection = database.GetCollection<Server>("servers");
                userCollection.Insert(server);
            }
        }

        public List<ServerInfo> GetServersInfo()
        {
            using (var database = new LiteDatabase(@"Storage.db"))
            {
                var userCollection = database.GetCollection<Server>("servers");
                var servers = userCollection.FindAll();
                var result = new List<ServerInfo>();
                //result.Add(new ServerInfo());
                //result.Add(new ServerInfo());

                //int i = 0;
                foreach (Server item in servers)
                {
                    result.Add(new ServerInfo { Endpoint = item.Endpoint, Info = item });
                    //result[i].Endpoint = item.Endpoint;
                    //result[i].Info = item;
                    //i++;
                }
                return result;
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Server server = new Server { Endpoint = "", GameModes = new string[] { "aaa", "bbb" }, Name = "MPS" };
            Service zz = new Service();
            zz.AddServerInfo(server, "test");
            Server server2 = new Server { Endpoint = "", GameModes = new string[] { "aaa", "bbb" }, Name = "MPS1111" };
            zz.AddServerInfo(server2, "test2");
            List<ServerInfo> result = zz.GetServersInfo();
        }
    }
}
