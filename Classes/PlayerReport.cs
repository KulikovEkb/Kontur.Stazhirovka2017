using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Kontur.GameStats.Server.Classes
{
    [DataContract(Name = "playerReport")]
    public class PlayerReport
    {
        [DataMember(Name = "name", Order = 1)]
        public string Name { get; set; }
        [DataMember(Name = "killToDeathRatio", Order = 2)]
        public float KillToDeathRatio { get; set; }
    }
}
