using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using LiteDB;

namespace Kontur.GameStats.Server.Classes
{
    [DataContract(Name = "serverReport")]
    public class ServerReport
    {
        double averageMatchesPerDay;

        [DataMember(Name = "endpoint", Order = 1)]
        public string Endpoint { get; set; }
        [DataMember(Name = "name", Order = 2)]
        public string Name { get; set; }
        [DataMember(Name = "averageMatchesPerDay", Order = 3)]
        public double AverageMatchesPerDay
        {
            get { return averageMatchesPerDay; }
            set { averageMatchesPerDay = Math.Round(value, 6, MidpointRounding.AwayFromZero); }
        }

        public static List<ServerReport> GetPopularServers(LiteCollection<Match> matchesCollection, LiteCollection<Classes.Server> serversCollection, int quantity)
        {
            var result = new List<ServerReport>();

            var serversEndpoints = matchesCollection.FindAll()
                .GroupBy(x => x.Endpoint)
                .Select(x => new { Endpoint = x.Key });

            foreach (var item in serversEndpoints)
            {

                int daysOfServer = ((matchesCollection.FindAll().Max(x => x.DateTimeTimestamp).Day) - (matchesCollection.Find(x => x.Endpoint == item.Endpoint).Min(x => x.DateTimeTimestamp).Day)) + 1;
                result.Add(new ServerReport
                {
                    Endpoint = item.Endpoint,
                    AverageMatchesPerDay = (double)matchesCollection.Find(x => x.Endpoint == item.Endpoint).Count() / daysOfServer,
                    Name = serversCollection.FindOne(x => x.Endpoint == item.Endpoint).Name
                });
            }

            return new List<ServerReport>(result.OrderByDescending(x => x.averageMatchesPerDay).Take(quantity));
        }
    }
}
