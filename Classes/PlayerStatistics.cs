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
        double averageScoreboardPercent;
        double averageMatchesPerDay;
        double killToDeathRatio;

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
        public double AverageScoreboardPercent
        {
            get { return averageScoreboardPercent; }
            set { averageScoreboardPercent = Math.Round(value, 6, MidpointRounding.AwayFromZero); }
        }
        [DataMember(Name = "maximumMatchesPerDay", Order = 7)]
        public int MaximumMatchesPerDay { get; set; }
        [DataMember(Name = "averageMatchesPerDay", Order = 8)]
        public double AverageMatchesPerDay
        {
            get { return averageMatchesPerDay; }
            set { averageMatchesPerDay = Math.Round(value, 6, MidpointRounding.AwayFromZero); }
        }
        [DataMember(Name = "lastMatchPlayed", Order = 9)]
        public string LastMatchPlayed { get; set; }
        [DataMember(Name = "killToDeathRatio", Order = 10)]
        public double KillToDeathRatio
        {
            get { return killToDeathRatio; }
            set { killToDeathRatio = Math.Round(value, 6, MidpointRounding.AwayFromZero); }
        }

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

            int daysOfPlayer = ((matchesCollection.FindAll().Max(x => x.DateTimeTimestamp).Day) - (playerMatches.Min(x => x.DateTimeTimestamp).Day)) + 1;
            playerStats.AverageMatchesPerDay = (double)playerStats.TotalMatchesPlayed / daysOfPlayer;

            playerStats.LastMatchPlayed = playerMatches
                .OrderByDescending(x => x.DateTimeTimestamp)
                .Select(x => x.StringTimestamp)
                .First();

            int totalKills = playerMatches
                .SelectMany(x => x.Scoreboard.Where(y => y.NameInUpperCase == name).Select(z => z.Kills))
                .Sum();
            int totalDeaths = playerMatches
                .SelectMany(x => x.Scoreboard.Where(y => y.NameInUpperCase == name).Select(z => z.Deaths))
                .Sum();
            playerStats.KillToDeathRatio = (double)totalKills / totalDeaths;

            return playerStats;
        }
    }
}
