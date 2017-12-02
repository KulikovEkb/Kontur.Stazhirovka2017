using LiteDB;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontur.GameStats.Server.ServiceTests
{
    [TestFixture]
    class GetServerReportTests
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
                            new Classes.Player { Name = "Player 1", Frags = 20, Kills = 21, Deaths = 3}
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
                            new Classes.Player { Name = "Player 1", Frags = 20, Kills = 21, Deaths = 3},
                            new Classes.Player { Name = "ZeroDeaths", Frags = 19, Kills = 20, Deaths = 0},
                            new Classes.Player { Name = "Player 2", Frags = 2, Kills = 2, Deaths = 21}
                        }
                    },
                    "167.42.23.32-1337",
                    "2017-01-22T15:18:00Z");

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
                            new Classes.Player { Name = "Player 1", Frags = 20, Kills = 21, Deaths = 3},
                            new Classes.Player { Name = "ZeroDeaths", Frags = 19, Kills = 20, Deaths = 0},
                            new Classes.Player { Name = "Player 2", Frags = 2, Kills = 2, Deaths = 21},
                            new Classes.Player { Name = "Player 3", Frags = 0, Kills = 0, Deaths = 25}
                        }
                    },
                    "167.42.23.32-1337",
                    "2017-01-22T15:19:00Z");

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
                            new Classes.Player { Name = "Player 1", Frags = 20, Kills = 21, Deaths = 3},
                            new Classes.Player { Name = "ZeroDeaths", Frags = 19, Kills = 20, Deaths = 0},
                            new Classes.Player { Name = "Player 2", Frags = 19, Kills = 20, Deaths = 4},
                            new Classes.Player { Name = "Player 3", Frags = 2, Kills = 2, Deaths = 21},
                            new Classes.Player { Name = "Player 4", Frags = 0, Kills = 0, Deaths = 25}
                        }
                    },
                    "167.42.23.32-1337",
                    "2017-01-22T15:20:00Z");

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
                            new Classes.Player { Name = "Player 1", Frags = 20, Kills = 21, Deaths = 3},
                            new Classes.Player { Name = "ZeroDeaths", Frags = 19, Kills = 20, Deaths = 0},
                            new Classes.Player { Name = "Player 2", Frags = 19, Kills = 20, Deaths = 4},
                            new Classes.Player { Name = "Player 3", Frags = 2, Kills = 2, Deaths = 21},
                            new Classes.Player { Name = "Player 4", Frags = 1, Kills = 1, Deaths = 23},
                            new Classes.Player { Name = "Player 5", Frags = 0, Kills = 0, Deaths = 25}
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
                            new Classes.Player { Name = "Player 1", Frags = 20, Kills = 21, Deaths = 3},
                            new Classes.Player { Name = "ZeroDeaths", Frags = 19, Kills = 20, Deaths = 0},
                            new Classes.Player { Name = "Player 2", Frags = 19, Kills = 20, Deaths = 4},
                            new Classes.Player { Name = "Player 3", Frags = 19, Kills = 20, Deaths = 4},
                            new Classes.Player { Name = "Player 4", Frags = 2, Kills = 2, Deaths = 21},
                            new Classes.Player { Name = "Player 5", Frags = 1, Kills = 1, Deaths = 23},
                            new Classes.Player { Name = "Player 6", Frags = 0, Kills = 0, Deaths = 25}
                        }
                    },
                    "62.210.26.88-1337",
                    "2017-01-28T15:17:00Z");

                Classes.Match.AddMatchInfo(matchesCollection,
                    serversCollection,
                    new Classes.Match
                    {
                        Map = "DM-Kitchen",
                        GameMode = "DM",
                        FragLimit = 20,
                        TimeLimit = 20,
                        TimeElapsed = 12.345678,
                        Scoreboard = new List<Classes.Player>
                        {
                            new Classes.Player { Name = "Player 1", Frags = 20, Kills = 21, Deaths = 3},
                            new Classes.Player { Name = "ZeroDeaths", Frags = 19, Kills = 20, Deaths = 0},
                            new Classes.Player { Name = "Player 2", Frags = 19, Kills = 20, Deaths = 4},
                            new Classes.Player { Name = "Player 3", Frags = 19, Kills = 20, Deaths = 4},
                            new Classes.Player { Name = "Player 4", Frags = 2, Kills = 2, Deaths = 21},
                            new Classes.Player { Name = "Player 5", Frags = 1, Kills = 1, Deaths = 23}
                        }
                    },
                    "62.210.26.88-1337",
                    "2017-01-27T15:17:00Z");

                Classes.Match.AddMatchInfo(matchesCollection,
                    serversCollection,
                    new Classes.Match
                    {
                        Map = "DM-Kitchen",
                        GameMode = "DM",
                        FragLimit = 20,
                        TimeLimit = 20,
                        TimeElapsed = 12.345678,
                        Scoreboard = new List<Classes.Player>
                        {
                            new Classes.Player { Name = "Player 1", Frags = 20, Kills = 21, Deaths = 3},
                            new Classes.Player { Name = "ZeroDeaths", Frags = 19, Kills = 20, Deaths = 0},
                            new Classes.Player { Name = "Player 2", Frags = 19, Kills = 20, Deaths = 4},
                            new Classes.Player { Name = "Player 3", Frags = 19, Kills = 20, Deaths = 4},
                            new Classes.Player { Name = "Player 4", Frags = 2, Kills = 2, Deaths = 21},
                            new Classes.Player { Name = "Player 5", Frags = 1, Kills = 1, Deaths = 23}
                        }
                    },
                    "62.210.26.88-1337",
                    "2017-01-27T15:18:00Z");

                Classes.Match.AddMatchInfo(matchesCollection,
                    serversCollection,
                    new Classes.Match
                    {
                        Map = "DM-Kitchen",
                        GameMode = "DM",
                        FragLimit = 20,
                        TimeLimit = 20,
                        TimeElapsed = 12.345678,
                        Scoreboard = new List<Classes.Player>
                        {
                            new Classes.Player { Name = "Player 1", Frags = 20, Kills = 21, Deaths = 3},
                            new Classes.Player { Name = "ZeroDeaths", Frags = 19, Kills = 20, Deaths = 0},
                            new Classes.Player { Name = "Player 2", Frags = 19, Kills = 20, Deaths = 4},
                            new Classes.Player { Name = "Player 3", Frags = 19, Kills = 20, Deaths = 4},
                            new Classes.Player { Name = "Player 4", Frags = 2, Kills = 2, Deaths = 21},
                            new Classes.Player { Name = "Player 5", Frags = 1, Kills = 1, Deaths = 23}
                        }
                    },
                    "62.210.26.88-1337",
                    "2017-01-29T15:18:00Z");

                Classes.Match.AddMatchInfo(matchesCollection,
                    serversCollection,
                    new Classes.Match
                    {
                        Map = "DM-Kitchen",
                        GameMode = "DM",
                        FragLimit = 20,
                        TimeLimit = 20,
                        TimeElapsed = 12.345678,
                        Scoreboard = new List<Classes.Player>
                        {
                            new Classes.Player { Name = "Player 1", Frags = 20, Kills = 21, Deaths = 3},
                            new Classes.Player { Name = "ZeroDeaths", Frags = 19, Kills = 20, Deaths = 0},
                            new Classes.Player { Name = "Player 2", Frags = 19, Kills = 20, Deaths = 4},
                            new Classes.Player { Name = "Player 3", Frags = 19, Kills = 20, Deaths = 4},
                            new Classes.Player { Name = "Player 4", Frags = 2, Kills = 2, Deaths = 21},
                            new Classes.Player { Name = "Player 5", Frags = 1, Kills = 1, Deaths = 23}
                        }
                    },
                    "62.210.26.88-1337",
                    "2017-01-29T15:19:00Z");

                Classes.Match.AddMatchInfo(matchesCollection,
                    serversCollection,
                    new Classes.Match
                    {
                        Map = "DM-Kitchen",
                        GameMode = "DM",
                        FragLimit = 20,
                        TimeLimit = 20,
                        TimeElapsed = 12.345678,
                        Scoreboard = new List<Classes.Player>
                        {
                            new Classes.Player { Name = "Player 1", Frags = 20, Kills = 21, Deaths = 3},
                            new Classes.Player { Name = "ZeroDeaths", Frags = 19, Kills = 20, Deaths = 0},
                            new Classes.Player { Name = "Player 2", Frags = 19, Kills = 20, Deaths = 4},
                            new Classes.Player { Name = "Player 3", Frags = 19, Kills = 20, Deaths = 4},
                            new Classes.Player { Name = "Player 4", Frags = 2, Kills = 2, Deaths = 21},
                            new Classes.Player { Name = "Player 5", Frags = 1, Kills = 1, Deaths = 23}
                        }
                    },
                    "62.210.26.88-1337",
                    "2017-01-30T15:18:00Z");
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
        public void TestGet1PopularServer()
        {
            using (var database = new LiteDatabase(AppDomain.CurrentDomain.BaseDirectory + "TestStorage.db"))
            {
                var matchesCollection = database.GetCollection<Classes.Match>("matches");
                var serversCollection = database.GetCollection<Classes.Server>("servers");

                var result = Classes.ServerReport.GetPopularServers(matchesCollection, serversCollection, 1);
                Assert.AreEqual(1, result.Count);
                Assert.AreEqual("62.210.26.88-1337", result[0].Endpoint);
                Assert.AreEqual(">> Sniper Heaven <<", result[0].Name);
                Assert.AreEqual(1.5, result[0].AverageMatchesPerDay);
            }
        }

        [Test]
        public void TestGet5PopularServers()
        {
            using (var database = new LiteDatabase(AppDomain.CurrentDomain.BaseDirectory + "TestStorage.db"))
            {
                var matchesCollection = database.GetCollection<Classes.Match>("matches");
                var serversCollection = database.GetCollection<Classes.Server>("servers");

                var result = Classes.ServerReport.GetPopularServers(matchesCollection, serversCollection, 5);
                Assert.AreEqual(2, result.Count);
                Assert.AreEqual("62.210.26.88-1337", result[0].Endpoint);
                Assert.AreEqual(">> Sniper Heaven <<", result[0].Name);
                Assert.AreEqual(1.5, result[0].AverageMatchesPerDay);
                Assert.AreEqual("167.42.23.32-1337", result[1].Endpoint);
                Assert.AreEqual("] My P3rfect Server [", result[1].Name);
                Assert.AreEqual( 0.555556,result[1].AverageMatchesPerDay);
            }
        }
    }
}
