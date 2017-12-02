using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Kontur.GameStats.Server.Classes
{
    [DataContract(Name = "serverReport")]
    public class ServerReport
    {
        [DataMember(Name = "endpoint", Order = 1)]
        public string Endpoint { get; set; }
        [DataMember(Name = "name", Order = 2)]
        public string Name { get; set; }
        [DataMember(Name = "averageMatchesPerDay", Order = 3)]
        public float AverageMatchesPerDay { get; set; }
    }
}
