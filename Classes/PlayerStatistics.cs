using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Kontur.GameStats.Server.Classes
{
    [DataContract(Name = "playerStatistics")]
    public class PlayerStatistics
    {
        [DataMember(Name = "totalMatchesPlayed", Order = 1)]
        public int TotalMatchesPlayed { get; set; }
        [DataMember(Name = "totalMatchesWon", Order = 2)]
        public int TotalMatchesWon { get; set; }
        [DataMember(Name = "favoriteServer", Order = 3)]
        public string FavoriteServer { get; set; }
        [DataMember(Name = "uniqueServers", Order = 4)]
        public int UniqueServers { get; set; }
        [DataMember(Name = "favoriteGameMode", Order = 5)]
        public string FavoriteGameMode { get; set; }
        [DataMember(Name = "averageScoreboardPercent", Order = 6)]
        public double AverageScoreboardPercent { get; set; }
        [DataMember(Name = "maximumMatchesPerDay", Order = 7)]
        public int MaximumMatchesPerDay { get; set; }
        [DataMember(Name = "averageMatchesPerDay", Order = 8)]
        public double AverageMatchesPerDay { get; set; }
        [DataMember(Name = "lastMatchPlayed", Order = 9)]
        public string LastMatchPlayed { get; set; }
        [DataMember(Name = "killToDeathRatio", Order = 10)]
        public double KillToDeathRatio { get; set; }
    }
}
