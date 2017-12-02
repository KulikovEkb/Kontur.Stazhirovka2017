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
    public class GetMatchTests
    {
        [SetUp]
        public void InsertData()
        {
            using (var testDatabase = new LiteDatabase(AppDomain.CurrentDomain.BaseDirectory + "TestStorage.db"))
            {
                var matchesCollection = testDatabase.GetCollection<Classes.Match>("matches");
                var serversCollection = testDatabase.GetCollection<Classes.Server>("servers");
                serversCollection.EnsureIndex(x => x.Endpoint);

                Classes.Server.AddServerInfo(serversCollection,
                    new Classes.Server
                    {
                        Name = "] My P3rfect Server [",
                        GameModes = new[] { "DM", "TDM" }
                    },
                    "167.42.23.32-1337");

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
        public void TestMatchGetting()
        {
            using (var testDatabase = new LiteDatabase(AppDomain.CurrentDomain.BaseDirectory + "TestStorage.db"))
            {
                var matchesCollection = testDatabase.GetCollection<Classes.Match>("matches");
                var resultMatch = Classes.Match.GetMatchInfo(matchesCollection,"167.42.23.32-1337", "2017-01-22T15:17:00Z");
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
        public void TestMatchGettingWithWrongEndpoint()
        {
            using (var testDatabase = new LiteDatabase(AppDomain.CurrentDomain.BaseDirectory + "TestStorage.db"))
            {
                var matchesCollection = testDatabase.GetCollection<Classes.Match>("matches");
                var exception = Assert.Throws<WebFaultException>(() => Classes.Match.GetMatchInfo(matchesCollection, "167.42.23.32-1337", "0000-00-00T15:17:00Z"));
                Assert.AreEqual("Not Found", exception.Message);
                Assert.AreEqual(typeof(WebFaultException), exception.GetType());
            }
        }

        [Test]
        public void TestMatchGettingWithWrongTimestamp()
        {
            using (var testDatabase = new LiteDatabase(AppDomain.CurrentDomain.BaseDirectory + "TestStorage.db"))
            {
                var matchesCollection = testDatabase.GetCollection<Classes.Match>("matches");
                var exception = Assert.Throws<WebFaultException>(() => Classes.Match.GetMatchInfo(matchesCollection, "127.0.0.1-8080", "2017-01-22T15:17:00Z"));
                Assert.AreEqual("Not Found", exception.Message);
                Assert.AreEqual(typeof(WebFaultException), exception.GetType());
            }
        }
    }
}
