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
    class AddServerTest
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
                testDatabase.DropCollection("servers");
            }
        }

        [Test]
        public static void TestServerAddition()
        {
            using (var testDatabase = new LiteDatabase(AppDomain.CurrentDomain.BaseDirectory + "TestStorage.db"))
            {
                var serversCollection = testDatabase.GetCollection<Classes.Server>("servers");
                var resultServer = serversCollection.FindOne(x => x.Endpoint == "167.42.23.32-1337");
                Assert.AreEqual("] My P3rfect Server [", resultServer.Name);
                Assert.AreEqual(new[] { "DM", "TDM" }, resultServer.GameModes);
            }
        }
        [Test]
        public static void TestServerUpdate()
        {
            using (var testDatabase = new LiteDatabase(AppDomain.CurrentDomain.BaseDirectory + "TestStorage.db"))
            {
                var serversCollection = testDatabase.GetCollection<Classes.Server>("servers");
                var resultServer = serversCollection.FindOne(x => x.Endpoint == "167.42.23.32-1337");
                Assert.AreEqual("] My P3rfect Server [", resultServer.Name);
                Assert.AreEqual(new[] { "DM", "TDM" }, resultServer.GameModes);

                Classes.Server.AddServerInfo(serversCollection,
                    new Classes.Server
                    {
                        Name = ">> Sniper Heaven <<",
                        GameModes = new[] { "CtF", "TS", "DM", "TDM" },
                        Endpoint = "167.42.23.32-1337"
                    },
                    "167.42.23.32-1337");

                resultServer = serversCollection.FindOne(x => x.Endpoint == "167.42.23.32-1337");
                Assert.AreEqual(">> Sniper Heaven <<", resultServer.Name);
                Assert.AreEqual(new[] { "CtF", "TS", "DM", "TDM" }, resultServer.GameModes);
            }
        }
        //[Test]
        //public static void TestServerAddition2()
        //{
        //    using (var testDatabase = new LiteDatabase(AppDomain.CurrentDomain.BaseDirectory + "TestStorage.db"))
        //    {
        //        var serversCollection = testDatabase.GetCollection<Classes.Server>("servers");
        //        serversCollection.EnsureIndex(x => x.Endpoint);

        //        var gameModes = new[] { "DM", "TDM" };
        //        Classes.Server.AddServerInfo(serversCollection,
        //            new Classes.Server() { Name = "] My P3rfect Server [", GameModes = gameModes, Endpoint = "167.42.23.32-1337" },
        //            "167.42.23.32-1337");

        //        var resultServer = serversCollection.FindOne(x => x.Endpoint == "167.42.23.32-1337");
        //        Assert.AreEqual("] My P3rfect Server [", resultServer.Name);
        //        Assert.AreEqual(gameModes, resultServer.GameModes);

        //        gameModes = new[] { "CtF", "TS", "DM", "TDM" };
        //        Classes.Server.AddServerInfo(serversCollection,
        //            new Classes.Server() { Name = ">> Sniper Heaven <<", GameModes = gameModes, Endpoint = "167.42.23.32-1338" },
        //            "167.42.23.32-1338");

        //        resultServer = serversCollection.FindOne(x => x.Endpoint == "167.42.23.32-1337");
        //        Assert.AreEqual("] My P3rfect Server [", resultServer.Name);
        //        Assert.AreEqual(new[] { "DM", "TDM" }, resultServer.GameModes);

        //        resultServer = serversCollection.FindOne(x => x.Endpoint == "167.42.23.32-1338");
        //        Assert.AreEqual(">> Sniper Heaven <<", resultServer.Name);
        //        Assert.AreEqual(gameModes, resultServer.GameModes);
        //    }
        //}
    }
}
