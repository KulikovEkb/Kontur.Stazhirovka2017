using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Kontur.GameStats.Server.Classes
{
    [DataContract(Name = "player")]
    public class Player
    {
        [DataMember(Name = "name", Order = 1)]
        public string Name { get; set; }
        [DataMember(Name = "frags", Order = 2)]
        public int Frags { get; set; }
        [DataMember(Name = "kills", Order = 3)]
        public int Kills { get; set; }
        [DataMember(Name = "deaths", Order = 4)]
        public int Deaths { get; set; }

        public string NameInUpperCase { get; set; }

        double scoreboardPercent;
        public double ScoreboardPercent
        {
            get { return scoreboardPercent; }
            set { scoreboardPercent = Math.Round(value, 6, MidpointRounding.AwayFromZero); }
        }
    }
}
