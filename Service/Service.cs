using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kontur.GameStats.Server.Classes;
using LiteDB;
using System.ServiceModel.Web;

namespace Kontur.GameStats.Server.Service
{
    class Service : IService
    {
        public void AddMatchInfo(Match match, string endpoint, string timestamp)
        {

            using (var database = new LiteDatabase(@"Storage.db"))
            {
                var matchesCollection = database.GetCollection<Match>("matches");

                var serversCollection = database.GetCollection<Classes.Server>("servers");
                if (!(serversCollection.FindOne(x => x.Endpoint == endpoint) == null))
                {
                    match.Endpoint = endpoint;
                    match.StringTimestamp = timestamp;
                    match.DateTimeTimestamp = Convert.ToDateTime(timestamp);
                    match.JustDateFromTimestamp = timestamp.Substring(0, 10);
                    double players = match.Scoreboard.Count();
                    foreach (Player item in match.Scoreboard)
                    {
                        if (players == 1)
                            item.ScoreboardPercent = 100;
                        else
                            item.ScoreboardPercent = (players - (Array.IndexOf(match.Scoreboard, item) + 1)) / (players - 1) * 100;
                    }

                    matchesCollection.Insert(match);
                }
                else
                {
                    throw new WebFaultException(System.Net.HttpStatusCode.BadRequest);
                }
            }
        }

        public void AddServerInfo(Classes.Server server, string endpoint)
        {

            server.Endpoint = endpoint;
            using (var database = new LiteDatabase(@"Storage.db"))
            {
                var userCollection = database.GetCollection<Classes.Server>("servers");
                if (userCollection.FindOne(x => x.Endpoint == endpoint) == null)
                    userCollection.Insert(server);
                else
                {
                    server.Id = userCollection.FindOne(x => x.Endpoint == endpoint).Id;
                    userCollection.Update(server);
                }
            }
        }

        public List<PlayerReport> GetBestPlayers(string count)
        {
            int quantity = Convert.ToInt32(count);
            quantity = quantity > 50 ? 50 : quantity;
            using (var database = new LiteDatabase(@"Storage.db"))
            {
                var userCollection = database.GetCollection<Match>("matches");
                userCollection.EnsureIndex(x => x.Scoreboard.Select(y => y.Name));
                var playersNames = userCollection.FindAll()
                    .SelectMany(x => x.Scoreboard.Select(y => new { Name = y.Name, Kills = y.Kills, Deaths = y.Deaths }))
                    .GroupBy(x => x.Name)
                    .Select(x => new { Name = x.Key, KillsSum = x.Sum(y => y.Kills), DeathsSum = x.Sum(y => y.Deaths), PlayedMatches = x.Count() })
                    .Where(x => x.DeathsSum > 0 && x.PlayedMatches >= 10)
                    .Select(x => new { Name = x.Name, KillToDeathRatio = (double)x.KillsSum / x.DeathsSum })
                    .OrderByDescending(x => x.KillToDeathRatio)
                    .Take(quantity);


                var result = new List<PlayerReport>();
                foreach (var item in playersNames)
                {
                    result.Add(new PlayerReport { KillToDeathRatio = item.KillToDeathRatio, Name = item.Name });
                }
                return result;
            }
        }

        public Match GetMatchInfo(string endpoint, string timestamp)
        {
            using (var database = new LiteDatabase(@"Storage.db"))
            {
                var matchCollection = database.GetCollection<Match>("matches");

                if (!(matchCollection.FindOne(x => x.Endpoint == endpoint && x.StringTimestamp == timestamp) == null))
                {
                    matchCollection.EnsureIndex(x => x.Endpoint);
                    matchCollection.EnsureIndex(x => x.StringTimestamp);
                    return matchCollection.FindOne(x => x.Endpoint == endpoint && x.StringTimestamp == timestamp);
                }
                else
                    throw new WebFaultException(System.Net.HttpStatusCode.NotFound);
            }
        }

