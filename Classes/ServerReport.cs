﻿using System;
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
        [DataMember(Name = "endpoint", Order = 1)]
        public string Endpoint { get; set; }
        [DataMember(Name = "name", Order = 2)]
        public string Name { get; set; }
        [DataMember(Name = "averageMatchesPerDay", Order = 3)]
        public float AverageMatchesPerDay { get; set; }

        public static List<ServerReport> GetPopularServers(LiteCollection<Match> matchesCollection, LiteCollection<Classes.Server> serversCollection, int quantity)
        {
            var tempResult = new List<ServerReport>();

            var serverEndpoints = matchesCollection.FindAll()
                .GroupBy(x => x.Endpoint)
                .Select(x => new { Endpoint = x.Key });

            foreach (var item in serverEndpoints)
            {
                tempResult.Add(new ServerReport
                {
                    Endpoint = item.Endpoint,
                    AverageMatchesPerDay = (float)matchesCollection.FindAll()
                .Where(x => x.Endpoint == item.Endpoint)
                .GroupBy(x => x.JustDateFromTimestamp)
                .Select(x => x.Count()).Average(),
                    Name = serversCollection.FindOne(x => x.Endpoint == item.Endpoint).Name
                });
            }

            IEnumerable<ServerReport> orderedTempResult = tempResult.OrderByDescending(x => x.AverageMatchesPerDay).Take(quantity);
            var result = new List<ServerReport>(orderedTempResult);
            return result;
        }
    }
}
