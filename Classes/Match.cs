using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Kontur.GameStats.Server.Classes
{
    [DataContract(Name = "match")]
    public class Match
    {
        [DataMember(Name = "map", Order = 1, IsRequired = true)]
        public string Map { get; set; }
        [DataMember(Name = "gameMode", Order = 2, IsRequired = true)]
        public string GameMode { get; set; }
        [DataMember(Name = "fragLimit", Order = 3, IsRequired = true)]
        public int FragLimit { get; set; }
        [DataMember(Name = "timeLimit", Order = 4, IsRequired = true)]
        public int TimeLimit { get; set; }

        double timeElapsed;
        [DataMember(Name = "timeElapsed", Order = 5, IsRequired = true)]
        public double TimeElapsed
        {
            get { return timeElapsed; }
            set { timeElapsed = Math.Round(value, 6, MidpointRounding.AwayFromZero); }
        }

        [DataMember(Name = "scoreboard", Order = 6, IsRequired = true)]
        public List<Player> Scoreboard { get; set; }

        public string Endpoint { get; set; }
        public string StringTimestamp { get; set; }
        public DateTime DateTimeTimestamp { get; set; }
        public string JustDateFromTimestamp { get; set; }
    }
}
