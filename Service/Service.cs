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

            using (var database = new LiteDatabase(Program.databasePath))
            {
                var matchesCollection = database.GetCollection<Match>("matches");
                var serversCollection = database.GetCollection<Classes.Server>("servers");
                Match.AddMatchInfo(matchesCollection, serversCollection, match, endpoint, timestamp);
            }

        }

        public void AddServerInfo(Classes.Server server, string endpoint)
        {
            try
            {
                server.Endpoint = endpoint;
                using (var database = new LiteDatabase(Program.databasePath))
                {
                    var serversCollection = database.GetCollection<Classes.Server>("servers");
                    serversCollection.EnsureIndex(x => x.Endpoint);
                    Classes.Server.AddServerInfo(serversCollection, server, endpoint);
                }
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
            }
        }

        public List<PlayerReport> GetBestPlayers(string count)
        {
            int quantity = Convert.ToInt32(count);
            if (quantity > 50)
                quantity = 50;
            else if (quantity <= 0)
                return new List<PlayerReport>();

            using (var database = new LiteDatabase(Program.databasePath))
            {
                var matchesCollection = database.GetCollection<Match>("matches");
                matchesCollection.EnsureIndex(x => x.Scoreboard.Select(y => y.Name));
                return PlayerReport.GetBestPlayers(matchesCollection, quantity);
            }
        }

        public Match GetMatchInfo(string endpoint, string timestamp)
        {
            using (var database = new LiteDatabase(Program.databasePath))
            {
                var matchesCollection = database.GetCollection<Match>("matches");
                matchesCollection.EnsureIndex(x => x.Endpoint);
                matchesCollection.EnsureIndex(x => x.StringTimestamp);
                return Match.GetMatchInfo(matchesCollection, endpoint, timestamp);
            }
        }

        public PlayerStatistics GetPlayerStats(string name)
        {
            name = name.ToUpper();
            using (var database = new LiteDatabase(Program.databasePath))
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

            using (var database = new LiteDatabase(Program.databasePath))
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
            using (var database = new LiteDatabase(Program.databasePath))
            {
                var matchesCollection = database.GetCollection<Match>("matches");
                matchesCollection.EnsureIndex(x => x.DateTimeTimestamp);
                return MatchReport.GetRecentMatches(matchesCollection, quantity);
            }
        }

        public Classes.Server GetServerInfo(string endpoint)
        {
            using (var database = new LiteDatabase(Program.databasePath))
            {
                var serversCollection = database.GetCollection<Classes.Server>("servers");
                serversCollection.EnsureIndex(x => x.Endpoint);
                return Classes.Server.GetServerInfo(serversCollection, endpoint);
            }
        }

        public List<ServerInfo> GetServersInfo()
        {
            using (var database = new LiteDatabase(Program.databasePath))
            {
                var serversCollection = database.GetCollection<Classes.Server>("servers");
                return ServerInfo.GetServersInfo(serversCollection);
            }
        }

        public ServerStatistics GetServerStats(string endpoint)
        {
            using (var database = new LiteDatabase(Program.databasePath))
            {
                var matchesCollection = database.GetCollection<Match>("matches");
                matchesCollection.EnsureIndex(x => x.Endpoint);
                return ServerStatistics.GetServerStats(matchesCollection, endpoint);
            }
        }

    }
}
