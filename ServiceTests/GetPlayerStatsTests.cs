using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using LiteDB;
using System.Web;

namespace Kontur.GameStats.Server.ServiceTests
{
    [TestFixture]
    class GetPlayerStatsTests
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
                            new Classes.Player { Name = "Player 6", Frags = 20, Kills = 21, Deaths = 3},
                            new Classes.Player { Name = "Player 2", Frags = 19, Kills = 20, Deaths = 4},
                            new Classes.Player { Name = "Player 7", Frags = 19, Kills = 20, Deaths = 4},
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
                            new Classes.Player { Name = "Player 6", Frags = 20, Kills = 21, Deaths = 3},
                            new Classes.Player { Name = "Player 2", Frags = 19, Kills = 20, Deaths = 4},
                            new Classes.Player { Name = "Player 7", Frags = 19, Kills = 20, Deaths = 4},
                            new Classes.Player { Name = "Player 4", Frags = 2, Kills = 2, Deaths = 21},
                            new Classes.Player { Name = "Player 5", Frags = 1, Kills = 1, Deaths = 23}
                        }
                    },
                    "62.210.26.88-1337",
                    "2017-01-27T15:18:00Z");
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
        public void TestGetPlayerStatsForPlayer1()
        {
            using (var testDatabase = new LiteDatabase(AppDomain.CurrentDomain.BaseDirectory + "TestStorage.db"))
            {
                var matchesCollection = testDatabase.GetCollection<Classes.Match>("matches");
                matchesCollection.EnsureIndex(x => x.Scoreboard.Select(y => y.NameInUpperCase));

                var result = Classes.PlayerStatistics.GetPlayerStats(matchesCollection, HttpUtility.UrlEncode("player 1"));
                Assert.AreEqual(6, result.TotalMatchesPlayed);
                Assert.AreEqual(6, result.TotalMatchesWon);
                Assert.AreEqual("167.42.23.32-1337", result.FavoriteServer);
                Assert.AreEqual(2, result.UniqueServers);
                Assert.AreEqual("CtF", result.FavoriteGameMode);
                Assert.AreEqual(100, result.AverageScoreboardPercent);
                Assert.AreEqual(4, result.MaximumMatchesPerDay);
                Assert.AreEqual(0.857143, result.AverageMatchesPerDay);
                Assert.AreEqual("2017-01-28T15:17:00Z", result.LastMatchPlayed);
                Assert.AreEqual(7, result.KillToDeathRatio);
            }
        }

        [Test]
        public void TestGetPlayerStatsForPlayer3()
        {
            using (var testDatabase = new LiteDatabase(AppDomain.CurrentDomain.BaseDirectory + "TestStorage.db"))
            {
                var matchesCollection = testDatabase.GetCollection<Classes.Match>("matches");
                matchesCollection.EnsureIndex(x => x.Scoreboard.Select(y => y.NameInUpperCase));

                var result = Classes.PlayerStatistics.GetPlayerStats(matchesCollection, HttpUtility.UrlEncode("Player 3"));
                Assert.AreEqual(4, result.TotalMatchesPlayed);
                Assert.AreEqual(0, result.TotalMatchesWon);
                Assert.AreEqual("167.42.23.32-1337", result.FavoriteServer);
                Assert.AreEqual(2, result.UniqueServers);
                Assert.AreEqual("CtF", result.FavoriteGameMode);
                Assert.AreEqual(35.833333, result.AverageScoreboardPercent);
                Assert.AreEqual(2, result.MaximumMatchesPerDay);
                Assert.AreEqual(0.571429, result.AverageMatchesPerDay);
                Assert.AreEqual("2017-01-28T15:17:00Z", result.LastMatchPlayed);
                Assert.AreEqual(0.338028, result.KillToDeathRatio);
            }
        }

        [Test]
        public void TestGetPlayerStatsForPlayer6()
        {
            using (var testDatabase = new LiteDatabase(AppDomain.CurrentDomain.BaseDirectory + "TestStorage.db"))
            {
                var matchesCollection = testDatabase.GetCollection<Classes.Match>("matches");
                matchesCollection.EnsureIndex(x => x.Scoreboard.Select(y => y.NameInUpperCase));

                var result = Classes.PlayerStatistics.GetPlayerStats(matchesCollection, HttpUtility.UrlEncode("PLAYER 6"));
                Assert.AreEqual(3, result.TotalMatchesPlayed);
                Assert.AreEqual(2, result.TotalMatchesWon);
                Assert.AreEqual("62.210.26.88-1337", result.FavoriteServer);
                Assert.AreEqual(1, result.UniqueServers);
                Assert.AreEqual("DM", result.FavoriteGameMode);
                Assert.AreEqual(66.666667, result.AverageScoreboardPercent);
                Assert.AreEqual(2, result.MaximumMatchesPerDay);
                Assert.AreEqual(1.5, result.AverageMatchesPerDay);
                Assert.AreEqual("2017-01-28T15:17:00Z", result.LastMatchPlayed);
                Assert.AreEqual(1.354839, result.KillToDeathRatio);
            }
        }
    }
}
