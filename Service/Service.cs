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
                    float players = match.Scoreboard.Count();
                    foreach (Player item in match.Scoreboard)
                    {
                        item.NameInUpperCase = item.Name.ToUpper();
                        if (players == 1)
                        {
                            item.ScoreboardPercent = 100;
                        }
                        else
                        {
                            item.ScoreboardPercent = (players - (Array.IndexOf(match.Scoreboard, item) + 1)) / (players - 1) * 100;
                        }
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
                var serverCollection = database.GetCollection<Classes.Server>("servers");
                if (serverCollection.FindOne(x => x.Endpoint == endpoint) == null)
                    serverCollection.Insert(server);
                else
                {
                    server.Id = serverCollection.FindOne(x => x.Endpoint == endpoint).Id;
                    serverCollection.Update(server);
                }
            }
        }

        public List<PlayerReport> GetBestPlayers(string count)
        {
            int quantity = Convert.ToInt32(count);
            if (quantity > 50)
                quantity = 50;
            else if (quantity <= 0)
                return new List<PlayerReport>();

            using (var database = new LiteDatabase(@"Storage.db"))
            {
                var matchesCollection = database.GetCollection<Match>("matches");
                matchesCollection.EnsureIndex(x => x.Scoreboard.Select(y => y.Name));
                var bestPlayers = matchesCollection.FindAll()
                    .SelectMany(x => x.Scoreboard.Select(y => new { Name = y.Name, Kills = y.Kills, Deaths = y.Deaths }))
                    .GroupBy(x => x.Name)
                    .Select(x => new { Name = x.Key, KillsSum = x.Sum(y => y.Kills), DeathsSum = x.Sum(y => y.Deaths), PlayedMatches = x.Count() })
                    .Where(x => x.DeathsSum > 0 && x.PlayedMatches >= 10)
                    .Select(x => new { Name = x.Name, KillToDeathRatio = (float)x.KillsSum / x.DeathsSum })
                    .OrderByDescending(x => x.KillToDeathRatio)
                    .Take(quantity);


                var result = new List<PlayerReport>();
                foreach (var item in bestPlayers)
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
                var matchesCollection = database.GetCollection<Match>("matches");

                if (!(matchesCollection.FindOne(x => x.Endpoint == endpoint && x.StringTimestamp == timestamp) == null))
                {
                    matchesCollection.EnsureIndex(x => x.Endpoint);
                    matchesCollection.EnsureIndex(x => x.StringTimestamp);
                    return matchesCollection.FindOne(x => x.Endpoint == endpoint && x.StringTimestamp == timestamp);
                }
                else
                    throw new WebFaultException(System.Net.HttpStatusCode.NotFound);
            }
        }

        public PlayerStatistics GetPlayerStats(string name)
        {
            name = name.ToUpper();
            using (var database = new LiteDatabase(@"Storage.db"))
            {
                var matchesCollection = database.GetCollection<Match>("matches");
                matchesCollection.EnsureIndex(x => x.Scoreboard.Select(y => y.NameInUpperCase));
                var playerMatches = matchesCollection.FindAll()
                    .Where(x => x.Scoreboard.Any(y => y.NameInUpperCase == name));
                var result = new PlayerStatistics();
                result.TotalMatchesPlayed = playerMatches.Count();

                result.TotalMatchesWon = playerMatches
                    .Where(x => x.Scoreboard[0].NameInUpperCase == name)
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
                    .SelectMany(x => x.Scoreboard.Where(y => y.NameInUpperCase == name).Select(z => z.ScoreboardPercent))
                    .Average();

                result.MaximumMatchesPerDay = playerMatches
                    .GroupBy(x => x.JustDateFromTimestamp)
                    .OrderByDescending(x => x.Count())
                    .Select(x => x.Count())
                    .First();

                result.AverageMatchesPerDay = (float)playerMatches
                    .GroupBy(x => x.JustDateFromTimestamp)
                    .Select(x => x.Count())
                    .Average();

                result.LastMatchPlayed = playerMatches
                    .OrderByDescending(x => x.DateTimeTimestamp)
                    .Select(x => x.StringTimestamp)
                    .First();

                var totalKills = playerMatches
                    .SelectMany(x => x.Scoreboard.Where(y => y.NameInUpperCase == name).Select(z => z.Kills))
                    .Sum();
                var totalDeaths = playerMatches
                    .SelectMany(x => x.Scoreboard.Where(y => y.NameInUpperCase == name).Select(z => z.Deaths))
                    .Sum();
                result.KillToDeathRatio = (float)totalKills / totalDeaths;

                return result;
            }
        }

        public List<ServerReport> GetPopularServers(string count)
        {
            int quantity = Convert.ToInt32(count);
            if (quantity > 50)
                quantity = 50;
            else if (quantity <= 0)
                return new List<ServerReport>();

            using (var database = new LiteDatabase(@"Storage.db"))
            {
                var matchesCollection = database.GetCollection<Match>("matches");
                var serversCollection = database.GetCollection<Classes.Server>("servers");
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

        public List<MatchReport> GetRecentMatches(string count)
        {
            int quantity = Convert.ToInt32(count);
            if (quantity > 50)
                quantity = 50;
            else if (quantity <= 0)
                return new List<MatchReport>();
            using (var database = new LiteDatabase(@"Storage.db"))
            {
                var matchesCollection = database.GetCollection<Match>("matches");
                matchesCollection.EnsureIndex(x => x.DateTimeTimestamp);
                var recentMatches = matchesCollection.FindAll().OrderByDescending(x => x.DateTimeTimestamp).Take(quantity);
                var result = new List<MatchReport>(quantity);
                foreach (Match item in recentMatches)
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
                var serversCollection = database.GetCollection<Classes.Server>("servers");
                var servers = serversCollection.FindAll();
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
                var matchesCollection = database.GetCollection<Match>("matches");
                matchesCollection.EnsureIndex(x => x.Endpoint);
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
}
