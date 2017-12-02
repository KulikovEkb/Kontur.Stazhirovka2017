using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Kontur.GameStats.Server.Classes
{
    [DataContract(Name = "server")]
    public class Server
    {
        public int Id { get; set; }

        [DataMember(Name = "name", Order = 1)]
        public string Name { get; set; }
        [DataMember(Name = "gameModes", Order = 2)]
        public string[] GameModes { get; set; }
        
        public string Endpoint { get; set; }
    }
}
