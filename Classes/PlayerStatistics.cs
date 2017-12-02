using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using LiteDB;

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
        public float AverageScoreboardPercent { get; set; }
        [DataMember(Name = "maximumMatchesPerDay", Order = 7)]
        public int MaximumMatchesPerDay { get; set; }
        [DataMember(Name = "averageMatchesPerDay", Order = 8)]
        public float AverageMatchesPerDay { get; set; }
        [DataMember(Name = "lastMatchPlayed", Order = 9)]
        public string LastMatchPlayed { get; set; }
        [DataMember(Name = "killToDeathRatio", Order = 10)]
        public float KillToDeathRatio { get; set; }

        public static PlayerStatistics GetPlayerStats(LiteCollection<Match> matchesCollection, string name)
        {
            var playerMatches = matchesCollection.FindAll()
                    .Where(x => x.Scoreboard.Any(y => y.NameInUpperCase == name));
            var playerStats = new PlayerStatistics();
            playerStats.TotalMatchesPlayed = playerMatches.Count();

            playerStats.TotalMatchesWon = playerMatches
                .Where(x => x.Scoreboard[0].NameInUpperCase == name)
                .Count();

            playerStats.FavoriteServer = playerMatches
                .GroupBy(x => x.Endpoint)
                .OrderByDescending(x => x.Count())
                .Select(x => x.Key)
                .First();

            playerStats.UniqueServers = playerMatches
                .GroupBy(x => x.Endpoint)
                .Count();

            playerStats.FavoriteGameMode = playerMatches
                .GroupBy(x => x.GameMode)
                .OrderByDescending(x => x.Count())
                .Select(x => x.Key)
                .First();

            playerStats.AverageScoreboardPercent = playerMatches
                .SelectMany(x => x.Scoreboard.Where(y => y.NameInUpperCase == name).Select(z => z.ScoreboardPercent))
                .Average();

            playerStats.MaximumMatchesPerDay = playerMatches
                .GroupBy(x => x.JustDateFromTimestamp)
                .OrderByDescending(x => x.Count())
                .Select(x => x.Count())
                .First();

            playerStats.AverageMatchesPerDay = (float)playerMatches
                .GroupBy(x => x.JustDateFromTimestamp)
                .Select(x => x.Count())
                .Average();

            playerStats.LastMatchPlayed = playerMatches
                .OrderByDescending(x => x.DateTimeTimestamp)
                .Select(x => x.StringTimestamp)
                .First();

            var totalKills = playerMatches
                .SelectMany(x => x.Scoreboard.Where(y => y.NameInUpperCase == name).Select(z => z.Kills))
                .Sum();
            var totalDeaths = playerMatches
                .SelectMany(x => x.Scoreboard.Where(y => y.NameInUpperCase == name).Select(z => z.Deaths))
                .Sum();
            playerStats.KillToDeathRatio = (float)totalKills / totalDeaths;

            return playerStats;
        }
    }
}
