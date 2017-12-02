using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using LiteDB;

namespace Kontur.GameStats.Server.ServiceTests
{
    //[Ignore("not ready yet")]
    [TestFixture]
    class AddServerTest
    {
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
                serversCollection.EnsureIndex(x => x.Endpoint);
                var gameModes = new[] { "DM", "TDM" };
                Classes.Server.AddServerInfo(serversCollection,
                    new Classes.Server() { Name = "] My P3rfect Server [", GameModes = gameModes, Endpoint = "167.42.23.32-1337" },
                    "167.42.23.32-1337");
                var resultServer = serversCollection.FindOne(x => x.Endpoint == "167.42.23.32-1337");
                Assert.AreEqual("] My P3rfect Server [", resultServer.Name);
                Assert.AreEqual(gameModes, resultServer.GameModes);
            }
        }
    }
}
