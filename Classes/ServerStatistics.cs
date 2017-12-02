using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using LiteDB;

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

        public static ServerStatistics GetServerStats(LiteCollection<Match> matchesCollection, string endpoint)
        {
            var matches = matchesCollection.Find(x => x.Endpoint == endpoint);
            var result = new ServerStatistics();
            result.TotalMatchesPlayed = matches.Count();

            result.MaximumMatchesPerDay = matches
                .GroupBy(x => x.JustDateFromTimestamp)
                .Max(x => x.Count());

            result.AverageMatchesPerDay = (float)matches
                .GroupBy(x => x.JustDateFromTimestamp)
                .Average(x => x.Count());

            result.MaximumPopulation = matches.Max(x => x.Scoreboard.Count());

            result.AveragePopulation = (float)matches.Average(x => x.Scoreboard.Count());

            result.Top5GameModes = new List<string>(matches
                .GroupBy(x => x.GameMode)
                .OrderByDescending(x => x.Count())
                .Select(x => x.Key)
                .Take(5));

            result.Top5Maps = new List<string>(matches
                .GroupBy(x => x.Map)
                .OrderByDescending(x => x.Count())
                .Select(x => x.Key)
                .Take(5));

            return result;
        }
    }
}
