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
    public class GetServersTests
    {
        [TearDown]
        public void DropDatabase()
        {
            using (var testDatabase = new LiteDatabase(AppDomain.CurrentDomain.BaseDirectory + "TestStorage.db"))
            {
                testDatabase.DropCollection("servers");
            }
        }

        [Test]
        public void TestServersGetting()
        {
            using (var testDatabase = new LiteDatabase(AppDomain.CurrentDomain.BaseDirectory + "TestStorage.db"))
            {
                var serversCollection = testDatabase.GetCollection<Classes.Server>("servers");
                serversCollection.EnsureIndex(x => x.Endpoint);

                Classes.Server.AddServerInfo(serversCollection,
                    new Classes.Server
                    {
                        Name = "] My P3rfect Server [",
                        GameModes = new[] { "DM", "TDM" }
                    },
                    "167.42.23.32-1337");
                Classes.Server.AddServerInfo(serversCollection,
                     new Classes.Server
                     {
                         Name = ">> Sniper Heaven <<",
                         GameModes = new[] { "DM" }
                     },
                     "62.210.26.88-1337");

                var result = Classes.ServerInfo.GetServersInfo(serversCollection);
                Assert.AreEqual(2, result.Count);
                Assert.AreEqual("] My P3rfect Server [", result[0].Info.Name);
                Assert.AreEqual(new[] { "DM", "TDM" }, result[0].Info.GameModes);
                Assert.AreEqual("167.42.23.32-1337", result[0].Endpoint);
                Assert.AreEqual(">> Sniper Heaven <<", result[1].Info.Name);
                Assert.AreEqual(new[] { "DM" }, result[1].Info.GameModes);
                Assert.AreEqual("62.210.26.88-1337", result[1].Endpoint);
            }
        }
    }
}
