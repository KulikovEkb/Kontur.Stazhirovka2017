using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Kontur.GameStats.Server.Classes
{
    [DataContract(Name = "serverStatistics")]
    public class ServerStatistics
    {
        [DataMember(Name = "totalMatchesPlayed", Order = 1)]
        public int TotalMatchesPlayed { get; set; }
        [DataMember(Name = "maximumMatchesPerDay", Order = 2)]
        public int MaximumMatchesPerDay { get; set; }
        [DataMember(Name = "averageMatchesPerDay", Order = 3)]
        public float AverageMatchesPerDay { get; set; }
        [DataMember(Name = "maximumPopulation", Order = 4)]
        public int MaximumPopulation { get; set; }
        [DataMember(Name = "averagePopulation", Order = 5)]
        public float AveragePopulation { get; set; }
        [DataMember(Name = "top5GameModes", Order = 6)]
        public List<string> Top5GameModes { get; set; }
        [DataMember(Name = "top5Maps", Order = 7)]
        public List<string> Top5Maps { get; set; }
    }
}
