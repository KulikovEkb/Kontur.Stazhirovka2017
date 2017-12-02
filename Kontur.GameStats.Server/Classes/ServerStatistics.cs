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
        double averageMatchesPerDay;
        double averagePopulation;

        [DataMember(Name = "totalMatchesPlayed", Order = 1)]
        public int TotalMatchesPlayed { get; set; }
        [DataMember(Name = "maximumMatchesPerDay", Order = 2)]
        public int MaximumMatchesPerDay { get; set; }
        [DataMember(Name = "averageMatchesPerDay", Order = 3)]
        public double AverageMatchesPerDay
        {
            get { return averageMatchesPerDay; }
            set { averageMatchesPerDay = Math.Round(value, 6, MidpointRounding.AwayFromZero); }
        }
        [DataMember(Name = "maximumPopulation", Order = 4)]
        public int MaximumPopulation { get; set; }
        [DataMember(Name = "averagePopulation", Order = 5)]
        public double AveragePopulation
        {
            get { return averagePopulation; }
            set { averagePopulation = Math.Round(value, 6, MidpointRounding.AwayFromZero); }
        }
        [DataMember(Name = "top5GameModes", Order = 6)]
        public List<string> Top5GameModes { get; set; }
        [DataMember(Name = "top5Maps", Order = 7)]
        public List<string> Top5Maps { get; set; }

        public static ServerStatistics GetServerStats(LiteCollection<Match> matchesCollection, string endpoint)
        {
            var matchesOnThatServer = matchesCollection.Find(x => x.Endpoint == endpoint);
            var result = new ServerStatistics();

            result.TotalMatchesPlayed = matchesOnThatServer.Count();

            result.MaximumMatchesPerDay = matchesOnThatServer
                .GroupBy(x => x.JustDateFromTimestamp)
                .Max(x => x.Count());

            int daysOfServer = ((matchesCollection.FindAll().Max(x => x.DateTimeTimestamp).Day) - (matchesOnThatServer.Min(x => x.DateTimeTimestamp).Day)) + 1;
            result.AverageMatchesPerDay = (double)result.TotalMatchesPlayed / daysOfServer;

            result.MaximumPopulation = matchesOnThatServer.Max(x => x.Scoreboard.Count());

            result.AveragePopulation = matchesOnThatServer.Average(x => x.Scoreboard.Count());

            result.Top5GameModes = new List<string>(matchesOnThatServer
                .GroupBy(x => x.GameMode)
                .OrderByDescending(x => x.Count())
                .Select(x => x.Key)
                .Take(5));

            result.Top5Maps = new List<string>(matchesOnThatServer
                .GroupBy(x => x.Map)
                .OrderByDescending(x => x.Count())
                .Select(x => x.Key)
                .Take(5));

            return result;
        }
    }
}
