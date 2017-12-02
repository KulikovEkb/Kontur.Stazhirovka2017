using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using LiteDB;

namespace Kontur.GameStats.Server.Classes
{
    [DataContract(Name = "matchReport")]
    public class MatchReport
    {
        [DataMember(Name = "server", Order = 1)]
        public string Server { get; set; }
        [DataMember(Name = "timestamp", Order = 2)]
        public string Timestamp { get; set; }
        [DataMember(Name = "results", Order = 3)]
        public Match Results { get; set; }

        public static List<MatchReport> GetRecentMatches(LiteCollection<Match> matchesCollection, int quantity)
        {
            var recentMatches = matchesCollection.FindAll().OrderByDescending(x => x.DateTimeTimestamp).Take(quantity);
            var result = new List<MatchReport>(quantity);
            foreach (Match item in recentMatches)
            {
                result.Add(new MatchReport { Server = item.Endpoint, Timestamp = item.StringTimestamp, Results = item });
            }

            return result;
        }
    }
}