        public PlayerStatistics GetPlayerStats(string name)
        {
            using (var database = new LiteDatabase(@"Storage.db"))
            {
                var userCollection = database.GetCollection<Match>("matches");
                userCollection.EnsureIndex(x => x.Scoreboard.Select(y => y.Name));
                var playerMatches = userCollection.FindAll()
                    .Where(x => x.Scoreboard.Any(y => y.Name == name));
                var result = new PlayerStatistics();
                result.TotalMatchesPlayed = playerMatches.Count();

                result.TotalMatchesWon = playerMatches
                    .Where(x => x.Scoreboard[0].Name == name)
                    .Count();

                result.FavoriteServer = playerMatches
                    .GroupBy(x => x.Endpoint)
                    .OrderByDescending(x => x.Count())
                    .Select(x => x.Key)
                    .First();

                result.UniqueServers = playerMatches
                    .GroupBy(x => x.Endpoint)
                    .Count();

                result.FavoriteGameMode = playerMatches
                    .GroupBy(x => x.GameMode)
                    .OrderByDescending(x => x.Count())
                    .Select(x => x.Key)
                    .First();

                result.AverageScoreboardPercent = playerMatches
                    .SelectMany(x => x.Scoreboard.Where(y => y.Name == name).Select(z => z.ScoreboardPercent))
                    .Average();

                result.MaximumMatchesPerDay = playerMatches
                    .GroupBy(x => x.JustDateFromTimestamp)
                    .OrderByDescending(x => x.Count())
                    .Select(x => x.Count())
                    .First();

                result.AverageMatchesPerDay = playerMatches
                    .GroupBy(x => x.JustDateFromTimestamp)
                    .Select(x => x.Count())
                    .Average();

                result.LastMatchPlayed = playerMatches
                    .OrderByDescending(x => x.DateTimeTimestamp)
                    .Select(x => x.StringTimestamp)
                    .First();

                var totalKills = playerMatches
                    .SelectMany(x => x.Scoreboard.Where(y => y.Name == name).Select(z => z.Kills))
                    .Sum();
                var totalDeaths = playerMatches
                    .SelectMany(x => x.Scoreboard.Where(y => y.Name == name).Select(z => z.Deaths))
                    .Sum();
                result.KillToDeathRatio = (double)totalKills / totalDeaths;

                return result;
            }
        }

        public List<ServerReport> GetPopularServers(string count)
        {
            int quantity = Convert.ToInt32(count);
            quantity = quantity > 50 ? 50 : quantity;
            using (var database = new LiteDatabase(@"Storage.db"))
            {
                var userCollection = database.GetCollection<Match>("matches");
                var servers = database.GetCollection<Classes.Server>("servers");
                var result = new List<ServerReport>();

                var serverEndpoints = userCollection.FindAll()
                    .GroupBy(x => x.Endpoint)
                    .Select(x => new { Endpoint = x.Key });

                foreach (var item in serverEndpoints)
                {
                    result.Add(new ServerReport
                    {
                        Endpoint = item.Endpoint,
                        AverageMatchesPerDay = userCollection.FindAll()
                    .Where(x => x.Endpoint == item.Endpoint)
                    .GroupBy(x => x.JustDateFromTimestamp)
                    .Select(x => x.Count()).Average(),
                        Name = servers.FindOne(x => x.Endpoint == item.Endpoint).Name
                    });
                }

                IEnumerable<ServerReport> asd = result.OrderByDescending(x => x.AverageMatchesPerDay).Take(quantity);
                var xxx = new List<ServerReport>(asd);
                return xxx;
            }
        }

        public List<MatchReport> GetRecentMatches(string count)
        {
            int quantity = Convert.ToInt32(count);
            using (var database = new LiteDatabase(@"Storage.db"))
            {
                var userCollection = database.GetCollection<Match>("matches");
                userCollection.EnsureIndex(x => x.DateTimeTimestamp);
                var matches = userCollection.FindAll().OrderByDescending(x => x.DateTimeTimestamp).Take(quantity);
                var result = new List<MatchReport>(quantity);
                foreach (Match item in matches)
                {
                    result.Add(new MatchReport { Server = item.Endpoint, Timestamp = item.StringTimestamp, Results = item });
                }

                return result;
            }
        }

        public Classes.Server GetServerInfo(string endpoint)
        {
            using (var database = new LiteDatabase(@"Storage.db"))
            {
                var serversCollection = database.GetCollection<Classes.Server>("servers");
                serversCollection.EnsureIndex(x => x.Endpoint);
                if (!(serversCollection.FindOne(x => x.Endpoint == endpoint) == null))
                    return serversCollection.FindOne(x => x.Endpoint == endpoint);
                else
                    throw new WebFaultException(System.Net.HttpStatusCode.NotFound);
            }
        }

        public List<ServerInfo> GetServersInfo()
        {
            using (var database = new LiteDatabase(@"Storage.db"))
            {
                var userCollection = database.GetCollection<Classes.Server>("servers");
                var servers = userCollection.FindAll();
                var result = new List<ServerInfo>();
                foreach (Classes.Server item in servers)
                {
                    result.Add(new ServerInfo { Endpoint = item.Endpoint, Info = item });
                }
                return result;
            }
        }

        public ServerStatistics GetServerStats(string endpoint)
        {
            using (var database = new LiteDatabase(@"Storage.db"))
            {
                var userCollection = database.GetCollection<Match>("matches");
                userCollection.EnsureIndex(x => x.Endpoint);
                var matches = userCollection.Find(x => x.Endpoint == endpoint);
                var result = new ServerStatistics();
                result.TotalMatchesPlayed = matches.Count();

                result.MaximumMatchesPerDay = matches
                    .GroupBy(x => x.JustDateFromTimestamp)
                    .Max(x => x.Count());

                result.AverageMatchesPerDay = matches
                    .GroupBy(x => x.JustDateFromTimestamp)
                    .Average(x => x.Count());

                result.MaximumPopulation = matches.Max(x => x.Scoreboard.Count());

                result.AveragePopulation = matches.Average(x => x.Scoreboard.Count());

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
}
