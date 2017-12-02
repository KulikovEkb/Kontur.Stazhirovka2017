using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using LiteDB;
using System.ServiceModel.Web;

namespace Kontur.GameStats.Server.Classes
{
    [DataContract(Name = "server")]
    public class Server
    {
        public int Id { get; set; }

        [DataMember(Name = "name", Order = 1, IsRequired = true)]
        public string Name { get; set; }
        [DataMember(Name = "gameModes", Order = 2, IsRequired = true)]
        public string[] GameModes { get; set; }

        public string Endpoint { get; set; }

        public static void AddServerInfo(LiteCollection<Classes.Server> serversCollection, Classes.Server server, string endpoint)
        {
            if (serversCollection.FindOne(x => x.Endpoint == endpoint) == null)
                serversCollection.Insert(server);
            else
            {
                server.Id = serversCollection.FindOne(x => x.Endpoint == endpoint).Id;
                serversCollection.Update(server);
            }
        }
        public static Classes.Server GetServerInfo(LiteCollection<Classes.Server> serversCollection, string endpoint)
        {
            if (!(serversCollection.FindOne(x => x.Endpoint == endpoint) == null))
                return serversCollection.FindOne(x => x.Endpoint == endpoint);
            else
                throw new WebFaultException(System.Net.HttpStatusCode.NotFound);
        }
    }
}
