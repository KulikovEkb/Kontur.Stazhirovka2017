using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using LiteDB;

namespace Kontur.GameStats.Server.ServiceTests
{
    [TestFixture]
    public class GetServerStatsTest
    {
        [SetUp]
        public void InsertData()
        {
            using (var testDatabase = new LiteDatabase(AppDomain.CurrentDomain.BaseDirectory + "TestStorage.db"))
            {
                var serversCollection = testDatabase.GetCollection<Classes.Server>("servers");
                var matchesCollection = testDatabase.GetCollection<Classes.Match>("matches");
                serversCollection.EnsureIndex(x => x.Endpoint);

                Classes.Server.AddServerInfo(serversCollection,
                    new Classes.Server
                    {
                        Name = "] My P3rfect Server [",
                        GameModes = new[] { "DM", "TDM", "CtF" }
                    },
                    "167.42.23.32-1337");

                Classes.Server.AddServerInfo(serversCollection,
                     new Classes.Server
                     {
                         Name = ">> Sniper Heaven <<",
                         GameModes = new[] { "DM" }
                     },
                     "62.210.26.88-1337");

                Classes.Match.AddMatchInfo(matchesCollection,
                    serversCollection,
                    new Classes.Match
                    {
                        Map = "DM-Appalachian Wonderland",
                        GameMode = "TS",
                        FragLimit = 20,
                        TimeLimit = 20,
                        TimeElapsed = 12.345678,
                        Scoreboard = new List<Classes.Player>
                        {
                            new Classes.Player { Name = "Player1", Frags = 20, Kills = 21, Deaths = 3}
                        }
                    },
                    "167.42.23.32-1337",
                    "2017-01-22T15:17:00Z");

                Classes.Match.AddMatchInfo(matchesCollection,
                    serversCollection,
                    new Classes.Match
                    {
                        Map = "DM-Appalachian Wonderland",
                        GameMode = "TDM",
                        FragLimit = 20,
                        TimeLimit = 20,
                        TimeElapsed = 12.345678,
                        Scoreboard = new List<Classes.Player>
                        {
                            new Classes.Player { Name = "Player1", Frags = 20, Kills = 21, Deaths = 3},
                            new Classes.Player { Name = "Player2", Frags = 2, Kills = 2, Deaths = 21}
                        }
                    },
                    "167.42.23.32-1337",
                    "2017-01-22T15:17:00Z");

                Classes.Match.AddMatchInfo(matchesCollection,
                    serversCollection,
                    new Classes.Match
                    {
                        Map = "DM-Appalachian Wonderland",
                        GameMode = "DM",
                        FragLimit = 20,
                        TimeLimit = 20,
                        TimeElapsed = 12.345678,
                        Scoreboard = new List<Classes.Player>
                        {
                            new Classes.Player { Name = "Player1", Frags = 20, Kills = 21, Deaths = 3},
                            new Classes.Player { Name = "Player2", Frags = 2, Kills = 2, Deaths = 21},
                            new Classes.Player { Name = "Player3", Frags = 0, Kills = 0, Deaths = 25}
                        }
                    },
                    "167.42.23.32-1337",
                    "2017-01-22T15:17:00Z");

                Classes.Match.AddMatchInfo(matchesCollection,
                    serversCollection,
                    new Classes.Match
                    {
                        Map = "DM-HelloWorld",
                        GameMode = "CtF",
                        FragLimit = 20,
                        TimeLimit = 20,
                        TimeElapsed = 12.345678,
                        Scoreboard = new List<Classes.Player>
                        {
                            new Classes.Player { Name = "Player1", Frags = 20, Kills = 21, Deaths = 3},
                            new Classes.Player { Name = "Player2", Frags = 19, Kills = 20, Deaths = 4},
                            new Classes.Player { Name = "Player3", Frags = 2, Kills = 2, Deaths = 21},
                            new Classes.Player { Name = "Player4", Frags = 0, Kills = 0, Deaths = 25}
                        }
                    },
                    "167.42.23.32-1337",
                    "2017-01-22T15:17:00Z");

                Classes.Match.AddMatchInfo(matchesCollection,
                    serversCollection,
                    new Classes.Match
                    {
                        Map = "DM-Kitchen",
                        GameMode = "CtF",
                        FragLimit = 20,
                        TimeLimit = 20,
                        TimeElapsed = 12.345678,
                        Scoreboard = new List<Classes.Player>
                        {
                            new Classes.Player { Name = "Player1", Frags = 20, Kills = 21, Deaths = 3},
                            new Classes.Player { Name = "Player2", Frags = 19, Kills = 20, Deaths = 4},
                            new Classes.Player { Name = "Player3", Frags = 2, Kills = 2, Deaths = 21},
                            new Classes.Player { Name = "Player4", Frags = 1, Kills = 1, Deaths = 23},
                            new Classes.Player { Name = "Player5", Frags = 0, Kills = 0, Deaths = 25}
                        }
                    },
                    "167.42.23.32-1337",
                    "2017-01-23T15:17:00Z");

                Classes.Match.AddMatchInfo(matchesCollection,
                    serversCollection,
                    new Classes.Match
                    {
                        Map = "DM-Kitchen",
                        GameMode = "CtF",
                        FragLimit = 20,
                        TimeLimit = 20,
                        TimeElapsed = 12.345678,
                        Scoreboard = new List<Classes.Player>
                        {
                            new Classes.Player { Name = "Player1", Frags = 20, Kills = 21, Deaths = 3},
                            new Classes.Player { Name = "Player2", Frags = 19, Kills = 20, Deaths = 4},
                            new Classes.Player { Name = "Player3", Frags = 19, Kills = 20, Deaths = 4},
                            new Classes.Player { Name = "Player4", Frags = 2, Kills = 2, Deaths = 21},
                            new Classes.Player { Name = "Player5", Frags = 1, Kills = 1, Deaths = 23},
                            new Classes.Player { Name = "Player6", Frags = 0, Kills = 0, Deaths = 25}
                        }
                    },
                    "62.210.26.88-1337",
                    "2017-01-28T15:17:00Z");
            }
        }

        [TearDown]
        public void DropDatabase()
        {
            using (var testDatabase = new LiteDatabase(AppDomain.CurrentDomain.BaseDirectory + "TestStorage.db"))
            {
                testDatabase.DropCollection("matches");
                testDatabase.DropCollection("servers");
            }
        }

        [Test]
        public void TestGetServerStats()
        {
            using (var testDatabase = new LiteDatabase(AppDomain.CurrentDomain.BaseDirectory + "TestStorage.db"))
            {
                var matchesCollection = testDatabase.GetCollection<Classes.Match>("matches");
                matchesCollection.EnsureIndex(x => x.Endpoint);

                var result = Classes.ServerStatistics.GetServerStats(matchesCollection, "167.42.23.32-1337");
                Assert.AreEqual(5, result.TotalMatchesPlayed);
                Assert.AreEqual(4, result.MaximumMatchesPerDay);
                Assert.AreEqual(0.714286, result.AverageMatchesPerDay);
                Assert.AreEqual(5, result.MaximumPopulation);
                Assert.AreEqual(3, result.AveragePopulation);
                Assert.AreEqual(new[] { "CtF", "TS", "TDM", "DM" }, result.Top5GameModes);
                Assert.AreEqual(new[] { "DM-Appalachian Wonderland", "DM-HelloWorld", "DM-Kitchen"}, result.Top5Maps);
            }
        }
    }
}
