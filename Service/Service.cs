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
                    match.DateTimeTimestamp = Convert.ToDateTime(timestamp).ToUniversalTime();
                    match.JustDateFromTimestamp = timestamp.Substring(0, 10);
                    int playersCount = match.Scoreboard.Count();

                    for (int i = 0; i < playersCount; i++)
                    {
                        match.Scoreboard[i].NameInUpperCase = match.Scoreboard[i].Name.ToUpper();
                        if (playersCount == 1)
                            match.Scoreboard[i].ScoreboardPercent = 100;
                        else
                            match.Scoreboard[i].ScoreboardPercent = (float)(playersCount - (i + 1)) / (playersCount - 1) * 100;
                    }
                }
                else
                    throw new WebFaultException(System.Net.HttpStatusCode.BadRequest);
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
                return PlayerReport.GetBestPlayers(matchesCollection, quantity);
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
                return PlayerStatistics.GetPlayerStats(matchesCollection, name);
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
                return ServerReport.GetPopularServers(matchesCollection, serversCollection, quantity);
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
                return MatchReport.GetRecentMatches(matchesCollection, quantity);
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
                return ServerStatistics.GetServerStats(matchesCollection, endpoint);
            }
        }

    }
}
