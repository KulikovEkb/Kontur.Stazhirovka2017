using LiteDB;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

namespace Kontur.GameStats.Server.ServiceTests
{
    [TestFixture]
    class AddMatchTest
    {
        [SetUp]
        public static void InsertData()
        {
            using (var testDatabase = new LiteDatabase(AppDomain.CurrentDomain.BaseDirectory + "TestStorage.db"))
            {
                var serversCollection = testDatabase.GetCollection<Classes.Server>("servers");

                Classes.Server.AddServerInfo(serversCollection,
                    new Classes.Server
                    {
                        Name = "] My P3rfect Server [",
                        GameModes = new[] { "DM", "TDM" },
                        Endpoint = "167.42.23.32-1337"
                    },
                    "167.42.23.32-1337");

                serversCollection.EnsureIndex(x => x.Endpoint);
            }
        }

        [TearDown]
        public static void DropDatabase()
        {
            using (var testDatabase = new LiteDatabase(AppDomain.CurrentDomain.BaseDirectory + "TestStorage.db"))
            {
                testDatabase.DropCollection("matches");
                testDatabase.DropCollection("servers");
            }
        }

        [Test]
        public static void TestMatchWith1PlayerAddition()
        {
            using (var testDatabase = new LiteDatabase(AppDomain.CurrentDomain.BaseDirectory + "TestStorage.db"))
            {
                var matchesCollection = testDatabase.GetCollection<Classes.Match>("matches");
                var serversCollection = testDatabase.GetCollection<Classes.Server>("servers");

                Classes.Match.AddMatchInfo(matchesCollection,
                    serversCollection,
                    new Classes.Match
                    {
                        Map = "DM-HelloWorld",
                        GameMode = "DM",
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

                var resultMatch = matchesCollection.FindOne(x => x.Endpoint == "167.42.23.32-1337" && x.StringTimestamp == "2017-01-22T15:17:00Z");
                Assert.AreEqual("DM-HelloWorld", resultMatch.Map);
                Assert.AreEqual("DM", resultMatch.GameMode);
                Assert.AreEqual(20, resultMatch.FragLimit);
                Assert.AreEqual(20, resultMatch.TimeLimit);
                Assert.AreEqual(12.345678, resultMatch.TimeElapsed);
                Assert.AreEqual(1, resultMatch.Scoreboard.Count);
                Assert.AreEqual(100, resultMatch.Scoreboard[0].ScoreboardPercent);
            }
        }

        [Test]
        public static void TestMatchWith2PlayersAddition()
        {
            using (var testDatabase = new LiteDatabase(AppDomain.CurrentDomain.BaseDirectory + "TestStorage.db"))
            {
                var matchesCollection = testDatabase.GetCollection<Classes.Match>("matches");
                var serversCollection = testDatabase.GetCollection<Classes.Server>("servers");

                Classes.Match.AddMatchInfo(matchesCollection,
                    serversCollection,
                    new Classes.Match
                    {
                        Map = "DM-HelloWorld",
                        GameMode = "DM",
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

                var resultMatch = matchesCollection.FindOne(x => x.Endpoint == "167.42.23.32-1337" && x.StringTimestamp == "2017-01-22T15:17:00Z");
                Assert.AreEqual("DM-HelloWorld", resultMatch.Map);
                Assert.AreEqual("DM", resultMatch.GameMode);
                Assert.AreEqual(20, resultMatch.FragLimit);
                Assert.AreEqual(20, resultMatch.TimeLimit);
                Assert.AreEqual(12.345678, resultMatch.TimeElapsed);
                Assert.AreEqual(2, resultMatch.Scoreboard.Count);
                Assert.AreEqual(100, resultMatch.Scoreboard[0].ScoreboardPercent);
                Assert.AreEqual(0, resultMatch.Scoreboard[1].ScoreboardPercent);
            }
        }

        [Test]
        public static void TestMatchWith3PlayersAddition()
        {
            using (var testDatabase = new LiteDatabase(AppDomain.CurrentDomain.BaseDirectory + "TestStorage.db"))
            {
                var matchesCollection = testDatabase.GetCollection<Classes.Match>("matches");
                var serversCollection = testDatabase.GetCollection<Classes.Server>("servers");

                Classes.Match.AddMatchInfo(matchesCollection,
                    serversCollection,
                    new Classes.Match
                    {
                        Map = "DM-HelloWorld",
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

                var resultMatch = matchesCollection.FindOne(x => x.Endpoint == "167.42.23.32-1337" && x.StringTimestamp == "2017-01-22T15:17:00Z");
                Assert.AreEqual("DM-HelloWorld", resultMatch.Map);
                Assert.AreEqual("DM", resultMatch.GameMode);
                Assert.AreEqual(20, resultMatch.FragLimit);
                Assert.AreEqual(20, resultMatch.TimeLimit);
                Assert.AreEqual(12.345678, resultMatch.TimeElapsed);
                Assert.AreEqual(3, resultMatch.Scoreboard.Count);
                Assert.AreEqual(100, resultMatch.Scoreboard[0].ScoreboardPercent);
                Assert.AreEqual(50, resultMatch.Scoreboard[1].ScoreboardPercent);
                Assert.AreEqual(0, resultMatch.Scoreboard[2].ScoreboardPercent);
            }
        }

        [Test]
        public static void TestMatchWith4PlayersAddition()
        {
            using (var testDatabase = new LiteDatabase(AppDomain.CurrentDomain.BaseDirectory + "TestStorage.db"))
            {
                var matchesCollection = testDatabase.GetCollection<Classes.Match>("matches");
                var serversCollection = testDatabase.GetCollection<Classes.Server>("servers");

                Classes.Match.AddMatchInfo(matchesCollection,
                    serversCollection,
                    new Classes.Match
                    {
                        Map = "DM-HelloWorld",
                        GameMode = "DM",
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

                var resultMatch = matchesCollection.FindOne(x => x.Endpoint == "167.42.23.32-1337" && x.StringTimestamp == "2017-01-22T15:17:00Z");
                Assert.AreEqual("DM-HelloWorld", resultMatch.Map);
                Assert.AreEqual("DM", resultMatch.GameMode);
                Assert.AreEqual(20, resultMatch.FragLimit);
                Assert.AreEqual(20, resultMatch.TimeLimit);
                Assert.AreEqual(12.345678, resultMatch.TimeElapsed);
                Assert.AreEqual(4, resultMatch.Scoreboard.Count);
                Assert.AreEqual(100, resultMatch.Scoreboard[0].ScoreboardPercent);
                Assert.AreEqual((double)2 / 3 * 100, resultMatch.Scoreboard[1].ScoreboardPercent);
                Assert.AreEqual((double)1 / 3 * 100, resultMatch.Scoreboard[2].ScoreboardPercent);
                Assert.AreEqual(0, resultMatch.Scoreboard[3].ScoreboardPercent);
            }
        }

        [Test]
        public static void TestMatchAdditionException()
        {
            using (var testDatabase = new LiteDatabase(AppDomain.CurrentDomain.BaseDirectory + "TestStorage.db"))
            {
                var matchesCollection = testDatabase.GetCollection<Classes.Match>("matches");
                var serversCollection = testDatabase.GetCollection<Classes.Server>("servers");
                var exception = Assert.Throws<WebFaultException>(() => Classes.Match.AddMatchInfo(
                    matchesCollection,
                    serversCollection,
                    new Classes.Match
                    {
                        Map = "DM-HelloWorld",
                        GameMode = "DM",
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
                    "127.0.0.1-1338",
                    "2017-01-22T15:17:00Z")
                    );

                Assert.AreEqual("Bad Request", exception.Message);
                Assert.AreEqual(typeof(WebFaultException), exception.GetType());
            }
        }
    }
}
