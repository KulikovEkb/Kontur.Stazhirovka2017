using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kontur.GameStats.Server.Classes;
using LiteDB;
using System.ServiceModel.Web;
using Serilog;

namespace Kontur.GameStats.Server.Service
{
    class Service : IService
    {
        internal int CheckCount(string count)
        {
            int quantity = Convert.ToInt32(count);
            if (quantity > 50)
                return 50;
            else if (quantity <= 0)
                throw new Exception("Count is less or equal zero");
            else
                return quantity;
        }

        public void AddMatchInfo(Match match, string endpoint, string timestamp)
        {
            try
            {
                using (var database = new LiteDatabase(Program.databasePath))
                {
                    var matchesCollection = database.GetCollection<Match>("matches");
                    var serversCollection = database.GetCollection<Classes.Server>("servers");
                    Match.AddMatchInfo(matchesCollection, serversCollection, match, endpoint, timestamp);
                }
            }
            catch (Exception exc)
            {
                Program.log.Error(exc, "An error occured for HTTP PUT AddMatchInfo");
                throw exc;
            }
        }

        public void AddServerInfo(Classes.Server server, string endpoint)
        {
            try
            {
                using (var database = new LiteDatabase(Program.databasePath))
                {
                    var serversCollection = database.GetCollection<Classes.Server>("servers");
                    serversCollection.EnsureIndex(x => x.Endpoint);
                    Classes.Server.AddServerInfo(serversCollection, server, endpoint);
                }
            }
            catch (Exception exc)
            {
                Program.log.Error(exc, "An error occured for HTTP PUT AddServerInfo");
                throw exc;
            }
        }

        public List<PlayerReport> GetBestPlayers(string count)
        {
            int quantity = 0;
            try
            {
                quantity = CheckCount(count);
            }
            catch (Exception exc)
            {
                if (exc.Message == "Count is less or equal zero")
                    return new List<PlayerReport>();
                else
                {
                    Program.log.Error(exc, "An error occured for HTTP GET GetBestPlayers");
                    throw exc;
                }
            }
            try
            {
                using (var database = new LiteDatabase(Program.databasePath))
                {
                    var matchesCollection = database.GetCollection<Match>("matches");
                    matchesCollection.EnsureIndex(x => x.Scoreboard.Select(y => y.Name));
                    return PlayerReport.GetBestPlayers(matchesCollection, quantity);
                }
            }
            catch (Exception exc)
            {
                Program.log.Error(exc, "An error occured for HTTP GET GetBestPlayers");
                throw exc;
            }
        }

        public Match GetMatchInfo(string endpoint, string timestamp)
        {
            try
            {
                using (var database = new LiteDatabase(Program.databasePath))
                {
                    var matchesCollection = database.GetCollection<Match>("matches");
                    matchesCollection.EnsureIndex(x => x.Endpoint);
                    matchesCollection.EnsureIndex(x => x.StringTimestamp);
                    return Match.GetMatchInfo(matchesCollection, endpoint, timestamp);
                }
            }
            catch (Exception exc)
            {
                Program.log.Error(exc, "An error occured for HTTP GET GetMatchInfo");
                throw exc;
            }
        }

        public PlayerStatistics GetPlayerStats(string name)
        {
            try
            {
                using (var database = new LiteDatabase(Program.databasePath))
                {
                    var matchesCollection = database.GetCollection<Match>("matches");
                    matchesCollection.EnsureIndex(x => x.Scoreboard.Select(y => y.NameInUpperCase));
                    return PlayerStatistics.GetPlayerStats(matchesCollection, name);
                }
            }
            catch (Exception exc)
            {
                Program.log.Error(exc, "An error occured for HTTP GET GetPlayerStats");
                throw exc;
            }
        }

        public List<ServerReport> GetPopularServers(string count)
        {
            int quantity = 0;
            try
            {
                quantity = CheckCount(count);
            }
            catch (Exception exc)
            {
                if (exc.Message == "Count is less or equal zero")
                    return new List<ServerReport>();
                else
                {
                    Program.log.Error(exc, "An error occured for HTTP GET GetPopularServers");
                    throw exc;
                }
            }

            try
            {
                using (var database = new LiteDatabase(Program.databasePath))
                {
                    var matchesCollection = database.GetCollection<Match>("matches");
                    var serversCollection = database.GetCollection<Classes.Server>("servers");
                    return ServerReport.GetPopularServers(matchesCollection, serversCollection, quantity);
                }
            }
            catch (Exception exc)
            {
                Program.log.Error(exc, "An error occured for HTTP GET GetPopularServers");
                throw exc;
            }
        }

        public List<MatchReport> GetRecentMatches(string count)
        {
            int quantity = 0;
            try
            {
                quantity = CheckCount(count);
            }
            catch (Exception exc)
            {
                if (exc.Message == "Count is less or equal zero")
                    return new List<MatchReport>();
                else
                {
                    Program.log.Error(exc, "An error occured for HTTP GET GetRecentMatches");
                    throw exc;
                }
            }

            try
            {
                using (var database = new LiteDatabase(Program.databasePath))
                {
                    var matchesCollection = database.GetCollection<Match>("matches");
                    matchesCollection.EnsureIndex(x => x.DateTimeTimestamp);
                    return MatchReport.GetRecentMatches(matchesCollection, quantity);
                }
            }
            catch (Exception exc)
            {
                Program.log.Error(exc, "An error occured for HTTP GET GetRecentMatches");
                throw exc;
            }
        }

        public Classes.Server GetServerInfo(string endpoint)
        {
            try
            {
                using (var database = new LiteDatabase(Program.databasePath))
                {
                    var serversCollection = database.GetCollection<Classes.Server>("servers");
                    serversCollection.EnsureIndex(x => x.Endpoint);
                    return Classes.Server.GetServerInfo(serversCollection, endpoint);
                }
            }
            catch (Exception exc)
            {
                Program.log.Error(exc, "An error occured for HTTP GET GetServerInfo");
                throw exc;
            }
        }

        public List<ServerInfo> GetServersInfo()
        {
            try
            {
                using (var database = new LiteDatabase(Program.databasePath))
                {
                    var serversCollection = database.GetCollection<Classes.Server>("servers");
                    return ServerInfo.GetServersInfo(serversCollection);
                }
            }
            catch (Exception exc)
            {
                Program.log.Error(exc, "An error occured for HTTP GET GetServersInfo");
                throw exc;
            }
        }

        public ServerStatistics GetServerStats(string endpoint)
        {
            try
            {
                using (var database = new LiteDatabase(Program.databasePath))
                {
                    var matchesCollection = database.GetCollection<Match>("matches");
                    matchesCollection.EnsureIndex(x => x.Endpoint);
                    return ServerStatistics.GetServerStats(matchesCollection, endpoint);
                }
            }
            catch (Exception exc)
            {
                Program.log.Error(exc, "An error occured for HTTP GET GetServerStats");
                throw exc;
            }
        }

    }
}
