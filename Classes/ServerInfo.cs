using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Kontur.GameStats.Server.Classes
{
    [DataContract(Name = "serverInfo")]
    public class ServerInfo
    {
        [DataMember(Name = "endpoint", Order = 1)]
        public string Endpoint { get; set; }
        [DataMember(Name = "info", Order = 2)]
        public Server Info { get; set; }
    }
}
